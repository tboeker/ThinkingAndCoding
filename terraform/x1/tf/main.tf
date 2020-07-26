provider "azurerm" { 
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.resource_prefix}_${var.rgname}"
  location = var.location
  tags     = var.tags
}

resource "azurerm_container_registry" "acr" {
  name                     = "${var.resource_prefix}${var.acrname}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  # sku                      = "Premium"
  admin_enabled            = true  
  tags     = var.tags
}

data "azurerm_client_config" "current" {
}

resource "azurerm_key_vault" "kv" {
  name                = "${var.resource_prefix}_${var.kvname}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tenant_id           = data.azurerm_client_config.current.tenant_id

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "create",
      "get",
    ]

    secret_permissions = [
      "set",
      "get",
      "delete",
    ]
  }

  tags     = var.tags
}