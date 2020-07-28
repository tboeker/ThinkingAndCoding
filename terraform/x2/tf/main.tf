provider "azurerm" { 
  version = "=2.20.0"
  features {}
}


module "app-shared" {
  source = "github.com/tboeker/terraform-azure-app-shared"

  appname = "tb-code2"
  appshort = "tbcode2"
}


module "env-shared" {
  source = "./modules/env-shared"

  appname = "tb-code2"
  appshort = "tbcode2"
  envshort = "dev"
}



