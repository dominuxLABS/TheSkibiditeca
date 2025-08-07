# Guía de Uso de Labels - TheSkibiditeca

## Estructura de Labels

### 🔧 Tipos de Trabajo
- `feature` - Nueva funcionalidad
- `bug` - Error o comportamiento inesperado  
- `hotfix` - Corrección urgente
- `enhancement` - Mejora de funcionalidad existente
- `refactor` - Reestructuración de código
- `documentation` - Documentación
- `test` - Pruebas

### 🏗️ Áreas del Proyecto
- `frontend` - UI, vistas, CSS, JavaScript
- `backend` - Lógica de negocio, APIs
- `database` - Modelo de datos, migraciones
- `infrastructure` - Docker, deployment
- `security` - Autenticación, autorización

### 📚 Funcionalidades Específicas
- `books` - Gestión de libros y catálogo
- `users` - Gestión de usuarios
- `loans` - Sistema de préstamos
- `returns` - Devoluciones
- `fines` - Gestión de multas
- `reports` - Reportes y estadísticas
- `search` - Búsqueda y filtrado

### ⚡ Prioridades
- `priority-high` - Alta prioridad (rojo)
- `priority-medium` - Prioridad media (amarillo)
- `priority-low` - Baja prioridad (verde)

### 📊 Estado y Flujo
- `draft` - Borrador o WIP
- `ready` - Listo para desarrollo
- `in-progress` - En desarrollo activo
- `review-needed` - Necesita code review
- `blocked` - Bloqueado por dependencias

### 🎨 Diseño y UX
- `design` - Diseño visual y UX
- `figma` - Prototipos en Figma
- `ui-components` - Componentes reutilizables

### 🚀 Calidad
- `performance` - Optimización
- `accessibility` - Accesibilidad (a11y)
- `responsive` - Diseño responsive

### 🤝 Colaboración
- `help-wanted` - Se necesita ayuda
- `good-first-issue` - Para nuevos colaboradores
- `question` - Pregunta o discusión

### 🎯 Milestone
- `v1.0` - Para versión 1.0
- `mvp` - Producto mínimo viable
- `phase-1` - Primera fase
- `phase-2` - Segunda fase

## Combinaciones Recomendadas

### Ejemplos de uso:
- `feature` + `frontend` + `books` + `priority-medium`
- `bug` + `backend` + `loans` + `priority-high`
- `enhancement` + `search` + `ui-components` + `ready`
- `documentation` + `phase-1` + `good-first-issue`

## Aplicación Manual de Labels

Para aplicar estos labels al repositorio, puedes:

1. **Vía GitHub CLI:**
```bash
# Instalar gh CLI y autenticarse
gh label create "feature" --color "0075ca" --description "Nueva funcionalidad o característica"
# Repetir para cada label...
```

2. **Vía interfaz web:**
- Ir a Settings > Labels en GitHub
- Crear manualmente cada label

3. **Herramientas automáticas:**
- Usar GitHub Actions con `crazy-max/ghaction-github-labeler`
- Usar herramientas como `github-label-sync`

## Script PowerShell para Aplicar Labels

```powershell
# Requiere GitHub CLI instalado y autenticado
$labels = @(
    @{name="feature"; color="0075ca"; description="Nueva funcionalidad o característica"},
    @{name="bug"; color="d73a49"; description="Error o comportamiento inesperado"},
    # ... resto de labels
)

foreach ($label in $labels) {
    gh label create $label.name --color $label.color --description $label.description
}
```
