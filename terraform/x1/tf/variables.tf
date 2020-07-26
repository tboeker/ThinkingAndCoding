variable "location" {
  type        = string
  default     = "westeurope"
  description = "Specify a location see: az account list-locations -o table"
}

variable "rgname" {
  type        = string
  default     = "terraform-group"
  description = "the resource group"
}

variable "acrname" {
  type        = string
  default     = "tbacr"
  description = "the azure container registry name"
}


variable "tags" {
  type        = map
  description = "A list of tags associated to all resources"

  default = {
    maintained_by = "terraform"
  }
}