# Informations de connexion pour les tests

## Utilisateur Admin par défaut
- **Email**: admin@encans.com
- **Mot de passe**: Admin123!

## Utilisateur Client (depuis ApplicationDbContext)
- **Email**: client@example.com
- **Mot de passe**: MotDePasseClient123!

## Utilisateur Admin (depuis ApplicationDbContext)
- **Email**: admin@example.com
- **Mot de passe**: MotDePasseAdmin123!

## Corrections déployées

### ✅ Base de données
- Recréation complète de la base au démarrage en production
- Suppression et recréation de toutes les données
- 3 encans créés (passé, en cours, futur)
- 4 lots d'art avec descriptions
- Catégories et médiums créés

### ✅ Authentification
- CORS ouvert temporairement pour toutes les origines
- JWT simplifié (pas de validation issuer/audience)
- Logs ajoutés pour débugger le login

### ✅ Déploiement Railway
- Branche: clean-deploy
- Déploiement automatique activé
- Les données seront recréées à chaque redémarrage

## Si les problèmes persistent

Vérifiez dans les logs Railway:
1. Recherchez "=== DÉBUT DU SEEDER ==="
2. Recherchez "=== DONNÉES CRÉÉES ==="
3. Recherchez "=== LOGIN ATTEMPT ===" pour les tentatives de connexion

Le système FORCE maintenant la recréation complète à chaque démarrage.