# Development Setup Instructions

## Code Quality Enforcement

This project uses automated tools to enforce coding standards and naming conventions. Follow these steps to set up your development environment:

### 1. Install Git Hooks (REQUIRED for all team members)

Choose the setup method based on your operating system:

#### **Option A: Universal Setup (Recommended)**
```bash
# Auto-detects your OS and runs the appropriate script
bash scripts/setup.sh
```

#### **Option B: Platform-Specific Setup**

**Windows (PowerShell):**
```powershell
# Run in PowerShell (recommended)
.\scripts\setup-dev-environment.ps1

# Or use Command Prompt
scripts\setup.bat
```

**Linux/macOS (Bash):**
```bash
# Make script executable first
chmod +x scripts/setup-dev-environment.sh

# Run setup
./scripts/setup-dev-environment.sh
```

#### **Option C: Manual Installation**

If automatic setup fails, manually install the pre-commit hook:

**Windows:**
```powershell
# Copy pre-commit hook
Copy-Item "scripts\pre-commit-hook.ps1" ".git\hooks\pre-commit" -Force
```

**Linux/macOS:**
```bash
# Copy and make executable
cp scripts/pre-commit-hook.sh .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit
```

### 2. IDE Configuration

#### Visual Studio 2022
1. Go to **Tools > Options > Text Editor > C# > Code Style**
2. Import the `.editorconfig` file (should be automatic)
3. Enable **Format document on save**
4. Enable **Remove unnecessary usings on save**

#### Visual Studio Code
1. Install the **C# extension**
2. Install the **EditorConfig extension**
3. The `.editorconfig` will be applied automatically

### 3. What Gets Checked

The automated system enforces:

- ✅ **Controller naming**: Must end with `Controller.cs`
- ✅ **File organization**: Controllers in `/Controllers/`, Models in `/Models/`, etc.
- ✅ **Code style**: Consistent indentation, formatting, using statements
- ✅ **Commit messages**: Must follow conventional format (`feat:`, `fix:`, etc.)
- ✅ **No debug code**: Prevents `Console.WriteLine` in production code
- ✅ **Build success**: Code must compile before commit

### 4. If You Break the Rules

The system will:
1. **Block your commit** with a clear error message
2. **Fail the GitHub Actions** on pull requests
3. **Show exactly what to fix**

### 5. Emergency Override (USE SPARINGLY!)

If you absolutely need to bypass checks:
```bash
git commit --no-verify -m "emergency: your message"
```

**⚠️ WARNING**: Overrides will be visible in commit history and may be questioned during code review!

### 6. Getting Help

If the automated checks are confusing:
1. Read the error message carefully
2. Check the naming conventions in the main README
3. Ask a team member
4. Create an issue in the repository

## Available Setup Scripts

The project includes multiple setup scripts for different platforms:

- **`scripts/setup.sh`** - Universal script (auto-detects OS)
- **`scripts/setup-dev-environment.ps1`** - Windows PowerShell
- **`scripts/setup-dev-environment.sh`** - Linux/macOS Bash  
- **`scripts/setup.bat`** - Windows Command Prompt (fallback)
- **`scripts/pre-commit-hook.ps1`** - Windows pre-commit hook
- **`scripts/pre-commit-hook.sh`** - Linux/macOS pre-commit hook

## Why We Do This

- 🎯 **Consistency**: Everyone's code looks the same
- 🐛 **Fewer bugs**: Catches common mistakes early
- 📚 **Learning**: Forces good practices
- 🚀 **Professional**: Industry-standard workflow
- 🤝 **Teamwork**: Easier to work with each other's code

---

**Remember**: The tools are here to help, not to annoy you! They catch mistakes before they become bigger problems.
