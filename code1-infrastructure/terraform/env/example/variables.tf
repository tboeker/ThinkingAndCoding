variable "EVENTSTORE_YODA_PASSWORD" {
  type        = string
  default     = "Password01"
  description = "the password of the yoda admin user"
}

variable "BACKEND_RESOURCE_GROUP_NAME" {
  type  = string  
}

variable "BACKEND_STORAGE_ACCOUNT_NAME" {
  type  = string  
}

variable "BACKEND_STORAGE_CONTAINER_NAME" {
  type  = string  
}