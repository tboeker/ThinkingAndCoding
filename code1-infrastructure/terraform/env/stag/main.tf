# provider "azurerm" { 
#   version = "=2.20.0"
#   features {}
# }

module "shared" {
  source = "../modules/env-shared"

  appname = "tb-code1"
  appshort = "tbcode1"
  envshort = "stag"
}
