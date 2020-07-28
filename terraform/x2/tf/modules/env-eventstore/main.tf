resource "azurerm_resource_group" "rg" {
  name     = "rg-${var.appname}-${var.envshort}-eventstore"
  location = var.location
  tags     = var.tags
}

data "azurerm_client_config" "current" {
}


resource "azurerm_key_vault_access_policy" "keyvault_default_policy" {
  key_vault_id = azurerm_key_vault.keyvault.id

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

resource "azurerm_key_vault_access_policy" "keyvault_default_developer" {
  key_vault_id = azurerm_key_vault.keyvault.id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = var.devgroupid

  key_permissions = [
      "create",
      "get",
      "list"
  ]

  secret_permissions = [
      "set",
      "get",
      "delete",
      "list"
  ]
}

resource "azurerm_storage_account" "storage1" {
 name                   = "st${var.appshort}${var.envshort}001"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name

  account_tier             = "Standard"
  account_replication_type = "RAGRS"

  tags = {
    environment = "staging"
  }
}


resource "azurerm_key_vault_secret" "keyvault_storage1_connectionstring" {
  name         = "AzureStorage--ConnectionString"
  value        = azurerm_storage_account.storage1.primary_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id

  depends_on = [
    azurerm_key_vault_access_policy.keyvault_default_policy,
    azurerm_storage_account.storage1
  ]

  tags     = var.tags
}


resource "azurerm_storage_container" "storage1_container" {
  name                  = "storage"
  storage_account_name  = azurerm_storage_account.storage1.name
  container_access_type = "private"

  depends_on = [
    azurerm_storage_account.storage1
  ]
}



resource "azurerm_storage_account" "storage2" {
 name                   = "st${var.appshort}${var.envshort}002"
  location              = azurerm_resource_group.rg.location
  resource_group_name   = azurerm_resource_group.rg.name

  account_tier             = "Standard"
  account_replication_type = "RAGRS"
  allow_blob_public_access = true

  tags     = var.tags
}

resource "azurerm_key_vault_secret" "keyvault_storage2_connectionstring" {
  name         = "AzureStorage2--ConnectionString"
  value        = azurerm_storage_account.storage2.primary_connection_string
  key_vault_id = azurerm_key_vault.keyvault.id

  depends_on = [
    azurerm_key_vault_access_policy.keyvault_default_policy,
    azurerm_storage_account.storage2
  ]

  tags     = var.tags
}

resource "azurerm_storage_container" "storage2_downloads_container" {
  name                  = "downloads"
  storage_account_name  = azurerm_storage_account.storage2.name
  container_access_type = "container"

  depends_on = [
    azurerm_storage_account.storage2
  ]
}


resource "azurerm_virtual_network" "vnet" {
  name                = "vnet-${var.appname}-${var.envshort}"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  address_space       = [ var.vnetaddressspace ]
}