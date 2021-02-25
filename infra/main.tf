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
