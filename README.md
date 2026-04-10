# CodeWithMe

## 1. Introduction

Here’s your content restructured into a clean **Markdown (.md)** format, ready to drop into documentation or a workflow guide:

```markdown
# CodeWithMe

Check .NET Version
```bash
dotnet --info
dotnet --version
```

---

## 2. Add Entity Framework Core Packages
```bash
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet tool install --global dotnet-ef --version 9.0.0
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0
```

---

## 3. Create Initial Migration
> ⚠️ Make sure you have defined your **DbContext** and entity classes before running this command.

```bash
dotnet ef migrations add InitialCreate --output-dir Core/Migrations
```

---
>  ⚠️ To undo this action

```bash
dotnet ef migrations remove
```

## 4. Apply Migration to Database
```bash
dotnet ef database update
```

---

## ✅ Notes
- `Microsoft.EntityFrameworkCore.Design` → provides design‑time services for migrations.  
- `dotnet-ef` → command‑line tool for managing migrations.  
- `Pomelo.EntityFrameworkCore.MySql` → EF Core provider for MySQL.  
- Always keep package versions aligned with your target **.NET SDK** and **EF Core runtime**.  
```


