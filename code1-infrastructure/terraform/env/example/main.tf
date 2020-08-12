provider "azurerm" { 
  version = "=2.20.0"
  features {}
}

terraform {
  backend "azurerm" {   
    storage_account_name = "tftbcode1example"
    container_name       = "example"
    key                  = "terraform.tfstate"
  }
}

module "shared" {
  source = "../modules/env-shared"

  appname = "tb-code1"
  appshort = "tbcode1"
  envshort = "example"
}


# module "eventstore" {
#   source = "../modules/env-eventstore"

#   appname = "tb-code1"
#   appshort = "tbcode1"
#   envshort = "example"

#   yoda_password = var.EVENTSTORE_YODA_PASSWORD
  
#   keyvault_id               = module.shared.keyvault_id
#   vnet_id                   = module.shared.vnet_id
#   vnet_name                 = module.shared.vnet_name
#   vnet_resource_group_name  = module.shared.vnet_resource_group_name

#   location      = module.shared.location
# }
