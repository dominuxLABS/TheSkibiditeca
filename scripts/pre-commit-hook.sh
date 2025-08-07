#!/bin/bash
# TheSkibiditeca Pre-commit Hook (Linux/macOS)
# This script runs before each commit to enforce code quality

echo "🔍 Running pre-commit checks..."

# Check if we're in the right directory
if [ ! -f "TheSkibiditeca.sln" ]; then
    echo "❌ Not in project root directory!"
    exit 1
fi

# Build the project
echo "🔨 Building project..."
if ! dotnet build --configuration Debug --no-restore --verbosity quiet; then
    echo "❌ Build failed! Fix compilation errors before committing."
    exit 1
fi

# Check naming conventions
echo "📋 Checking naming conventions..."

# Check Controllers
find . -name "*Controller.cs" -path "*/Controllers/*" | while read -r file; do
    filename=$(basename "$file" .cs)
    if [[ ! "$filename" =~ Controller$ ]]; then
        echo "❌ NAMING ERROR: $file should end with 'Controller'"
        exit 1
    fi
done

# Check for files in wrong folders
if find . -name "*.cs" -path "*/Controllers/*" | grep -v "Controller\.cs$" | grep -q .; then
    echo "❌ STRUCTURE ERROR: Non-controller files found in Controllers folder!"
    find . -name "*.cs" -path "*/Controllers/*" | grep -v "Controller\.cs$" | while read -r file; do
        echo "  - $file"
    done
    exit 1
fi

# Check staged files for common mistakes
git diff --cached --name-only --diff-filter=ACM | grep "\.cs$" | while read -r file; do
    if [ -f "$file" ]; then
        # Check for TODO/FIXME in staged code
        if grep -q "TODO\|FIXME\|HACK" "$file"; then
            echo "⚠️  WARNING: $file contains TODO/FIXME/HACK comments"
        fi
        
        # Check for Console.WriteLine in non-Program.cs files
        if [[ "$file" != *"Program.cs" ]] && grep -q "Console\.WriteLine" "$file"; then
            echo "❌ ERROR: $file contains Console.WriteLine - use proper logging instead!"
            exit 1
        fi
    fi
done

echo "✅ All pre-commit checks passed!"
echo "🚀 Proceeding with commit..."
