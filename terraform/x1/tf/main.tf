provider "azurerm" { 
  features {}
}

resource "azurerm_resource_group" "resg" {
  name     = "terraform-group"
  location = var.location
  tags     = var.tags
}