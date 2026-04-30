# ✅ Code Review – Projet ASP.NET Core (Version Améliorée)

---

## 📌 Informations générales

| Élément            | Valeur                                 |
| ------------------ | -------------------------------------- |
| Projet             |                                        |
| Repository         |                                        |
| Branche / Tag      |                                        |
| Commit / PR        |                                        |
| Auteur             |                                        |
| Reviewer           |                                        |
| Date               |                                        |
| Type de changement | ☐ Feature ☐ Bugfix ☐ Refactor ☐ Hotfix |

---

## 🎯 Objectif de la revue

* [ ] Respect des standards (.NET / équipe)
* [ ] Absence de bugs évidents
* [ ] Sécurité des données et endpoints
* [ ] Performance acceptable
* [ ] Code testable et maintenable
* [ ] Impact maîtrisé (non régression)

---

## 🚨 Synthèse rapide (à remplir en dernier)

| Critère         | Statut                       |
| --------------- | ---------------------------- |
| Qualité globale | ☐ OK ☐ Moyen ☐ Problématique |
| Risque          | ☐ Faible ☐ Moyen ☐ Élevé     |
| Prêt pour merge | ☐ Oui ☐ Non                  |

**Points bloquants :**

* …

**Recommandations principales :**

* …

---

## 🧩 Format obligatoire des remarques

Chaque problème DOIT suivre ce format :

**🔴 Problème :**
Description claire du problème

**⚠️ Impact :**
(Bug / Sécurité / Performance / Maintenabilité)

**💡 Suggestion :**
Explication de l’amélioration

**🛠 Exemple (avant) :**

```csharp
// code actuel
```

**✅ Exemple (après) :**

```csharp
// code corrigé
```

---

## 🏗️ Architecture & Structure

### ✅ À vérifier

* [ ] Architecture cohérente (Clean / Onion / autre explicitée)
* [ ] Couche Domain indépendante
* [ ] Pas de logique métier dans Controllers
* [ ] Services correctement découpés
* [ ] Respect du SRP
* [ ] Pas de dépendances circulaires

### 🔍 Red flags + corrections attendues

* [ ] Classe trop grosse → proposer découpage
* [ ] Méthode longue → proposer extraction
* [ ] Trop de dépendances → proposer interface/service

---

## ⚙️ Configuration & Environnement

### 🔍 Améliorations attendues

* Externaliser toute config sensible
* Utiliser `IOptions<T>` correctement
* Centraliser la config

---

## 🌐 API & Controllers

### 🔍 Corrections attendues

* Remplacer `try/catch` par middleware global
* Ajouter validation via FluentValidation
* Uniformiser les réponses HTTP

---

## 🧠 Logique métier

### 🔍 Corrections attendues

* Extraire logique métier hors controllers
* Supprimer duplication
* Clarifier règles métier

---

## 🗄️ Accès aux données

### 🔍 Corrections attendues

* Optimiser requêtes EF Core
* Supprimer N+1
* Ajouter pagination

---

## 🔐 Sécurité

### 🔍 Corrections attendues

* Ajouter validation des inputs
* Masquer données sensibles
* Implémenter policies d’autorisation

---

## ⚡ Performance

### 🔍 Corrections attendues

* Corriger mauvais usage async/await
* Supprimer `.Result` / `.Wait()`
* Ajouter cache si nécessaire

---

## 🧪 Tests

### 🔍 Corrections attendues

* Ajouter tests manquants
* Améliorer assertions
* Mock dépendances

---

## 🧹 Qualité du code

### 🔍 Corrections attendues

* Renommer variables
* Supprimer code mort
* Factoriser duplication

---

## 📌 Commentaires détaillés (OBLIGATOIRE)

| Fichier | Ligne | Gravité                            | Type                      | Commentaire | Suggestion |
| ------- | ----- | ---------------------------------- | ------------------------- | ----------- | ---------- |
|         |       | 🔴 Critique / 🟠 Moyen / 🟢 Faible | Bug / Perf / Secu / Clean |             |            |

---

## 🎯 Plan d’actions recommandé

### 🔴 Priorité haute

* …

### 🟠 Priorité moyenne

* …

### 🟢 Améliorations

* …

---

## ✅ Conclusion

* [ ] ✅ Approuvé
* [ ] 🔁 Modifications requises
* [ ] ❌ Refusé
