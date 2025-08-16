# Copyright (c) dominuxLABS. All rights reserved.

# =============================================================================
# TERRAFORM OUTPUTS
# =============================================================================
# Los outputs permiten obtener información sobre los recursos creados
# Útiles para integración con otros sistemas o para debugging

# =============================================================================
# INFORMACIÓN DE LA MÁQUINA VIRTUAL
# =============================================================================

output "vm_id" {
  description = "ID único de la VM en Proxmox"
  value       = proxmox_vm_qemu.sql_server.vmid
}

output "vm_name" {
  description = "Nombre de la VM de SQL Server"
  value       = proxmox_vm_qemu.sql_server.name
}

output "vm_node" {
  description = "Nodo de Proxmox donde está desplegada la VM"
  value       = proxmox_vm_qemu.sql_server.target_node
}

output "vm_status" {
  description = "Estado actual de la VM"
  value       = "deployed"  # Indica que la VM fue desplegada exitosamente
}

# =============================================================================
# INFORMACIÓN DE RED
# =============================================================================

output "vm_ip_address" {
  description = "Dirección IP de la VM de SQL Server"
  value       = var.vm_ip_address
}

output "vm_gateway" {
  description = "Gateway de red configurado"
  value       = var.vm_gateway
}

output "vm_dns_servers" {
  description = "Servidores DNS configurados"
  value       = var.vm_dns_servers
}

# =============================================================================
# INFORMACIÓN DE SQL SERVER
# =============================================================================

output "sql_server_version" {
  description = "Versión de SQL Server instalada"
  value       = var.sql_server_version
}

output "sql_server_instance" {
  description = "Nombre de la instancia de SQL Server"
  value       = "MSSQLSERVER"  # Instancia por defecto
}

output "sql_server_port" {
  description = "Puerto TCP de SQL Server"
  value       = 1433
}

output "sql_database_name" {
  description = "Nombre de la base de datos de la aplicación"
  value       = var.sql_database_name
}

# =============================================================================
# CADENAS DE CONEXIÓN
# =============================================================================

# Cadena de conexión con autenticación SQL
output "sql_connection_string_sql_auth" {
  description = "Cadena de conexión usando autenticación SQL (SA)"
  value       = "Server=${var.vm_ip_address},1433;Database=${var.sql_database_name};User Id=sa;Password=${var.sql_sa_password};TrustServerCertificate=true;Encrypt=false;"
  sensitive   = true  # Marca como sensible porque contiene credenciales
}

# Cadena de conexión para Entity Framework
output "sql_connection_string_ef" {
  description = "Cadena de conexión para Entity Framework"
  value       = "Server=${var.vm_ip_address},1433;Database=${var.sql_database_name};User Id=sa;Password=${var.sql_sa_password};TrustServerCertificate=true;Encrypt=false;MultipleActiveResultSets=true;"
  sensitive   = true
}

# Cadena de conexión con autenticación integrada (para uso interno)
output "sql_connection_string_integrated" {
  description = "Cadena de conexión con autenticación integrada de Windows"
  value       = "Server=${var.vm_ip_address},1433;Database=${var.sql_database_name};Integrated Security=true;TrustServerCertificate=true;Encrypt=false;"
  sensitive   = false  # No contiene credenciales explícitas
}

# =============================================================================
# INFORMACIÓN PARA APLICACIÓN
# =============================================================================

# Configuración para appsettings.json
output "appsettings_connection_string" {
  description = "Configuración para appsettings.json de la aplicación"
  value = jsonencode({
    ConnectionStrings = {
      DefaultConnection = "Server=${var.vm_ip_address},1433;Database=${var.sql_database_name};User Id=sa;Password=${var.sql_sa_password};TrustServerCertificate=true;Encrypt=false;MultipleActiveResultSets=true;"
    }
  })
  sensitive = true
}

# =============================================================================
# INFORMACIÓN DE ACCESO
# =============================================================================

output "admin_access_info" {
  description = "Información de acceso administrativo"
  value = {
    ssh_address       = "${var.vm_ip_address}:22"
    admin_user        = var.vm_admin_user
    sql_sa_user       = "sa"
    vnc_access        = "${var.vm_ip_address}:5900"
  }
  sensitive = false
}

# =============================================================================
# COMANDOS ÚTILES
# =============================================================================

output "useful_commands" {
  description = "Comandos útiles para gestionar la infraestructura"
  value = {
    connect_ssh         = "ssh ${var.vm_admin_user}@${var.vm_ip_address}"
    test_sql_connection = "sqlcmd -S ${var.vm_ip_address} -U sa -P [PASSWORD]"
    ssh_tunnel          = "ssh -L 1433:${var.vm_ip_address}:1433 ${var.vm_admin_user}@${var.vm_ip_address}"
    backup_database     = "sqlcmd -S ${var.vm_ip_address} -U sa -Q \"BACKUP DATABASE [${var.sql_database_name}] TO DISK = '/var/opt/mssql/backup/${var.sql_database_name}.bak'\""
    check_sql_status    = "ssh ${var.vm_admin_user}@${var.vm_ip_address} 'sudo systemctl status mssql-server'"
  }
}

# =============================================================================
# MÉTRICAS Y MONITOREO
# =============================================================================

output "resource_allocation" {
  description = "Asignación de recursos de la VM"
  value = {
    cpu_cores = var.vm_cores
    memory_mb = var.vm_memory
    disk_size = var.vm_disk_size
    storage_pool = var.vm_storage
  }
}

# =============================================================================
# INFORMACIÓN DE DEPLOYMENT
# =============================================================================

output "deployment_info" {
  description = "Información del despliegue"
  value = {
    terraform_version = "~> 1.0"
    proxmox_provider = "telmate/proxmox ~> 2.9"
    deployment_time  = timestamp()
    vm_template     = var.vm_template
    proxmox_node    = var.vm_target_node
  }
}
