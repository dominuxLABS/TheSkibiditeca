#!/bin/bash
# Copyright (c) dominuxLABS. All rights reserved.

# =============================================================================
# SCRIPT DE CONFIGURACIÓN DE FIREWALL PARA SQL SERVER EN LINUX
# =============================================================================
# Este script configura las reglas de firewall necesarias para permitir
# conexiones a SQL Server desde la red en sistemas Linux.
# =============================================================================

set -euo pipefail

# Variables
LOG_FILE="/var/log/sql-server-firewall.log"

# Colores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# =============================================================================
# FUNCIONES AUXILIARES
# =============================================================================

log() {
    local message="$1"
    local timestamp=$(date '+%Y-%m-%d %H:%M:%S')
    echo -e "${BLUE}[$timestamp]${NC} $message"
    echo "[$timestamp] $message" >> "$LOG_FILE"
}

log_success() {
    local message="$1"
    local timestamp=$(date '+%Y-%m-%d %H:%M:%S')
    echo -e "${GREEN}[$timestamp] ✅ $message${NC}"
    echo "[$timestamp] SUCCESS: $message" >> "$LOG_FILE"
}

log_error() {
    local message="$1"
    local timestamp=$(date '+%Y-%m-%d %H:%M:%S')
    echo -e "${RED}[$timestamp] ❌ $message${NC}" >&2
    echo "[$timestamp] ERROR: $message" >> "$LOG_FILE"
}

log_warning() {
    local message="$1"
    local timestamp=$(date '+%Y-%m-%d %H:%M:%S')
    echo -e "${YELLOW}[$timestamp] ⚠️  $message${NC}"
    echo "[$timestamp] WARNING: $message" >> "$LOG_FILE"
}

check_root() {
    if [[ $EUID -ne 0 ]]; then
        log_error "Este script debe ejecutarse como root (use sudo)"
        exit 1
    fi
}

configure_ufw() {
    log "Configurando UFW (Uncomplicated Firewall)..."
    
    # Verificar si UFW está instalado
    if ! command -v ufw >/dev/null 2>&1; then
        log "UFW no está instalado, instalando..."
        apt-get update -qq
        apt-get install -y -qq ufw
    fi
    
    # Configurar políticas por defecto
    ufw --force default deny incoming
    ufw --force default allow outgoing
    
    # Permitir SSH (CRÍTICO - evita perder acceso remoto)
    ufw allow ssh comment "SSH access"
    ufw allow 22/tcp comment "SSH explicit"
    
    # Permitir SQL Server
    ufw allow 1433/tcp comment "SQL Server"
    
    # Permitir ping (ICMP)
    ufw allow from any to any port 22 proto tcp
    
    # Reglas adicionales para SQL Server
    # SQL Server Browser (opcional, para instancias con nombre)
    # ufw allow 1434/udp comment "SQL Server Browser"
    
    # Habilitar UFW si no está activo
    if ! ufw status | grep -q "Status: active"; then
        log "Habilitando UFW..."
        echo "y" | ufw enable
    else
        log "UFW ya está activo"
    fi
    
    # Mostrar estado
    log "Estado actual de UFW:"
    ufw status numbered
    
    log_success "UFW configurado correctamente"
}

configure_iptables() {
    log "Configurando iptables..."
    
    # Verificar si iptables está disponible
    if ! command -v iptables >/dev/null 2>&1; then
        log_error "iptables no está disponible"
        return 1
    fi
    
    # Backup de reglas actuales
    if command -v iptables-save >/dev/null 2>&1; then
        iptables-save > /tmp/iptables-backup-$(date +%Y%m%d-%H%M%S).rules
        log "Backup de iptables creado en /tmp/"
    fi
    
    # Permitir tráfico loopback
    iptables -A INPUT -i lo -j ACCEPT
    iptables -A OUTPUT -o lo -j ACCEPT
    
    # Permitir conexiones establecidas
    iptables -A INPUT -m state --state ESTABLISHED,RELATED -j ACCEPT
    
    # Permitir SSH (CRÍTICO)
    iptables -A INPUT -p tcp --dport 22 -j ACCEPT
    
    # Permitir SQL Server
    iptables -A INPUT -p tcp --dport 1433 -j ACCEPT
    
    # Permitir ping
    iptables -A INPUT -p icmp --icmp-type echo-request -j ACCEPT
    
    # Política por defecto (DROP INPUT, ACCEPT OUTPUT)
    iptables -P INPUT DROP
    iptables -P FORWARD DROP
    iptables -P OUTPUT ACCEPT
    
    # Intentar guardar reglas
    if command -v iptables-save >/dev/null 2>&1; then
        # Ubuntu/Debian
        mkdir -p /etc/iptables
        iptables-save > /etc/iptables/rules.v4 2>/dev/null || true
        
        # CentOS/RHEL/Fedora
        iptables-save > /etc/sysconfig/iptables 2>/dev/null || true
        
        log_success "Reglas iptables guardadas"
    else
        log_warning "No se pudieron guardar las reglas iptables permanentemente"
    fi
    
    # Mostrar reglas actuales
    log "Reglas iptables actuales:"
    iptables -L -n --line-numbers
}

configure_firewalld() {
    log "Configurando firewalld..."
    
    # Verificar si firewalld está disponible
    if ! command -v firewall-cmd >/dev/null 2>&1; then
        log_warning "firewalld no está disponible"
        return 1
    fi
    
    # Verificar si firewalld está ejecutándose
    if ! systemctl is-active --quiet firewalld; then
        log "Iniciando firewalld..."
        systemctl start firewalld
        systemctl enable firewalld
    fi
    
    # Permitir SSH
    firewall-cmd --permanent --add-service=ssh
    
    # Permitir SQL Server
    firewall-cmd --permanent --add-port=1433/tcp
    
    # Recargar configuración
    firewall-cmd --reload
    
    # Mostrar configuración
    log "Configuración actual de firewalld:"
    firewall-cmd --list-all
    
    log_success "firewalld configurado correctamente"
}

install_netstat() {
    log "Instalando herramientas de red..."
    
    # Instalar net-tools para netstat
    if ! command -v netstat >/dev/null 2>&1; then
        apt-get update -qq
        apt-get install -y -qq net-tools
        log_success "net-tools instalado"
    fi
    
    # Instalar ss (alternativa moderna a netstat)
    if ! command -v ss >/dev/null 2>&1; then
        apt-get install -y -qq iproute2
        log_success "iproute2 instalado"
    fi
}

test_firewall_configuration() {
    log "Verificando configuración de firewall..."
    
    # Verificar puertos abiertos
    log "Verificando puertos SQL Server..."
    
    # Verificar puerto 1433
    if netstat -tln 2>/dev/null | grep -q ":1433 " || ss -tln 2>/dev/null | grep -q ":1433 "; then
        log_success "Puerto 1433 está abierto (SQL Server)"
    else
        log_warning "Puerto 1433 no está abierto"
    fi
    
    # Verificar puerto 22 (SSH)
    if netstat -tln 2>/dev/null | grep -q ":22 " || ss -tln 2>/dev/null | grep -q ":22 "; then
        log_success "Puerto 22 está abierto (SSH)"
    else
        log_warning "Puerto 22 no está abierto"
    fi
    
    # Verificar servicios
    log "Verificando servicios..."
    
    # SQL Server
    if systemctl is-active --quiet mssql-server; then
        log_success "Servicio mssql-server está ejecutándose"
    else
        log_warning "Servicio mssql-server no está ejecutándose"
    fi
    
    # SSH
    if systemctl is-active --quiet ssh || systemctl is-active --quiet sshd; then
        log_success "Servicio SSH está ejecutándose"
    else
        log_warning "Servicio SSH no está ejecutándose"
    fi
    
    # Mostrar resumen de puertos
    log "Puertos abiertos en el sistema:"
    if command -v ss >/dev/null 2>&1; then
        ss -tln | grep LISTEN
    elif command -v netstat >/dev/null 2>&1; then
        netstat -tln | grep LISTEN
    fi
}

configure_fail2ban() {
    log "Configurando fail2ban para seguridad adicional..."
    
    # Verificar si fail2ban está disponible
    if ! command -v fail2ban-server >/dev/null 2>&1; then
        log "Instalando fail2ban..."
        apt-get update -qq
        apt-get install -y -qq fail2ban
    fi
    
    # Crear configuración personalizada para SSH
    cat > /etc/fail2ban/jail.local << 'EOF'
[DEFAULT]
bantime = 3600
findtime = 600
maxretry = 5

[sshd]
enabled = true
port = ssh
filter = sshd
logpath = /var/log/auth.log
maxretry = 3
bantime = 3600
EOF
    
    # Iniciar y habilitar fail2ban
    systemctl restart fail2ban
    systemctl enable fail2ban
    
    # Verificar estado
    if systemctl is-active --quiet fail2ban; then
        log_success "fail2ban configurado y ejecutándose"
        
        # Mostrar estado de jails
        fail2ban-client status
    else
        log_warning "Error configurando fail2ban"
    fi
}

show_security_recommendations() {
    log_success "=== RECOMENDACIONES DE SEGURIDAD ==="
    echo
    echo -e "${YELLOW}🔒 CONFIGURACIÓN DE SEGURIDAD COMPLETADA${NC}"
    echo "======================================"
    echo
    echo -e "${GREEN}✅ Configurado:${NC}"
    echo "• Firewall habilitado"
    echo "• Puerto 1433 abierto para SQL Server"
    echo "• Puerto 22 abierto para SSH"
    echo "• fail2ban configurado para SSH"
    echo
    echo -e "${BLUE}🛡️  Recomendaciones adicionales:${NC}"
    echo "================================="
    echo "1. Cambiar puerto SSH por defecto:"
    echo "   sudo nano /etc/ssh/sshd_config"
    echo "   # Cambiar: Port 22 → Port 2222"
    echo
    echo "2. Configurar autenticación por clave SSH:"
    echo "   ssh-keygen -t ed25519"
    echo "   ssh-copy-id user@server"
    echo
    echo "3. Deshabilitar autenticación por contraseña:"
    echo "   sudo nano /etc/ssh/sshd_config"
    echo "   # Cambiar: PasswordAuthentication yes → no"
    echo
    echo "4. Configurar VPN o túnel SSH para SQL Server:"
    echo "   ssh -L 1433:localhost:1433 user@server"
    echo
    echo "5. Configurar certificados SSL para SQL Server:"
    echo "   sudo /opt/mssql/bin/mssql-conf set network.forceencryption 1"
    echo
    echo "6. Configurar backup automático de firewall:"
    echo "   sudo crontab -e"
    echo "   # 0 2 * * * iptables-save > /etc/iptables/rules.backup"
    echo
    echo -e "${RED}⚠️  IMPORTANTE:${NC}"
    echo "==============" 
    echo "• Testee la conectividad antes de desconectarse"
    echo "• Mantenga una sesión SSH abierta como respaldo"
    echo "• Documente todos los cambios realizados"
    echo "• Configure monitoreo de logs de seguridad"
}

# =============================================================================
# SCRIPT PRINCIPAL
# =============================================================================

main() {
    log "=== CONFIGURANDO FIREWALL PARA SQL SERVER ==="
    log "Fecha: $(date)"
    log "Host: $(hostname)"
    log "Usuario: $(whoami)"
    
    # Crear directorio de logs
    mkdir -p "$(dirname "$LOG_FILE")"
    touch "$LOG_FILE"
    
    # Verificaciones iniciales
    check_root
    
    # Instalar herramientas necesarias
    install_netstat
    
    # Detectar y configurar firewall disponible
    if command -v ufw >/dev/null 2>&1; then
        configure_ufw
    elif command -v firewall-cmd >/dev/null 2>&1; then
        configure_firewalld
    elif command -v iptables >/dev/null 2>&1; then
        configure_iptables
    else
        log_error "No se encontró ningún firewall disponible"
        exit 1
    fi
    
    # Configurar fail2ban para seguridad adicional
    configure_fail2ban
    
    # Verificar configuración
    test_firewall_configuration
    
    # Mostrar recomendaciones
    show_security_recommendations
    
    log_success "=== CONFIGURACIÓN DE FIREWALL COMPLETADA ==="
    log "Log completo en: $LOG_FILE"
}

# Ejecutar función principal
main "$@"
