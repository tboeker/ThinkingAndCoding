provider "azurerm" { 
  version = "=2.20.0"
  features {}
}

module "dev-shared" {
  source = "../modules/env-shared"

  appname = "tb-code1-ex"
  appshort = "tbcode1ex"
  envshort = "example"
}
