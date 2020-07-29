resource "azurerm_resource_group" "rg" {
  name     = "rg-${var.appname}-${var.envshort}-eventstore"
  location = var.location
  tags     = var.tags
}

data "azurerm_client_config" "current" {
}

data "azurerm_virtual_network" "vnet" {
  name                = var.vnet_name
  resource_group_name = var.vnet_resource_group_name
}

resource "azurerm_key_vault_secret" "keyvault_vm_password" {
  name         = "EventStore--YodaPassword"
  value        = var.yoda_password
  key_vault_id = var.keyvault_id

  depends_on = [
    var.keyvault_id
  ]

  tags     = var.tags
}

resource "azurerm_subnet" "eventstore" {
  name                 = "SubnetEventstore"
  resource_group_name  = var.vnet_resource_group_name
  virtual_network_name = var.vnet_name
  address_prefixes     = ["10.0.1.0/24"]

  depends_on = [
    var.vnet_id
  ]
}


# resource "azurerm_storage_account" "storage1" {
#  name                   = "st${var.appshort}${var.envshort}001"
#   location              = azurerm_resource_group.rg.location
#   resource_group_name   = azurerm_resource_group.rg.name

#   account_tier             = "Standard"
#   account_replication_type = "RAGRS"

#   tags = {
#     environment = "staging"
#   }
# }


# resource "azurerm_key_vault_secret" "keyvault_storage1_connectionstring" {
#   name         = "AzureStorage--ConnectionString"
#   value        = azurerm_storage_account.storage1.primary_connection_string
#   key_vault_id = azurerm_key_vault.keyvault.id

#   depends_on = [
#     azurerm_key_vault_access_policy.keyvault_default_policy,
#     azurerm_storage_account.storage1
#   ]

#   tags     = var.tags
# }


# resource "azurerm_storage_container" "storage1_container" {
#   name                  = "storage"
#   storage_account_name  = azurerm_storage_account.storage1.name
#   container_access_type = "private"

#   depends_on = [
#     azurerm_storage_account.storage1
#   ]
# }



# resource "azurerm_storage_account" "storage2" {
#  name                   = "st${var.appshort}${var.envshort}002"
#   location              = azurerm_resource_group.rg.location
#   resource_group_name   = azurerm_resource_group.rg.name

#   account_tier             = "Standard"
#   account_replication_type = "RAGRS"
#   allow_blob_public_access = true

#   tags     = var.tags
# }

# resource "azurerm_key_vault_secret" "keyvault_storage2_connectionstring" {
#   name         = "AzureStorage2--ConnectionString"
#   value        = azurerm_storage_account.storage2.primary_connection_string
#   key_vault_id = azurerm_key_vault.keyvault.id

#   depends_on = [
#     azurerm_key_vault_access_policy.keyvault_default_policy,
#     azurerm_storage_account.storage2
#   ]

#   tags     = var.tags
# }

# resource "azurerm_storage_container" "storage2_downloads_container" {
#   name                  = "downloads"
#   storage_account_name  = azurerm_storage_account.storage2.name
#   container_access_type = "container"

#   depends_on = [
#     azurerm_storage_account.storage2
#   ]
# }


# resource "azurerm_virtual_network" "vnet" {
#   name                = "vnet-${var.appname}-${var.envshort}"
#   location            = azurerm_resource_group.rg.location
#   resource_group_name = azurerm_resource_group.rg.name
#   address_space       = [ var.vnetaddressspace ]
# }