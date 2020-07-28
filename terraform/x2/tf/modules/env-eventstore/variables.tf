variable "location" {
  type        = string
  default     = "westeurope"
  description = "Specify a location see: az account list-locations -o table"
}

variable "appname" {
  type = string
  default = "tb-code1"
  description = "the long name of the app: tb-code1"
}

variable "appshort" {
  type = string
  default = "tbcode1"
  description = "the short name of the app: tbcode1"
}

variable "envshort" {
  type        = string
  default     = "dev"
  description = "the short name of the environment"
}


variable "vnetid" {
  type        = string
  default     = ""
  description = "the id of the shared virtual network"
}


variable "tags" {
  type        = map
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}