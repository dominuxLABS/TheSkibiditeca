#!/bin/bash
# Copyright (c) dominuxLABS. All rights reserved.

# =============================================================================
# SCRIPT DE INSTALACIÓN DE SQL SERVER EN LINUX
# =============================================================================
# Este script automatiza la instalación de SQL Server en Ubuntu/Debian
# incluyendo configuración de red, firewall y creación de base de datos.
#
# Uso: ./install-sql-server.sh <sa_password> <sql_version> <database_name>
#
# Parámetros:
#   sa_password   - Contraseña para el usuario SA de SQL Server
#   sql_version   - Versión de SQL Server (2019 o 2022)
#   database_name - Nombre de la base de datos de la aplicación
# =============================================================================

set -euo pipefail  # Salir en caso de error

# Variables globales
SA_PASSWORD="${1:-}"
SQL_VERSION="${2:-2022}"
DATABASE_NAME="${3:-TheSkibiditeca}"
LOG_FILE="/var/log/sql-server-installation.log"

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

validate_parameters() {
    log "Validando parámetros de entrada..."
    
    if [[ -z "$SA_PASSWORD" ]]; then
        log_error "La contraseña SA es requerida"
        echo "Uso: $0 <sa_password> [sql_version] [database_name]"
        exit 1
    fi
    
    # Validar contraseña SA (requisitos de SQL Server)
    if [[ ${#SA_PASSWORD} -lt 8 ]]; then
        log_error "La contraseña SA debe tener al menos 8 caracteres"
        exit 1
    fi
    
    if [[ ! "$SA_PASSWORD" =~ [A-Z] ]] || [[ ! "$SA_PASSWORD" =~ [a-z] ]] || \
       [[ ! "$SA_PASSWORD" =~ [0-9] ]] || [[ ! "$SA_PASSWORD" =~ [^A-Za-z0-9] ]]; then
        log_error "La contraseña SA debe contener mayúsculas, minúsculas, números y símbolos"
        exit 1
    fi
    
    # Validar versión de SQL Server
    if [[ "$SQL_VERSION" != "2019" && "$SQL_VERSION" != "2022" ]]; then
        log_error "Versión de SQL Server no soportada: $SQL_VERSION (use 2019 o 2022)"
        exit 1
    fi
    
    log_success "Parámetros validados correctamente"
}

check_prerequisites() {
    log "Verificando prerequisitos del sistema..."
    
    # Verificar distribución
    if [[ -f /etc/os-release ]]; then
        . /etc/os-release
        log "Distribución detectada: $NAME $VERSION"
        
        case "$ID" in
            ubuntu)
                if [[ $(echo "$VERSION_ID >= 18.04" | bc -l) -eq 0 ]]; then
                    log_error "Se requiere Ubuntu 18.04 o superior"
                    exit 1
                fi
                ;;
            debian)
                if [[ $(echo "$VERSION_ID >= 10" | bc -l) -eq 0 ]]; then
                    log_error "Se requiere Debian 10 o superior"
                    exit 1
                fi
                ;;
            *)
                log_warning "Distribución no oficialmente soportada: $ID"
                ;;
        esac
    else
        log_error "No se pudo determinar la distribución del sistema"
        exit 1
    fi
    
    # Verificar arquitectura
    ARCH=$(uname -m)
    if [[ "$ARCH" != "x86_64" ]]; then
        log_error "SQL Server solo soporta arquitectura x86_64, detectada: $ARCH"
        exit 1
    fi
    
    # Verificar memoria RAM (mínimo 2GB)
    MEMORY_KB=$(grep MemTotal /proc/meminfo | awk '{print $2}')
    MEMORY_GB=$((MEMORY_KB / 1024 / 1024))
    log "Memoria RAM detectada: ${MEMORY_GB}GB"
    
    if [[ $MEMORY_GB -lt 2 ]]; then
        log_error "SQL Server requiere al menos 2GB de RAM"
        exit 1
    fi
    
    # Verificar espacio en disco (mínimo 6GB libres)
    DISK_SPACE_GB=$(df / | tail -1 | awk '{print int($4/1024/1024)}')
    log "Espacio libre en /: ${DISK_SPACE_GB}GB"
    
    if [[ $DISK_SPACE_GB -lt 6 ]]; then
        log_error "Se requieren al menos 6GB libres en el sistema de archivos raíz"
        exit 1
    fi
    
    log_success "Prerequisitos verificados correctamente"
}

update_system() {
    log "Actualizando sistema..."
    
    # Actualizar índice de paquetes
    apt-get update -qq
    
    # Instalar dependencias básicas
    apt-get install -y -qq \
        curl \
        wget \
        gnupg2 \
        software-properties-common \
        apt-transport-https \
        ca-certificates \
        lsb-release \
        bc
    
    log_success "Sistema actualizado"
}

install_sql_server() {
    log "Iniciando instalación de SQL Server $SQL_VERSION..."
    
    # Descargar e importar la clave pública del repositorio
    log "Configurando repositorio de Microsoft..."
    curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
    
    # Configurar repositorio según la distribución
    . /etc/os-release
    case "$ID" in
        ubuntu)
            echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/$VERSION_ID/mssql-server-$SQL_VERSION main" > /etc/apt/sources.list.d/mssql-server-$SQL_VERSION.list
            ;;
        debian)
            echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/debian/$VERSION_ID/mssql-server-$SQL_VERSION main" > /etc/apt/sources.list.d/mssql-server-$SQL_VERSION.list
            ;;
    esac
    
    # Actualizar índice con el nuevo repositorio
    apt-get update -qq
    
    # Instalar SQL Server
    log "Instalando paquete mssql-server..."
    export DEBIAN_FRONTEND=noninteractive
    apt-get install -y -qq mssql-server
    
    log_success "SQL Server instalado"
}

configure_sql_server() {
    log "Configurando SQL Server..."
    
    # Configurar SQL Server usando mssql-conf
    log "Ejecutando configuración inicial..."
    
    # Configurar edición (Developer es gratuita)
    /opt/mssql/bin/mssql-conf set editions.defaultedition Developer
    
    # Configurar contraseña SA y aceptar EULA
    export MSSQL_SA_PASSWORD="$SA_PASSWORD"
    export ACCEPT_EULA=Y
    
    # Ejecutar configuración
    /opt/mssql/bin/mssql-conf setup accept-eula
    
    # Configurar opciones adicionales
    log "Configurando opciones de red..."
    
    # Habilitar TCP/IP en puerto 1433 (ya viene habilitado por defecto)
    /opt/mssql/bin/mssql-conf set network.tcpport 1433
    
    # Configurar memoria máxima (dejar 25% para el sistema)
    MEMORY_KB=$(grep MemTotal /proc/meminfo | awk '{print $2}')
    MAX_MEMORY_MB=$((MEMORY_KB * 75 / 100 / 1024))
    
    if [[ $MAX_MEMORY_MB -gt 2048 ]]; then
        log "Configurando memoria máxima: ${MAX_MEMORY_MB}MB"
        /opt/mssql/bin/mssql-conf set memory.memorylimitmb $MAX_MEMORY_MB
    fi
    
    # Habilitar servicios de texto completo si están disponibles
    /opt/mssql/bin/mssql-conf set sqlagent.enabled true 2>/dev/null || true
    
    log_success "SQL Server configurado"
}

start_sql_server() {
    log "Iniciando servicios de SQL Server..."
    
    # Reiniciar servicio para aplicar configuración
    systemctl restart mssql-server
    
    # Habilitar arranque automático
    systemctl enable mssql-server
    
    # Esperar a que SQL Server esté disponible
    log "Esperando que SQL Server esté disponible..."
    local max_attempts=30
    local attempt=1
    
    while [[ $attempt -le $max_attempts ]]; do
        if systemctl is-active --quiet mssql-server; then
            log_success "SQL Server está ejecutándose"
            break
        fi
        
        log "Intento $attempt/$max_attempts - Esperando..."
        sleep 10
        ((attempt++))
    done
    
    if [[ $attempt -gt $max_attempts ]]; then
        log_error "Timeout esperando que SQL Server esté disponible"
        systemctl status mssql-server
        exit 1
    fi
    
    # Verificar que podemos conectarnos
    local test_attempts=10
    local test_attempt=1
    
    while [[ $test_attempt -le $test_attempts ]]; do
        if /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT @@VERSION" >/dev/null 2>&1; then
            log_success "Conexión a SQL Server verificada"
            break
        fi
        
        log "Intento de conexión $test_attempt/$test_attempts..."
        sleep 5
        ((test_attempt++))
    done
    
    if [[ $test_attempt -gt $test_attempts ]]; then
        log_error "No se pudo establecer conexión con SQL Server"
        exit 1
    fi
}

install_sql_tools() {
    log "Instalando herramientas de SQL Server..."
    
    # Configurar repositorio para herramientas
    . /etc/os-release
    case "$ID" in
        ubuntu)
            echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/$VERSION_ID/prod main" > /etc/apt/sources.list.d/msprod.list
            ;;
        debian)
            echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/debian/$VERSION_ID/prod main" > /etc/apt/sources.list.d/msprod.list
            ;;
    esac
    
    # Actualizar e instalar herramientas
    apt-get update -qq
    
    # Instalar sqlcmd y bcp
    export DEBIAN_FRONTEND=noninteractive
    echo 'mssql-tools18 mssql-tools18/accept-eula boolean true' | debconf-set-selections
    apt-get install -y -qq mssql-tools18
    
    # Agregar herramientas al PATH
    echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> /etc/environment
    export PATH="$PATH:/opt/mssql-tools18/bin"
    
    # Crear symlinks para compatibilidad
    ln -sf /opt/mssql-tools18/bin/sqlcmd /usr/local/bin/sqlcmd
    ln -sf /opt/mssql-tools18/bin/bcp /usr/local/bin/bcp
    
    log_success "Herramientas de SQL Server instaladas"
}

create_database() {
    log "Creando base de datos $DATABASE_NAME..."
    
    # Script SQL para crear la base de datos
    local sql_script="/tmp/create_database.sql"
    cat > "$sql_script" << EOF
-- Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '$DATABASE_NAME')
BEGIN
    CREATE DATABASE [$DATABASE_NAME]
    COLLATE SQL_Latin1_General_CP1_CI_AS
END
GO

-- Usar la base de datos
USE [$DATABASE_NAME]
GO

-- Configurar opciones de la base de datos
ALTER DATABASE [$DATABASE_NAME] SET RECOVERY FULL
GO
ALTER DATABASE [$DATABASE_NAME] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [$DATABASE_NAME] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [$DATABASE_NAME] SET AUTO_UPDATE_STATISTICS ON
GO

-- Mostrar información de la base de datos
SELECT 
    name as DatabaseName,
    collation_name as Collation,
    compatibility_level as CompatibilityLevel,
    state_desc as State
FROM sys.databases 
WHERE name = '$DATABASE_NAME'
GO
EOF
    
    # Ejecutar script
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i "$sql_script" -C
    
    if [[ $? -eq 0 ]]; then
        log_success "Base de datos '$DATABASE_NAME' creada exitosamente"
    else
        log_error "Error creando base de datos '$DATABASE_NAME'"
        exit 1
    fi
    
    # Limpiar archivo temporal
    rm -f "$sql_script"
    
    # Crear directorio de backups
    mkdir -p /var/opt/mssql/backup
    chown mssql:mssql /var/opt/mssql/backup
    chmod 755 /var/opt/mssql/backup
    
    # Crear backup inicial
    log "Creando backup inicial..."
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "BACKUP DATABASE [$DATABASE_NAME] TO DISK = '/var/opt/mssql/backup/${DATABASE_NAME}_initial.bak'"
    
    log_success "Backup inicial creado en /var/opt/mssql/backup/${DATABASE_NAME}_initial.bak"
}

configure_firewall() {
    log "Configurando firewall..."
    
    # Verificar si ufw está instalado
    if command -v ufw >/dev/null 2>&1; then
        log "Configurando reglas UFW..."
        
        # Permitir SSH (importante para no perder acceso)
        ufw allow ssh
        
        # Permitir SQL Server
        ufw allow 1433/tcp comment "SQL Server"
        
        # Habilitar firewall si no está activo
        if ! ufw status | grep -q "Status: active"; then
            echo "y" | ufw enable
        fi
        
        log_success "Firewall UFW configurado"
    elif command -v iptables >/dev/null 2>&1; then
        log "Configurando reglas iptables..."
        
        # Permitir SQL Server
        iptables -A INPUT -p tcp --dport 1433 -j ACCEPT
        
        # Guardar reglas (método varía según distribución)
        if command -v iptables-save >/dev/null 2>&1; then
            iptables-save > /etc/iptables/rules.v4 2>/dev/null || true
        fi
        
        log_success "Firewall iptables configurado"
    else
        log_warning "No se encontró firewall configurado"
    fi
}

test_installation() {
    log "Verificando instalación..."
    
    # Verificar servicio
    if ! systemctl is-active --quiet mssql-server; then
        log_error "El servicio mssql-server no está ejecutándose"
        exit 1
    fi
    
    # Verificar conexión y versión
    local version_info
    version_info=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT @@VERSION as SqlVersion" -h -1 -W)
    
    if [[ $? -eq 0 ]]; then
        log_success "Conexión exitosa a SQL Server"
        log "Versión: $(echo "$version_info" | head -1)"
    else
        log_error "Error conectando a SQL Server"
        exit 1
    fi
    
    # Verificar base de datos
    local db_check
    db_check=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT name FROM sys.databases WHERE name = '$DATABASE_NAME'" -h -1 -W)
    
    if [[ "$db_check" == "$DATABASE_NAME" ]]; then
        log_success "Base de datos '$DATABASE_NAME' verificada"
    else
        log_error "Base de datos '$DATABASE_NAME' no encontrada"
        exit 1
    fi
    
    # Verificar puerto
    if netstat -tln | grep -q ":1433 "; then
        log_success "SQL Server escuchando en puerto 1433"
    else
        log_error "SQL Server no está escuchando en puerto 1433"
        exit 1
    fi
    
    log_success "Verificación de instalación completada"
}

show_summary() {
    log_success "=== INSTALACIÓN COMPLETADA EXITOSAMENTE ==="
    echo
    echo -e "${GREEN}📊 RESUMEN DE LA INSTALACIÓN${NC}"
    echo "======================================"
    echo "🗄️  SQL Server: $SQL_VERSION"
    echo "💾 Base de datos: $DATABASE_NAME"
    echo "👤 Usuario SA: sa"
    echo "🌐 Puerto: 1433"
    echo "🏠 Host: $(hostname -I | awk '{print $1}')"
    echo "📁 Backups: /var/opt/mssql/backup/"
    echo "📋 Logs: /var/opt/mssql/log/"
    echo "⚙️  Configuración: /var/opt/mssql/"
    echo
    echo -e "${BLUE}🔗 INFORMACIÓN DE CONEXIÓN${NC}"
    echo "=============================="
    echo "Servidor: $(hostname -I | awk '{print $1}'),1433"
    echo "Base de datos: $DATABASE_NAME"
    echo "Usuario: sa"
    echo "Contraseña: [CONFIGURADA]"
    echo
    echo -e "${YELLOW}📝 CADENA DE CONEXIÓN${NC}"
    echo "======================="
    echo "Server=$(hostname -I | awk '{print $1}'),1433;Database=$DATABASE_NAME;User Id=sa;Password=***;TrustServerCertificate=true;Encrypt=false;"
    echo
    echo -e "${BLUE}🛠️  COMANDOS ÚTILES${NC}"
    echo "=================="
    echo "• Estado del servicio: sudo systemctl status mssql-server"
    echo "• Reiniciar servicio: sudo systemctl restart mssql-server"
    echo "• Conectar localmente: sqlcmd -S localhost -U sa"
    echo "• Ver logs: sudo tail -f /var/opt/mssql/log/errorlog"
    echo "• Configuración: sudo /opt/mssql/bin/mssql-conf"
    echo
    echo -e "${GREEN}✅ SQL Server está listo para usar!${NC}"
}

# =============================================================================
# SCRIPT PRINCIPAL
# =============================================================================

main() {
    log "=== INICIANDO INSTALACIÓN DE SQL SERVER EN LINUX ==="
    log "Versión: $SQL_VERSION"
    log "Base de datos: $DATABASE_NAME"
    log "Fecha: $(date)"
    log "Host: $(hostname)"
    log "Usuario: $(whoami)"
    
    # Crear directorio de logs
    mkdir -p "$(dirname "$LOG_FILE")"
    touch "$LOG_FILE"
    
    # Verificaciones iniciales
    check_root
    validate_parameters
    check_prerequisites
    
    # Instalación
    update_system
    install_sql_server
    configure_sql_server
    start_sql_server
    install_sql_tools
    create_database
    configure_firewall
    
    # Verificación final
    test_installation
    show_summary
    
    log "=== INSTALACIÓN COMPLETADA ==="
    log "Log completo en: $LOG_FILE"
}

# Ejecutar función principal
main "$@"
