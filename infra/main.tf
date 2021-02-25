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
  location            = "eastus2"
  resource_group_name = azurerm_resource_group.pact-broker-rg.name
}

resource "azurerm_subnet" "postgres-sn" {
  name                 = "postgres-sn"
  resource_group_name  = azurerm_resource_group.pact-broker-rg.name
  virtual_network_name = azurerm_virtual_network.pact-broker-vn.name
  address_prefix       = "10.0.1.0/24"

  enforce_private_link_endpoint_network_policies = true
}
