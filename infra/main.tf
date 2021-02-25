terraform {
  required_version = ">= 0.14"
  backend "azurerm" {}
}

provider "azurerm" {
  # The "feature" block is required for AzureRM provider 2.x.
  # If you're using version 1.x, the "features" block is not allowed.
  version = "~>2.0"
  features {}
}

resource "random_string" "random" {
  length = 6
  special = false
  upper = false
}
