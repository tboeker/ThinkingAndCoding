provider "azurerm" { 
  features {}
}

resource "azurerm_resource_group" "resg" {
  name     = var.rgname
  location = var.location
  tags     = var.tags
}