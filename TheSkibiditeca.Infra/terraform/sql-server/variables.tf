# Copyright (c) dominuxLABS. All rights reserved.

# =============================================================================
# PROXMOX CONFIGURATION VARIABLES
# =============================================================================

variable "proxmox_api_url" {
  description = "URL de la API de Proxmox VE (ej: https://192.168.1.100:8006/api2/json)"
  type        = string
  validation {
    condition     = can(regex("^https?://", var.proxmox_api_url))
    error_message = "La URL debe comenzar con http:// o https://"
  }
}

variable "proxmox_user" {
  description = "Usuario de Proxmox (ej: root@pam, terraform@pve)"
  type        = string
  default     = "root@pam"
}

variable "proxmox_password" {
  description = "Contraseña del usuario de Proxmox"
  type        = string
  sensitive   = true
  validation {
    condition     = length(var.proxmox_password) >= 6
    error_message = "La contraseña debe tener al menos 6 caracteres."
  }
}

variable "proxmox_tls_insecure" {
  description = "Omitir verificación de certificados TLS (para entornos de desarrollo)"
  type        = bool
  default     = true
}

# =============================================================================
# VIRTUAL MACHINE CONFIGURATION
# =============================================================================

variable "vm_name" {
  description = "Nombre de la VM de SQL Server"
  type        = string
  default     = "sql-server-theskibiditeca"
  validation {
    condition     = can(regex("^[a-zA-Z0-9-]+$", var.vm_name))
    error_message = "El nombre de VM solo puede contener letras, números y guiones."
  }
}

variable "vm_target_node" {
  description = "Nodo de Proxmox donde desplegar la VM"
  type        = string
  validation {
    condition     = length(var.vm_target_node) > 0
    error_message = "Debe especificar el nodo de destino."
  }
}

variable "vm_template" {
  description = "Template base para clonar (debe ser Ubuntu 20.04+ o Debian 11+)"
  type        = string
  default     = "ubuntu-20.04-server-template"
}

variable "vm_cores" {
  description = "Número de núcleos de CPU"
  type        = number
  default     = 4
  validation {
    condition     = var.vm_cores >= 2 && var.vm_cores <= 32
    error_message = "Los núcleos deben estar entre 2 y 32."
  }
}

variable "vm_memory" {
  description = "Memoria RAM en MB"
  type        = number
  default     = 4096
  validation {
    condition     = var.vm_memory >= 2048
    error_message = "SQL Server en Linux requiere al menos 2GB de RAM."
  }
}

variable "vm_disk_size" {
  description = "Tamaño del disco en GB"
  type        = string
  default     = "100G"
}

variable "vm_storage" {
  description = "Pool de almacenamiento de Proxmox"
  type        = string
  default     = "local-lvm"
}

# =============================================================================
# NETWORK CONFIGURATION
# =============================================================================

variable "vm_network_bridge" {
  description = "Bridge de red de Proxmox"
  type        = string
  default     = "vmbr0"
}

variable "vm_ip_address" {
  description = "Dirección IP estática para la VM (sin máscara de red)"
  type        = string
  validation {
    condition     = can(regex("^([0-9]{1,3}\\.){3}[0-9]{1,3}$", var.vm_ip_address))
    error_message = "Debe ser una dirección IP válida (ej: 192.168.1.100)."
  }
}

variable "vm_gateway" {
  description = "Gateway de red"
  type        = string
  validation {
    condition     = can(regex("^([0-9]{1,3}\\.){3}[0-9]{1,3}$", var.vm_gateway))
    error_message = "Debe ser una dirección IP válida para el gateway."
  }
}

variable "vm_dns_servers" {
  description = "Servidores DNS"
  type        = list(string)
  default     = ["8.8.8.8", "8.8.4.4"]
}

variable "vm_network_mask" {
  description = "Máscara de red (CIDR)"
  type        = number
  default     = 24
  validation {
    condition     = var.vm_network_mask >= 8 && var.vm_network_mask <= 30
    error_message = "La máscara debe estar entre /8 y /30."
  }
}

# =============================================================================
# SQL SERVER CONFIGURATION
# =============================================================================

variable "sql_server_version" {
  description = "Versión de SQL Server a instalar"
  type        = string
  default     = "2022"
  validation {
    condition     = contains(["2019", "2022"], var.sql_server_version)
    error_message = "Versión soportada: 2019 o 2022."
  }
}

variable "sql_sa_password" {
  description = "Contraseña del usuario SA de SQL Server"
  type        = string
  sensitive   = true
  validation {
    condition = can(regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", var.sql_sa_password))
    error_message = "La contraseña debe tener al menos 8 caracteres, incluyendo mayúsculas, minúsculas, números y símbolos."
  }
}

variable "sql_database_name" {
  description = "Nombre de la base de datos de la aplicación"
  type        = string
  default     = "TheSkibiditeca"
}

# =============================================================================
# CLOUD-INIT CONFIGURATION
# =============================================================================

variable "vm_admin_user" {
  description = "Usuario administrador de Linux"
  type        = string
  default     = "ubuntu"
}

variable "vm_admin_password" {
  description = "Contraseña del administrador (por defecto usa la del SA)"
  type        = string
  default     = ""
  sensitive   = true
}

variable "ssh_public_key" {
  description = "Clave SSH pública para acceso sin contraseña (opcional)"
  type        = string
  default     = ""
}
