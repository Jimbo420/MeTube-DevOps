# Sets global variables for this Terraform project.

variable "subscription_id" {
  description = "The Azure subscription ID"
  type        = string
}

variable "app_name" {
  default = "metubedevops"
}

variable "location" {
  default = "westeurope"
}

variable "kubernetes_version" {
  default = "1.30.6"
}