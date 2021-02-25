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

resource "azurerm_key_vault" "the-key-vault" {
  name                        = "the-key-vault"
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