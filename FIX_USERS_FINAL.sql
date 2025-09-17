-- ===== CORRECTION DÉFINITIVE DES UTILISATEURS =====
-- Ce script REMPLACE FIX_PARTIE_2_USERS.sql

-- 1. VÉRIFIER CE QUI EXISTE
SELECT 'UTILISATEURS ACTUELS:' as info;
SELECT "Id", "Email", "UserName" FROM "AspNetUsers";

-- 2. METTRE À JOUR client@example.com (au lieu d'INSERT)
-- Hash pour "MotDePasseClient123!"
UPDATE "AspNetUsers"
SET
    "UserName" = 'client@example.com',
    "NormalizedUserName" = 'CLIENT@EXAMPLE.COM',
    "Email" = 'client@example.com',
    "NormalizedEmail" = 'CLIENT@EXAMPLE.COM',
    "EmailConfirmed" = true,
    "PasswordHash" = 'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw=='
WHERE
    "Email" = 'client@example.com' OR
    "UserName" = 'client@example.com' OR
    "Id" = '1d8ac862-e54d-4f10-b6f8-638808c02967';

-- 3. METTRE À JOUR admin@example.com
-- Hash CORRIGÉ pour "MotDePasseAdmin123!" (depuis ApplicationDbContext.cs)
UPDATE "AspNetUsers"
SET
    "UserName" = 'admin@example.com',
    "NormalizedUserName" = 'ADMIN@EXAMPLE.COM',
    "Email" = 'admin@example.com',
    "NormalizedEmail" = 'ADMIN@EXAMPLE.COM',
    "EmailConfirmed" = true,
    "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ=='
WHERE
    "Email" = 'admin@example.com' OR
    "UserName" = 'admin@example.com' OR
    "Id" = '87162d4b-fc3b-4d65-8341-2f4a480982d1';

-- 4. SI AUCUN UTILISATEUR N'EXISTE, LES CRÉER
-- Client
INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
    "Name", "FirstName", "Avatar", "StripeCustomer"
)
SELECT
    '1d8ac862-e54d-4f10-b6f8-638808c02967',
    'client@example.com', 'CLIENT@EXAMPLE.COM',
    'client@example.com', 'CLIENT@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
    '860b65a9-156b-473d-b11b-be6f787cf1e4',
    'ff176598-423c-4557-bfc4-48e928f579e9',
    '455-555-5555', false, false, NULL, true, 0,
    'Dupont', 'Jean', '/Avatars/Defaut.png', ''
WHERE NOT EXISTS (
    SELECT 1 FROM "AspNetUsers"
    WHERE "Email" = 'client@example.com' OR "Id" = '1d8ac862-e54d-4f10-b6f8-638808c02967'
);

-- Admin
INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
    "Name", "FirstName", "Avatar", "StripeCustomer"
)
SELECT
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'admin@example.com', 'ADMIN@EXAMPLE.COM',
    'admin@example.com', 'ADMIN@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'f1f3de20-69b9-491d-bc82-b484b44cd47f',
    '466-666-6666', false, false, NULL, true, 0,
    'Admin', 'Super', '/Avatars/Defaut.png', ''
WHERE NOT EXISTS (
    SELECT 1 FROM "AspNetUsers"
    WHERE "Email" = 'admin@example.com' OR "Id" = '87162d4b-fc3b-4d65-8341-2f4a480982d1'
);

-- 5. VÉRIFICATION FINALE
SELECT '======================================' as separator;
SELECT '✅ CONNEXIONS DISPONIBLES:' as titre;
SELECT '======================================' as separator;
SELECT '' as blank;
SELECT '📧 CLIENT:' as type, 'client@example.com' as email, 'MotDePasseClient123!' as password;
SELECT '📧 ADMIN:' as type, 'admin@example.com' as email, 'MotDePasseAdmin123!' as password;
SELECT '' as blank;
SELECT '======================================' as separator;

-- 6. VÉRIFIER QUE LES MOTS DE PASSE SONT BIEN MIS À JOUR
SELECT
    "Email",
    CASE
        WHEN "PasswordHash" = 'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==' THEN '✅ MotDePasseClient123!'
        WHEN "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==' THEN '✅ MotDePasseAdmin123!'
        ELSE '❌ Hash inconnu'
    END as mot_de_passe_status
FROM "AspNetUsers"
WHERE "Email" IN ('client@example.com', 'admin@example.com');