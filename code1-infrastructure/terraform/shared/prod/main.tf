# provider "azurerm" { 
#   version = "=2.20.0"
#   features {}
# }

module "app-shared" {
  source = "github.com/tboeker/terraform-azure-app-shared"

  appname = "tb-code1"
  appshort = "tbcode1"
}
