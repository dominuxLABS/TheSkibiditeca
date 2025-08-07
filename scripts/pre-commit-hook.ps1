# TheSkibiditeca Pre-commit Hook (Windows PowerShell)
# This script runs before each commit to enforce code quality

Write-Host "🔍 Running pre-commit checks..." -ForegroundColor Cyan

# Check if we're in the right directory
if (-not (Test-Path "TheSkibiditeca.sln")) {
    Write-Host "❌ Not in project root directory!" -ForegroundColor Red
    exit 1
}

# Build the project
Write-Host "🔨 Building project..." -ForegroundColor Yellow
$buildResult = dotnet build --configuration Debug --no-restore --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed! Fix compilation errors before committing." -ForegroundColor Red
    exit 1
}

# Check naming conventions
Write-Host "📋 Checking naming conventions..." -ForegroundColor Yellow

# Check Controllers
$controllers = Get-ChildItem -Path "." -Include "*Controller.cs" -Recurse | Where-Object { $_.Directory.Name -eq "Controllers" }
foreach ($controller in $controllers) {
    if (-not $controller.BaseName.EndsWith("Controller")) {
        Write-Host "❌ NAMING ERROR: $($controller.FullName) should end with 'Controller'" -ForegroundColor Red
        exit 1
    }
}

# Check for files in wrong folders
$modelsInControllers = Get-ChildItem -Path "." -Include "*.cs" -Recurse | Where-Object { 
    $_.Directory.Name -eq "Controllers" -and -not $_.BaseName.EndsWith("Controller") 
}
if ($modelsInControllers) {
    Write-Host "❌ STRUCTURE ERROR: Non-controller files found in Controllers folder!" -ForegroundColor Red
    $modelsInControllers | ForEach-Object { Write-Host "  - $($_.FullName)" -ForegroundColor Red }
    exit 1
}

# Check staged files for common mistakes
$stagedFiles = git diff --cached --name-only --diff-filter=ACM | Where-Object { $_ -like "*.cs" }
foreach ($file in $stagedFiles) {
    $content = Get-Content $file -Raw
    
    # Check for TODO/FIXME in staged code
    if ($content -match "TODO|FIXME|HACK") {
        Write-Host "⚠️  WARNING: $file contains TODO/FIXME/HACK comments" -ForegroundColor Yellow
    }
    
    # Check for Console.WriteLine in non-Program.cs files
    if ($file -notlike "*Program.cs" -and $content -match "Console\.WriteLine") {
        Write-Host "❌ ERROR: $file contains Console.WriteLine - use proper logging instead!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "✅ All pre-commit checks passed!" -ForegroundColor Green
Write-Host "🚀 Proceeding with commit..." -ForegroundColor Cyan
