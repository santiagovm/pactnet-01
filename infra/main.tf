terraform {
  required_version = ">= 0.14"
  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}

resource "random_string" "random" {
  length = 6
  special = false
  upper = false
}
