#!/bin/bash
# Universal Setup Script - Auto-detects OS and runs appropriate setup

echo "🔍 Detecting operating system..."

if [[ "$OSTYPE" == "msys" ]] || [[ "$OSTYPE" == "cygwin" ]] || [[ "$OS" == "Windows_NT" ]]; then
    echo "🪟 Windows detected - running PowerShell setup..."
    if command -v powershell &> /dev/null; then
        powershell -ExecutionPolicy Bypass -File "scripts/setup-dev-environment.ps1"
    elif command -v pwsh &> /dev/null; then
        pwsh -File "scripts/setup-dev-environment.ps1"
    else
        echo "❌ PowerShell not found! Please install PowerShell or run setup manually."
        exit 1
    fi
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
    echo "🐧 Linux detected - running Bash setup..."
    bash scripts/setup-dev-environment.sh
elif [[ "$OSTYPE" == "darwin"* ]]; then
    echo "🍎 macOS detected - running Bash setup..."
    bash scripts/setup-dev-environment.sh
else
    echo "❓ Unknown OS: $OSTYPE"
    echo "Please run the appropriate setup script manually:"
    echo "  Windows: scripts/setup-dev-environment.ps1"
    echo "  Linux/macOS: scripts/setup-dev-environment.sh"
    exit 1
fi
