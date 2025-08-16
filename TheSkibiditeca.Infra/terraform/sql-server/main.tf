# Copyright (c) dominuxLABS. All rights reserved.

# =============================================================================
# SQL SERVER VIRTUAL MACHINE
# =============================================================================

# Recurso principal: VM de SQL Server en Proxmox
# Este recurso define la máquina virtual que alojará SQL Server
resource "proxmox_vm_qemu" "sql_server" {
  # Identificación y ubicación
  name        = var.vm_name
  target_node = var.vm_target_node
  desc        = "SQL Server ${var.sql_server_version} en Linux para TheSkibiditeca - Desplegado con Terraform"
  
  # Clonación desde template
  # El template debe ser Ubuntu 20.04+ o Debian 11+
  clone = var.vm_template
  
  # ==========================================================================
  # CONFIGURACIÓN DE HARDWARE
  # ==========================================================================
  
  # CPU Configuration
  cores   = var.vm_cores
  sockets = 1
  cpu     = "host"  # Usa las características del CPU del host
  
  # Memoria RAM
  memory = var.vm_memory
  
  # Configuración de arranque
  boot    = "order=scsi0;net0"
  scsihw  = "virtio-scsi-pci"  # Controlador SCSI más eficiente
  
  # ==========================================================================
  # CONFIGURACIÓN DE ALMACENAMIENTO
  # ==========================================================================
  
  # Disco principal del sistema
  disk {
    slot     = 0
    type     = "scsi"
    storage  = var.vm_storage
    size     = var.vm_disk_size
    cache    = "writethrough"  # Balance entre rendimiento y seguridad
    ssd      = 1              # Indica que es SSD para optimizaciones
    iothread = 1              # Mejora el rendimiento de I/O
  }
  
  # ==========================================================================
  # CONFIGURACIÓN DE RED
  # ==========================================================================
  
  # Interfaz de red principal
  network {
    model    = "virtio"           # Driver de red más eficiente
    bridge   = var.vm_network_bridge
    firewall = false              # Deshabilitado para simplicidad
  }
  
  # Configuración IP estática
  ipconfig0 = "ip=${var.vm_ip_address}/${var.vm_network_mask},gw=${var.vm_gateway}"
  
  # Servidores DNS
  nameserver = join(" ", var.vm_dns_servers)
  
  # ==========================================================================
  # CONFIGURACIÓN DE CLOUD-INIT
  # ==========================================================================
  
  # Usuario y contraseña inicial
  ciuser     = var.vm_admin_user
  cipassword = var.vm_admin_password != "" ? var.vm_admin_password : var.sql_sa_password
  
  # Clave SSH pública (opcional, para acceso sin contraseña)
  sshkeys = var.ssh_public_key != "" ? var.ssh_public_key : null
  
  # Configuración del sistema operativo
  agent    = 1          # Habilita QEMU Guest Agent
  os_type  = "cloud-init"  # Tipo de OS para cloud-init
  
  # ==========================================================================
  # CONFIGURACIONES AVANZADAS
  # ==========================================================================
  
  # Configuración de arranque
  bios = "seabios"  # BIOS tradicional para Linux
  
  # ==========================================================================
  # LIFECYCLE MANAGEMENT
  # ==========================================================================
  
  # Ignora cambios en ciertos atributos para evitar recreación innecesaria
  lifecycle {
    ignore_changes = [
      network,      # Los cambios de red no deberían recrear la VM
      cipassword,   # Los cambios de contraseña no requieren recreación
    ]
  }
  
  # Espera a que la VM esté completamente iniciada
  # Esto es importante para el provisioning posterior
  provisioner "remote-exec" {
    inline = [
      "echo 'VM inicializada correctamente'",
      "sudo systemctl status qemu-guest-agent",
      "uptime"
    ]
    
    connection {
      type     = "ssh"
      user     = var.vm_admin_user
      password = var.vm_admin_password != "" ? var.vm_admin_password : var.sql_sa_password
      host     = var.vm_ip_address
      port     = 22        # Puerto SSH
      timeout  = "10m"     # Timeout para conexión
    }
  }
  
  # Tags para organización y facturación
  tags = "terraform,sql-server,linux,theskibiditeca,${var.sql_server_version}"
}

# =============================================================================
# INSTALACIÓN Y CONFIGURACIÓN DE SQL SERVER
# =============================================================================

# Recurso para copiar scripts de instalación
resource "null_resource" "copy_installation_scripts" {
  depends_on = [proxmox_vm_qemu.sql_server]
  
  # Copia el script de instalación principal
  provisioner "file" {
    source      = "${path.module}/scripts/install-sql-server.sh"
    destination = "/tmp/install-sql-server.sh"
    
    connection {
      type     = "ssh"
      user     = var.vm_admin_user
      password = var.vm_admin_password != "" ? var.vm_admin_password : var.sql_sa_password
      host     = var.vm_ip_address
      port     = 22
      timeout  = "5m"
    }
  }
  
  # Copia script de configuración de firewall
  provisioner "file" {
    source      = "${path.module}/scripts/configure-firewall.sh"
    destination = "/tmp/configure-firewall.sh"
    
    connection {
      type     = "ssh"
      user     = var.vm_admin_user
      password = var.vm_admin_password != "" ? var.vm_admin_password : var.sql_sa_password
      host     = var.vm_ip_address
      port     = 22
      timeout  = "5m"
    }
  }
  
  # Trigger para recrear cuando cambien los scripts
  triggers = {
    script_hash = filemd5("${path.module}/scripts/install-sql-server.sh")
  }
}

# Instalación de SQL Server
resource "null_resource" "sql_server_installation" {
  depends_on = [null_resource.copy_installation_scripts]
  
  # Ejecuta la instalación de SQL Server
  provisioner "remote-exec" {
    inline = [
      "chmod +x /tmp/install-sql-server.sh",
      "sudo /tmp/install-sql-server.sh '${var.sql_sa_password}' '${var.sql_server_version}' '${var.sql_database_name}'"
    ]
    
    connection {
      type     = "ssh"
      user     = var.vm_admin_user
      password = var.vm_admin_password != "" ? var.vm_admin_password : var.sql_sa_password
      host     = var.vm_ip_address
      port     = 22
      timeout  = "30m"  # La instalación puede tomar mucho tiempo
    }
  }
  
  # Configura el firewall
  provisioner "remote-exec" {
    inline = [
      "chmod +x /tmp/configure-firewall.sh",
      "sudo /tmp/configure-firewall.sh"
    ]
    
    connection {
      type     = "ssh"
      user     = var.vm_admin_user
      password = var.vm_admin_password != "" ? var.vm_admin_password : var.sql_sa_password
      host     = var.vm_ip_address
      port     = 22
      timeout  = "5m"
    }
  }
  
  # Trigger para reinstalar si cambia la configuración
  triggers = {
    sa_password_hash = sha256(var.sql_sa_password)
    sql_version     = var.sql_server_version
    database_name   = var.sql_database_name
  }
}
