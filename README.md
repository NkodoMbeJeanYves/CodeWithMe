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

## 5. Voici le contenu de la documentation que tu peux **copier directement** dans ton fichier Markdown (`README.md` ou autre).  

```markdown
# Documentation: dotnet-aspnet-codegenerator

Le `dotnet-aspnet-codegenerator` est un outil de scaffolding pour ASP.NET Core.  
Il permet de générer rapidement des contrôleurs, vues, Razor Pages et composants Identity.

---

## 🔹 Installation
```bash
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
```

---

### 🔹 Commandes utiles

### 1. Générer un contrôleur API
```bash
dotnet-aspnet-codegenerator controller -name WeatherForecastController -m WeatherForecast -dc AppDbContext -outDir Controllers -api
```
- `-name` → Nom du contrôleur  
- `-m` → Classe modèle  
- `-dc` → DbContext  
- `-outDir` → Dossier de sortie  
- `-api` → Génère un contrôleur API (sans vues)

---

### 2. Générer un contrôleur MVC avec vues
```bash
dotnet-aspnet-codegenerator controller -name ProductController -m Product -dc AppDbContext -outDir Controllers -f
```
- `-f` → Génère les vues Razor associées

---

### 3. Générer des Razor Pages
```bash
dotnet-aspnet-codegenerator razorpage -m Product -dc AppDbContext -outDir Pages/Products -udl
```
- `-udl` → Utilise le layout par défaut

---

### 4. Générer l’UI Identity
```bash
dotnet-aspnet-codegenerator identity -dc AppDbContext
```
Scaffold des pages d’authentification (login, register, etc.).

---

### 5. Générer une vue Razor
```bash
dotnet-aspnet-codegenerator view Index Empty -outDir Views/Home
```
- `Index` → Nom de la vue  
- `Empty` → Type de template

---

#### 🔹 Options utiles
- `-m` → Classe modèle  
- `-dc` → DbContext  
- `-outDir` → Dossier de sortie  
- `-api` → Contrôleur API  
- `-f` → Générer les vues  
- `-udl` → Utiliser le layout par défaut  

---

#### 🔹 Notes
- Toujours ajouter le package `Microsoft.VisualStudio.Web.CodeGeneration.Design` au projet.  
- Lancer les commandes depuis la racine du projet (où se trouve le `.csproj`).  
- Utiliser `dotnet-aspnet-codegenerator --help` pour voir toutes les options disponibles.  

---

#### 🔹 Exemple workflow complet
1. Ajouter le package NuGet :
   ```bash
   dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
   ```
2. Installer l’outil global :
   ```bash
   dotnet tool install -g dotnet-aspnet-codegenerator
   ```
3. Générer un contrôleur CRUD :
   ```bash
   dotnet-aspnet-codegenerator controller -name WeatherForecastController -m WeatherForecast -dc AppDbContext -outDir Controllers -api
   ```
```

