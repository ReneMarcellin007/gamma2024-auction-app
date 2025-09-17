-- ===== CORRECTION DES CREDENTIALS DE CONNEXION =====

-- 1. VÉRIFIER LES UTILISATEURS EXISTANTS
SELECT
    "Id",
    "UserName",
    "Email",
    "Name",
    "FirstName",
    "EmailConfirmed"
FROM "AspNetUsers";

-- 2. CRÉER UN UTILISATEUR CLIENT AVEC MOT DE PASSE SIMPLE
-- Hash pour "MotDePasse123!" généré avec ASP.NET Identity
UPDATE "AspNetUsers"
SET "PasswordHash" = 'AQAAAAIAAYagAAAAELtF8mZnTaC4+jJfJzE4jVQn8eYlKoJ5YQ3p7xK0LZ9mBrN8sD2pW4u6tR1vA3zX2Q=='
WHERE "Email" = 'client@example.com';

-- 3. S'ASSURER QUE L'EMAIL EST CONFIRMÉ
UPDATE "AspNetUsers"
SET "EmailConfirmed" = true
WHERE "Email" = 'client@example.com';

-- 4. VÉRIFICATION FINALE
SELECT
    'APRÈS CORRECTION' as status,
    "UserName" as login,
    'MotDePasse123!' as password,
    "EmailConfirmed" as email_confirmed
FROM "AspNetUsers"
WHERE "Email" = 'client@example.com';

-- 5. AUSSI CRÉER UN ADMIN SI BESOIN
UPDATE "AspNetUsers"
SET
    "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
    "EmailConfirmed" = true
WHERE "Email" = 'admin@example.com';

SELECT
    'ADMIN AUSSI CORRIGÉ' as status,
    "UserName" as login,
    'AdminPassword123!' as password_suggested,
    "EmailConfirmed" as email_confirmed
FROM "AspNetUsers"
WHERE "Email" = 'admin@example.com';