terraform {
  required_version = ">= 0.14"
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "pact-broker-rg" {
  name     = "pact-broker-rg"
  location = "eastus2"
}

resource "azurerm_virtual_network" "pact-broker-vn" {
  name                = "pact-broker-vn"
  address_space       = ["10.0.0.0/16"]
  location            = azurerm_resource_group.pact-broker-rg.location
  resource_group_name = azurerm_resource_group.pact-broker-rg.name
}

resource "azurerm_subnet" "postgres-sn" {
  name                 = "postgres-sn"
  resource_group_name  = azurerm_resource_group.pact-broker-rg.name
  virtual_network_name = azurerm_virtual_network.pact-broker-vn.name
  address_prefix       = "10.0.1.0/24"

  enforce_private_link_endpoint_network_policies = true
}

data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "key-vault" {
  name                        = "svasquez-key-vault-01"
  location                    = azurerm_resource_group.pact-broker-rg.location
  resource_group_name         = azurerm_resource_group.pact-broker-rg.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "get",
    ]

    secret_permissions = [
      "get", "backup", "delete", "list", "purge", "recover", "restore", "set",
    ]

    storage_permissions = [
      "get",
    ]
  } 
}

resource "random_password" "postgres-password" {
  length  = 21
  special = true
}

resource "azurerm_key_vault_secret" "postgres-password-secret" {
  name = "postgres-password-secret"
  value = random_password.postgres-password.result
  key_vault_id = azurerm_key_vault.key-vault.id
}

resource "azurerm_postgresql_server" "postgres" {
  name                 = "svasquez-postgres-server-01"
  location             = azurerm_resource_group.pact-broker-rg.location
  resource_group_name  = azurerm_resource_group.pact-broker-rg.name
  
  sku_name = "GP_Gen5_1"

  storage_profile {
    storage_mb            = 5120
    backup_retention_days = 7
    geo_redundant_backup  = "Disabled"
    auto_grow             = "Enabled"
  }

  administrator_login          = "sa"
  administrator_login_password = azurerm_key_vault_secret.postgres-password-secret.value
  version                      = "11"
  ssl_enforcement_enabled      = true
}

resource "azurerm_private_endpoint" "postgres-pe" {
  name                 = "postgres-private-endpoint"
  location             = azurerm_resource_group.pact-broker-rg.location
  resource_group_name  = azurerm_resource_group.pact-broker-rg.name
  subnet_id            = azurerm_subnet.postgres-sn.id

  private_service_connection {
    name                           = "postgres-private-service-connection"
    private_connection_resource_id = azurerm_postgresql_server.postgres.id
    subresource_names              = [ "postgresqlServer" ]
    is_manual_connection           = false
  }
}
