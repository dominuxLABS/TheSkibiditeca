# Development Environment Setup Script (Windows PowerShell)
# Run this once to set up code quality tools

Write-Host "🚀 Setting up TheSkibiditeca development environment..." -ForegroundColor Cyan

# Check if we're in the right directory
if (-not (Test-Path "TheSkibiditeca.sln")) {
    Write-Host "❌ Please run this script from the project root directory!" -ForegroundColor Red
    exit 1
}

# Install pre-commit hook
Write-Host "📋 Installing pre-commit hook..." -ForegroundColor Yellow
if (-not (Test-Path ".git/hooks")) {
    New-Item -ItemType Directory -Path ".git/hooks" -Force | Out-Null
}

Copy-Item "scripts\pre-commit-hook.ps1" ".git\hooks\pre-commit" -Force
Write-Host "✅ Pre-commit hook installed!" -ForegroundColor Green

# Restore NuGet packages
Write-Host "📦 Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Packages restored!" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to restore packages!" -ForegroundColor Red
    exit 1
}

# Test build
Write-Host "🔨 Testing build..." -ForegroundColor Yellow
dotnet build --configuration Debug
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build successful!" -ForegroundColor Green
} else {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

# Check Git configuration
Write-Host "🔧 Checking Git configuration..." -ForegroundColor Yellow
$gitName = git config user.name
$gitEmail = git config user.email

if (-not $gitName -or -not $gitEmail) {
    Write-Host "⚠️  Git user not configured. Please run:" -ForegroundColor Yellow
    Write-Host "git config --global user.name 'Your Name'" -ForegroundColor Cyan
    Write-Host "git config --global user.email 'your.email@example.com'" -ForegroundColor Cyan
} else {
    Write-Host "✅ Git configured for: $gitName <$gitEmail>" -ForegroundColor Green
}

Write-Host ""
Write-Host "🎉 Development environment setup complete!" -ForegroundColor Green
Write-Host ""
Write-Host "📚 Next steps:" -ForegroundColor Cyan
Write-Host "1. Read DEVELOPMENT.md for coding guidelines" -ForegroundColor White
Write-Host "2. Try making a commit to test the pre-commit hook" -ForegroundColor White
Write-Host "3. Follow the naming conventions in README.md" -ForegroundColor White
Write-Host ""
Write-Host "Happy coding! 🚀" -ForegroundColor Green
