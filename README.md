# 🏛️ Gamma2024 - Plateforme d'Enchères en Ligne

Une application complète de ventes aux enchères développée avec **.NET 8** et **Vue.js**, permettant aux utilisateurs de participer à des enchères en temps réel sur des œuvres d'art et objets de collection.

## 🎯 Fonctionnalités Principales

### 👥 Pour les Utilisateurs
- **Inscription/Connexion** sécurisée avec JWT
- **Navigation des encans** présents, futurs et passés  
- **Mise en temps réel** avec notifications instantanées via SignalR
- **Surenchères automatiques** programmables
- **Historique des mises** et des achats
- **Gestion des cartes de crédit** via Stripe
- **Facturation et livraison** automatisées

### 🔨 Soirées de Clôture
- **Décompte en temps réel** pour chaque lot
- **Mises automatiques** et **surenchères manuelles**
- **Attribution automatique** du lot au plus offrant
- **Génération de factures** instantanée
- **Options de livraison** configurables

### 🛠️ Panneau Administrateur
- **Gestion des encans** (création, modification, publication)
- **Ajout et modification des lots** avec photos
- **Gestion des vendeurs** et commissions
- **Supervision des ventes** et statistiques
- **Gestion des factures** et livraisons
- **Suivi des paiements** Stripe

## 🏗️ Architecture Technique

### Backend (.NET 8)
- **ASP.NET Core** avec Entity Framework Core
- **Base de données**: SQLite (dev) / PostgreSQL (prod)
- **Authentication**: ASP.NET Identity avec JWT
- **Temps réel**: SignalR pour les mises en direct
- **Paiements**: Intégration Stripe complète
- **Emails**: Service de notification automatique

### Frontend (Vue.js 3)
- **Vue 3** avec Composition API
- **Vuex** pour la gestion d'état
- **Vue Router** pour la navigation
- **Bootstrap 5** pour le design responsive
- **SignalR Client** pour les mises en temps réel
- **DataTables** pour l'affichage des données

### 🚀 Déploiement
- **Railway** ready avec configuration automatique
- **Docker** support avec Dockerfile optimisé
- **Auto-migrations** en production
- **Variables d'environnement** sécurisées

## 📋 Prérequis

- **.NET 8** SDK
- **Node.js** 18+ et npm
- **PostgreSQL** (production) ou SQLite (développement)
- **Compte Stripe** pour les paiements

## 🚀 Installation et Démarrage

### 1. Cloner le repository
```bash
git clone https://github.com/votre-username/Gamma2024.git
cd Gamma2024
```

### 2. Configuration Backend
```bash
cd Gamma2024.Server
dotnet restore
dotnet ef database update
```

### 3. Configuration Frontend  
```bash
cd gamma2024.client
npm install
```

### 4. Variables d'environnement
Créer `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=gamma2024.db"
  },
  "Jwt": {
    "Secret": "votre-clé-jwt-très-sécurisée-minimum-32-caractères",
    "Issuer": "Gamma2024",
    "Audience": "Gamma2024Users"
  },
  "Stripe": {
    "PublishableKey": "pk_test_...",
    "SecretKey": "sk_test_..."
  }
}
```

### 5. Initialiser la base de données avec des données de test
```bash
cd Gamma.Seeder
dotnet run
```

### 6. Démarrer l'application
```bash
# Terminal 1 - Backend
cd Gamma2024.Server
dotnet run

# Terminal 2 - Frontend  
cd gamma2024.client
npm run dev
```

L'application sera disponible sur:
- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5122

## 👤 Comptes de Test

### Administrateur
- **Email**: admin@example.com
- **Mot de passe**: MotDePasseAdmin123!

### Client Test
- **Email**: client@example.com  
- **Mot de passe**: MotDePasseClient123!

## 🎨 Captures d'Écran

*[Ajoutez ici des captures d'écran de votre application]*

## 🛣️ Roadmap

- [ ] Application mobile React Native
- [ ] Intégration PayPal
- [ ] Système d'évaluation des vendeurs
- [ ] Chat en temps réel
- [ ] Notifications push
- [ ] API publique pour développeurs

## 🤝 Contribution

1. **Fork** le projet
2. Créer une **branch feature** (`git checkout -b feature/nouvelle-fonctionnalité`)
3. **Commit** vos changements (`git commit -m 'Ajout nouvelle fonctionnalité'`)
4. **Push** vers la branch (`git push origin feature/nouvelle-fonctionnalité`)
5. Ouvrir une **Pull Request**

## 📄 Licence

Ce projet est sous licence **MIT**. Voir le fichier [LICENSE](LICENSE) pour plus de détails.

## 👨‍💻 Auteur

**René Tchokomi**
- GitHub: [@votre-username](https://github.com/votre-username)
- Email: rene.tchokomi@gmail.com

---

© Les Encans de Nantes au Québec {{ new Date().getFullYear() }} par René Tchokomi

*Développé avec ❤️ en .NET 8 et Vue.js*