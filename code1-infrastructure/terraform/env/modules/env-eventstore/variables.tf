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


variable "keyvault_id" {
  type        = string
  default     = ""
  description = "the id of the shared keyvault"
}


variable "vnet_id" {
  type        = string
  default     = ""
  description = "the id of the shared virtual network"
}

variable "vnet_name" {
  type        = string
  default     = ""
  description = "the name of the shared virtual network"
}

variable "vnet_resource_group_name" {
  type        = string
  default     = ""
  description = "the name of the shared virtual network resource group"
}

variable "yoda_password" {
  type        = string
  default     = ""
  description = "the password of the yoda admin user"
}

variable "tags" {
  type        = map
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}