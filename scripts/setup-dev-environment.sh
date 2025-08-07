#!/bin/bash
# Development Environment Setup Script (Linux/macOS)
# Run this once to set up code quality tools

echo "🚀 Setting up TheSkibiditeca development environment..."

# Check if we're in the right directory
if [ ! -f "TheSkibiditeca.sln" ]; then
    echo "❌ Please run this script from the project root directory!"
    exit 1
fi

# Install pre-commit hook
echo "📋 Installing pre-commit hook..."
mkdir -p .git/hooks

# Copy the appropriate hook based on the platform
if command -v pwsh &> /dev/null; then
    echo "PowerShell Core detected, using PowerShell hook..."
    cp scripts/pre-commit-hook.ps1 .git/hooks/pre-commit
else
    echo "Using Bash hook..."
    cp scripts/pre-commit-hook.sh .git/hooks/pre-commit
fi

chmod +x .git/hooks/pre-commit
echo "✅ Pre-commit hook installed!"

# Check for .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found! Please install .NET 8.0 SDK first."
    echo "Visit: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

# Restore NuGet packages
echo "📦 Restoring NuGet packages..."
if dotnet restore; then
    echo "✅ Packages restored!"
else
    echo "❌ Failed to restore packages!"
    exit 1
fi

# Test build
echo "🔨 Testing build..."
if dotnet build --configuration Debug; then
    echo "✅ Build successful!"
else
    echo "❌ Build failed!"
    exit 1
fi

# Check Git configuration
echo "🔧 Checking Git configuration..."
GIT_NAME=$(git config user.name)
GIT_EMAIL=$(git config user.email)

if [ -z "$GIT_NAME" ] || [ -z "$GIT_EMAIL" ]; then
    echo "⚠️  Git user not configured. Please run:"
    echo "git config --global user.name 'Your Name'"
    echo "git config --global user.email 'your.email@example.com'"
else
    echo "✅ Git configured for: $GIT_NAME <$GIT_EMAIL>"
fi

echo ""
echo "🎉 Development environment setup complete!"
echo ""
echo "📚 Next steps:"
echo "1. Read DEVELOPMENT.md for coding guidelines"
echo "2. Try making a commit to test the pre-commit hook"
echo "3. Follow the naming conventions in README.md"
echo ""
echo "Happy coding! 🚀"
