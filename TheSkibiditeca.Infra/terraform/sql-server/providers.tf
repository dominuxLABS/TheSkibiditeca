# Copyright (c) dominuxLABS. All rights reserved.

# Terraform configuration block
# Define la versión mínima de Terraform y los proveedores requeridos
terraform {
  required_version = ">= 1.0"
  
  required_providers {
    # Proveedor para Proxmox VE
    # Permite gestionar VMs, contenedores, almacenamiento, etc.
    proxmox = {
      source  = "telmate/proxmox"
      version = "~> 2.9"
    }
    
    # Proveedor null para ejecutar scripts y comandos
    # Útil para provisioning personalizado
    null = {
      source  = "hashicorp/null"
      version = "~> 3.1"
    }
  }
}

# Configuración del proveedor Proxmox
# Se conecta a la API de Proxmox VE para gestionar recursos
provider "proxmox" {
  pm_api_url      = var.proxmox_api_url
  pm_user         = var.proxmox_user
  pm_password     = var.proxmox_password
  pm_tls_insecure = var.proxmox_tls_insecure
  
  # Configuración opcional para debugging
  pm_log_enable = true
  pm_log_file   = "terraform-plugin-proxmox.log"
  pm_debug      = true
  pm_log_levels = {
    _default    = "debug"
    _capturelog = ""
  }
}
