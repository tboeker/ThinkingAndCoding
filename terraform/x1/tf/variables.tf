variable "ENVIRONMENTNAME" {
  type = string
  default = "DevRoot"
}

variable "location" {
  type        = string
  default     = "westeurope"
  description = "Specify a location see: az account list-locations -o table"
}

variable "RESOURCE_PREFIX" {
  type = string
  default = "tb2"
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


variable "kvname" {
  type        = string
  default     = "mykv"
  description = "the azure keyvault name"
}

variable "devgroupid" {
  type        = string
  default     = "a11cdfb2-0654-472b-a1e3-55c4748a1f01"
  description = "the developer group id"
}


variable "tags" {
  type        = map
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}