# Script para aplicar labels al repositorio TheSkibiditeca
# Requiere GitHub CLI instalado y autenticado

Write-Host "Aplicando labels estandarizados al repositorio..." -ForegroundColor Green

# Lista de labels con sus configuraciones
$labels = @(
    # Tipos de trabajo
    @{name="feature"; color="0075ca"; description="Nueva funcionalidad o característica"},
    @{name="bug"; color="d73a49"; description="Error o comportamiento inesperado"},
    @{name="hotfix"; color="b60205"; description="Corrección urgente para producción"},
    @{name="enhancement"; color="a2eeef"; description="Mejora de funcionalidad existente"},
    @{name="refactor"; color="5319e7"; description="Reestructuración de código sin cambiar funcionalidad"},
    @{name="documentation"; color="0052cc"; description="Documentación y comentarios"},
    @{name="test"; color="1d76db"; description="Pruebas unitarias, integración o E2E"},
    
    # Áreas del proyecto
    @{name="frontend"; color="fbca04"; description="Interfaz de usuario, vistas, CSS, JavaScript"},
    @{name="backend"; color="d4c5f9"; description="Lógica de negocio, APIs, controladores"},
    @{name="database"; color="c2e0c6"; description="Modelo de datos, migraciones, consultas"},
    @{name="infrastructure"; color="f9d0c4"; description="Docker, deployment, configuración de servidor"},
    @{name="security"; color="e4e669"; description="Autenticación, autorización, seguridad"},
    
    # Funcionalidades específicas
    @{name="books"; color="7057ff"; description="Gestión de libros y catálogo"},
    @{name="users"; color="008672"; description="Gestión de usuarios y perfiles"},
    @{name="loans"; color="e99695"; description="Sistema de préstamos y reservas"},
    @{name="returns"; color="f7e068"; description="Devoluciones de libros"},
    @{name="fines"; color="d93f0b"; description="Gestión de multas y pagos"},
    @{name="reports"; color="0e8a16"; description="Reportes y estadísticas"},
    @{name="search"; color="006b75"; description="Búsqueda y filtrado"},
    
    # Prioridades
    @{name="priority-high"; color="b60205"; description="Alta prioridad - resolver ASAP"},
    @{name="priority-medium"; color="fbca04"; description="Prioridad media"},
    @{name="priority-low"; color="0e8a16"; description="Baja prioridad"},
    
    # Estado y flujo
    @{name="draft"; color="ededed"; description="Borrador o trabajo en progreso"},
    @{name="ready"; color="c5def5"; description="Listo para comenzar desarrollo"},
    @{name="in-progress"; color="fef2c0"; description="En desarrollo activo"},
    @{name="review-needed"; color="f9d0c4"; description="Necesita revisión de código"},
    @{name="blocked"; color="d93f0b"; description="Bloqueado por dependencias externas"},
    
    # Diseño y UX
    @{name="design"; color="f7c6c7"; description="Diseño visual y UX"},
    @{name="figma"; color="ff6b6b"; description="Prototipos y mockups en Figma"},
    @{name="ui-components"; color="fad8b5"; description="Componentes de interfaz reutilizables"},
    
    # Calidad
    @{name="performance"; color="5319e7"; description="Optimización de rendimiento"},
    @{name="accessibility"; color="0052cc"; description="Accesibilidad web (a11y)"},
    @{name="responsive"; color="1d76db"; description="Diseño responsive y mobile-friendly"},
    
    # Colaboración
    @{name="help-wanted"; color="008672"; description="Se necesita ayuda externa"},
    @{name="good-first-issue"; color="7057ff"; description="Buena para nuevos colaboradores"},
    @{name="question"; color="cc317c"; description="Pregunta o discusión necesaria"},
    
    # Milestone y releases
    @{name="v1.0"; color="0075ca"; description="Para la versión 1.0"},
    @{name="mvp"; color="d4c5f9"; description="Producto mínimo viable"},
    @{name="phase-1"; color="bfdadc"; description="Primera fase del proyecto"},
    @{name="phase-2"; color="c2e0c6"; description="Segunda fase del proyecto"}
)

# Aplicar cada label
foreach ($label in $labels) {
    try {
        Write-Host "Creando/actualizando label: $($label.name)" -ForegroundColor Yellow
        gh label create $label.name --color $label.color --description $label.description --repo dominuxLABS/TheSkibiditeca --force
        Write-Host "✓ Label '$($label.name)' creado/actualizado exitosamente" -ForegroundColor Green
    }
    catch {
        Write-Host "⚠ Error creando label '$($label.name)': $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`nProceso completado. Revisa los labels en: https://github.com/dominuxLABS/TheSkibiditeca/labels" -ForegroundColor Cyan
