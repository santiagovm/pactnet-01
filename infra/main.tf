terraform {
  required_version = ">= 0.14"
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}

resource "azurerm_virtual_network" "pact-broker-vn" {
  name                = "pact-broker-vn"
  address_space       = ["10.0.0.0/16"]
  location            = "eastus2"
  resource_group_name = "pact-broker-rg"
}
