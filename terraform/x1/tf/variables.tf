variable "location" {
  type        = string
  default     = "westeurope"
  description = "Specify a location see: az account list-locations -o table"
}

variable "resource_prefix" {
  type = string
  default = "tb1"
}

variable "rgname" {
  type        = string
  default     = "{var.resource_prefix}_rg_my1"
  description = "the resource group"
}

variable "acrname" {
  type        = string
  default     = "{var.resource_prefix}myacr1"
  description = "the azure container registry name"
}


variable "tags" {
  type        = map
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}