output "vnet_id" {
  value = "${azurerm_virtual_network.vnet.id}"
}

output "vnet_name" {
   value = "${azurerm_virtual_network.vnet.name}"
}

output "vnet_resource_group_name" {
   value = "${azurerm_virtual_network.vnet.resource_group_name}"
}

output "keyvault_id" {
  value = "${azurerm_key_vault.keyvault.id}"
}

output "location" {
  value = "${azurerm_resource_group.rg.location}"
}
