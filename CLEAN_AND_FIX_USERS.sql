-- ===== NETTOYAGE ET CORRECTION COMPLÈTE DES UTILISATEURS =====
-- Exécutez ce script si FIX_USERS_FINAL.sql donne des erreurs

-- 1. SUPPRIMER LES UTILISATEURS EXISTANTS (pour éviter les conflits)
DELETE FROM "AspNetUsers"
WHERE "Email" IN ('client@example.com', 'admin@example.com')
   OR "UserName" IN ('client@example.com', 'admin@example.com')
   OR "Id" IN ('1d8ac862-e54d-4f10-b6f8-638808c02967', '87162d4b-fc3b-4d65-8341-2f4a480982d1');

-- 2. CRÉER client@example.com PROPREMENT
INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
    "Name", "FirstName", "Avatar", "StripeCustomer"
) VALUES (
    '1d8ac862-e54d-4f10-b6f8-638808c02967',
    'client@example.com',
    'CLIENT@EXAMPLE.COM',
    'client@example.com',
    'CLIENT@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==', -- Hash pour MotDePasseClient123!
    '860b65a9-156b-473d-b11b-be6f787cf1e4',
    'ff176598-423c-4557-bfc4-48e928f579e9',
    '455-555-5555',
    false,
    false,
    NULL,
    true,
    0,
    'Dupont',
    'Jean',
    '/Avatars/Defaut.png',
    ''
);

-- 3. CRÉER admin@example.com PROPREMENT
INSERT INTO "AspNetUsers" (
    "Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail",
    "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled",
    "LockoutEnd", "LockoutEnabled", "AccessFailedCount",
    "Name", "FirstName", "Avatar", "StripeCustomer"
) VALUES (
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'admin@example.com',
    'ADMIN@EXAMPLE.COM',
    'admin@example.com',
    'ADMIN@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==', -- Hash pour MotDePasseAdmin123!
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'f1f3de20-69b9-491d-bc82-b484b44cd47f',
    '466-666-6666',
    false,
    false,
    NULL,
    true,
    0,
    'Admin',
    'Super',
    '/Avatars/Defaut.png',
    ''
);

-- 4. VÉRIFICATION
SELECT '==========================================' as info;
SELECT '✅ UTILISATEURS CRÉÉS AVEC SUCCÈS!' as status;
SELECT '==========================================' as info;
SELECT '' as blank;

SELECT 'CLIENT:' as type, 'client@example.com' as email, 'MotDePasseClient123!' as password
UNION ALL
SELECT 'ADMIN:' as type, 'admin@example.com' as email, 'MotDePasseAdmin123!' as password;

SELECT '' as blank;
SELECT '==========================================' as info;

-- Vérification finale
SELECT 'Email' as col1, 'Confirmé' as col2, 'Hash OK?' as col3
UNION ALL
SELECT
    "Email",
    CASE WHEN "EmailConfirmed" THEN 'OUI' ELSE 'NON' END,
    CASE
        WHEN "PasswordHash" IN (
            'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
            'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ=='
        ) THEN 'OUI'
        ELSE 'NON'
    END
FROM "AspNetUsers"
WHERE "Email" IN ('client@example.com', 'admin@example.com');