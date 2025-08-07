@echo off
REM Development Environment Setup Script (Windows Batch)
REM Run this once to set up code quality tools

echo 🚀 Setting up TheSkibiditeca development environment...

REM Check if we're in the right directory
if not exist "TheSkibiditeca.sln" (
    echo ❌ Please run this script from the project root directory!
    pause
    exit /b 1
)

REM Check for PowerShell and run PS1 version if available
where pwsh >nul 2>nul
if %ERRORLEVEL% == 0 (
    echo 🔧 PowerShell Core found, using PowerShell setup...
    pwsh -ExecutionPolicy Bypass -File "scripts\setup-dev-environment.ps1"
    goto :end
)

where powershell >nul 2>nul
if %ERRORLEVEL% == 0 (
    echo 🔧 Windows PowerShell found, using PowerShell setup...
    powershell -ExecutionPolicy Bypass -File "scripts\setup-dev-environment.ps1"
    goto :end
)

REM Fallback to batch implementation
echo 📋 Installing pre-commit hook...
if not exist ".git\hooks" mkdir ".git\hooks"
copy "scripts\pre-commit-hook.ps1" ".git\hooks\pre-commit" >nul
echo ✅ Pre-commit hook installed!

REM Check for .NET SDK
dotnet --version >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo ❌ .NET SDK not found! Please install .NET 8.0 SDK first.
    echo Visit: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo 📦 Restoring NuGet packages...
dotnet restore
if %ERRORLEVEL% neq 0 (
    echo ❌ Failed to restore packages!
    pause
    exit /b 1
)
echo ✅ Packages restored!

echo 🔨 Testing build...
dotnet build --configuration Debug
if %ERRORLEVEL% neq 0 (
    echo ❌ Build failed!
    pause
    exit /b 1
)
echo ✅ Build successful!

echo.
echo 🎉 Development environment setup complete!
echo.
echo 📚 Next steps:
echo 1. Read DEVELOPMENT.md for coding guidelines
echo 2. Try making a commit to test the pre-commit hook
echo 3. Follow the naming conventions in README.md
echo.
echo Happy coding! 🚀

:end
pause
