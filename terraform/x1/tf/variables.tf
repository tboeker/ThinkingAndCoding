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
  default     = "rg_my1"
  description = "the resource group"
}

variable "acrname" {
  type        = string
  default     = "myacr1"
  description = "the azure container registry name"
}


variable "tags" {
  type        = map
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}