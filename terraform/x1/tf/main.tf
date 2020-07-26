provider "azurerm" { 
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.resource_prefix}_${var.rgname}"
  location = var.location
  tags     = var.tags
}

resource "azurerm_container_registry" "acr" {
  name                     = "${var.resource_prefix}_${var.acrname}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  # sku                      = "Premium"
  admin_enabled            = true
}