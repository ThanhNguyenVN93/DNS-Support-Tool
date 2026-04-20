# DNS Flush and Renew Tool

**A Windows utility to flush DNS cache, renew IP address, and change DNS servers with a single click.**

---

## 📋 Overview

| Property | Value |
|----------|-------|
| **Platform** | Windows 10+ (64-bit) |
| **.NET Version** | .NET 8.0 |
| **Framework** | Windows Forms |
| **Language** | C# 12.0 |
| **File Size** | ~1.3 MB (Single EXE) |

---

## 🎯 Features

### 1. **Flush DNS**
   - Execute: `ipconfig /release` → `ipconfig /renew` → `ipconfig /flushdns`
   - After completion, **automatically triggers DNS change dialog**
   - Real-time output display in text box

### 2. **Change DNS**
   - Primary DNS: **1.1.1.1** (Cloudflare)
   - Secondary DNS: **8.8.8.8** (Google)
   - Confirmation dialog before applying changes
   - Exits application after operation (Yes/No)

### 3. **System Information**
   - Real-time clock display in title bar
   - Windows version detection (Windows 7/8/10/11, etc.)

---

## ⚙️ Requirements

| Requirement | Details |
|-------------|---------|
| **OS** | Windows 10 or later (64-bit) |
| **Runtime** | .NET 8 Runtime (included in single EXE) |
| **Permissions** | Administrator privileges required |
| **Network** | Internet connection (not required for basic functions) |

---

## 🚀 Installation & Usage

### **Method 1: Direct Execution**
```powershell
# Run the published EXE
.\publish\DNS Flush and Renew (Release).exe
```

### **Method 2: Build from Source**
```powershell
# Build Release
dotnet build -c Release

# Publish single EXE
dotnet publish -c Release -o .\publish
```

### **Step-by-step Usage:**
1. Run application as Administrator
2. **Option A - Flush DNS:**
   - Click "Flush DNS" button
   - View output in text box
   - Confirm DNS change dialog
3. **Option B - Change DNS Only:**
   - Click "Change DNS" button
   - Confirm dialog (Yes/No)
   - Application closes after operation

---

## 📁 Project Structure

```
frmdnsflush/
├── Form1.cs                          # Main form logic
├── Form1.Designer.cs                 # UI designer
├── Program.cs                        # Entry point (requires admin)
├── app.manifest                      # Administrator elevation manifest
├── frmdnsflush.csproj               # Project configuration
├── frmdnsflush.sln                  # Solution file
├── .gitignore                       # Git ignore rules
├── README.md                        # This file
├── Properties/
│   └── Resources.resx               # UI resources
└── publish/
    └── DNS Flush and Renew (Release).exe  # Final executable
```

---

## 🔧 Technical Details

### **Admin Elevation**
- `app.manifest`: `<requestedExecutionLevel level="requireAdministrator"/>`
- Windows UAC prompt on launch
- No manual admin check in code needed

### **DNS Configuration**
- Uses `netsh interface ip set dns` command
- Automatically detects active network interface
- Supports both wired and wireless connections

### **Command Execution**
- All commands run asynchronously (`async/await`)
- Non-blocking UI during command execution
- Output captured and displayed in real-time

### **Windows Version Detection**
```csharp
// Detects:
- Windows 11 (Build >= 22000)
- Windows 10 (Build < 22000)
- Windows 8.1, 8, 7, Vista
- Displays as: "Windows 11", not full version string
```

---

## 🔄 Build & Publish

### **Debug Build:**
```powershell
dotnet build
# Output: bin\Debug\net8.0-windows\
```

### **Release Build (Single EXE):**
```powershell
dotnet publish -c Release -o .\publish
# Output: publish\DNS Flush and Renew (Release).exe (~1.3 MB)
```

### **Project Settings (frmdnsflush.csproj)**
```xml
<PublishSingleFile>true</PublishSingleFile>
<SelfContained>false</SelfContained>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
<PublishReadyToRun>true</PublishReadyToRun>
```

- **PublishSingleFile**: Creates single EXE (no DLL files)
- **SelfContained=false**: Requires .NET 8 Runtime (but only ~1.3 MB)
- **RuntimeIdentifier**: Targets 64-bit Windows only
- **PublishReadyToRun**: Pre-compiled for faster startup

---

## 📝 Git Repository

### **Files to Commit:**
```
✅ Form1.cs
✅ Form1.Designer.cs
✅ Program.cs
✅ app.manifest
✅ frmdnsflush.csproj
✅ frmdnsflush.sln
✅ .gitignore
✅ README.md
✅ Properties/
```

### **Files to Ignore:**
```
❌ bin/
❌ obj/
❌ .vs/
❌ *.user
❌ publish/  (optional - can commit for releases)
```

### **.gitignore Content:**
```
bin/
obj/
.vs/
*.user
*.suo
publish/
*.nupkg
packages/
```

---

## 🔐 Security & Permissions

⚠️ **Administrator Rights Required For:**
- `ipconfig /release` - Release current IP
- `ipconfig /renew` - Renew IP address
- `ipconfig /flushdns` - Flush DNS cache
- `netsh interface ip set dns` - Modify network settings

✅ **How to Run as Admin:**
1. Right-click EXE → "Run as Administrator"
2. Or add to startup with admin privileges
3. Or set app manifest (already configured)

---

## ❌ Troubleshooting

### **Issue: "No active network interface found"**
- **Cause:** Network disconnected
- **Fix:** Check network connection, try again

### **Issue: "Access Denied" errors**
- **Cause:** Not running as Administrator
- **Fix:** Run as Administrator

### **Issue: "Command not found"**
- **Cause:** Windows system commands unavailable
- **Fix:** Unlikely on Windows 10+, check System32 folder exists

### **Issue: DNS not changed**
- **Cause:** Firewall/Network restrictions
- **Fix:** Disable firewall temporarily, or run from administrator account

---

## 📊 Version History

| Version | Release Date | Changes |
|---------|--------------|---------|
| **1.0** | 2024 | Initial release - Flush DNS + Change DNS features |

---

## 🔗 Resources

- **Cloudflare DNS:** https://1.1.1.1/
- **.NET 8 Documentation:** https://learn.microsoft.com/dotnet/core/
- **Windows Forms:** https://learn.microsoft.com/dotnet/desktop/winforms/

---

## 💻 System Information Displayed

- **OS:** Detected Windows version (Windows 11, Windows 10, etc.)
- **Current Time:** Real-time clock in title bar (24-hour format)
- **Network Interface:** Automatically detected active connection

---

## 📌 Notes

- Application **automatically exits** after DNS change operation
- Output is **logged to text box** (not file-saved)
- Supports **multiple network interfaces** (picks first active)
- **No internet connection required** for basic operation
- Code is **fully English** for international use

---

## 👤 Author

**ThanhCode93**

---

## 📄 License

Free to use and modify for personal/educational purposes.

---

**Last Updated:** 2024
**Built with:** .NET 8 | Windows Forms | C# 12.0
