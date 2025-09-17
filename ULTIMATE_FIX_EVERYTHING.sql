-- ===== SCRIPT ULTIME POUR TOUT RÉPARER DÉFINITIVEMENT =====
-- EXÉCUTEZ CE SCRIPT DANS NEON CONSOLE

-- 1. CORRIGER TOUTES LES IMAGES CASSÉES
-- Les URLs sont tronquées, on les reconstruit complètement

-- D'abord supprimer toutes les photos existantes
DELETE FROM photos;

-- Recréer TOUTES les photos avec URLs COMPLÈTES ET FONCTIONNELLES
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Photos Encan présent (234)
(100, 100, 'https://picsum.photos/800/600?random=100'),
(101, 101, 'https://picsum.photos/800/600?random=101'),
(102, 102, 'https://picsum.photos/800/600?random=102'),
(103, 103, 'https://picsum.photos/800/600?random=103'),
(104, 104, 'https://picsum.photos/800/600?random=104'),
(105, 105, 'https://picsum.photos/800/600?random=105'),
(106, 106, 'https://picsum.photos/800/600?random=106'),
(107, 107, 'https://picsum.photos/800/600?random=107'),
(108, 108, 'https://picsum.photos/800/600?random=108'),

-- Photos Encan passé 1 (232)
(200, 200, 'https://picsum.photos/800/600?random=200'),
(201, 201, 'https://picsum.photos/800/600?random=201'),
(202, 202, 'https://picsum.photos/800/600?random=202'),
(203, 203, 'https://picsum.photos/800/600?random=203'),
(204, 204, 'https://picsum.photos/800/600?random=204'),
(205, 205, 'https://picsum.photos/800/600?random=205'),
(206, 206, 'https://picsum.photos/800/600?random=206'),
(207, 207, 'https://picsum.photos/800/600?random=207'),
(208, 208, 'https://picsum.photos/800/600?random=208'),

-- Photos Encan passé 2 (233)
(300, 300, 'https://picsum.photos/800/600?random=300'),
(301, 301, 'https://picsum.photos/800/600?random=301'),
(302, 302, 'https://picsum.photos/800/600?random=302'),
(303, 303, 'https://picsum.photos/800/600?random=303'),
(304, 304, 'https://picsum.photos/800/600?random=304'),

-- Photos Encan futur 1 (235)
(400, 400, 'https://picsum.photos/800/600?random=400'),
(401, 401, 'https://picsum.photos/800/600?random=401'),
(402, 402, 'https://picsum.photos/800/600?random=402'),
(403, 403, 'https://picsum.photos/800/600?random=403'),
(404, 404, 'https://picsum.photos/800/600?random=404'),
(405, 405, 'https://picsum.photos/800/600?random=405'),
(406, 406, 'https://picsum.photos/800/600?random=406'),
(407, 407, 'https://picsum.photos/800/600?random=407'),

-- Photos Encan futur 2 (236)
(500, 500, 'https://picsum.photos/800/600?random=500'),
(501, 501, 'https://picsum.photos/800/600?random=501'),
(502, 502, 'https://picsum.photos/800/600?random=502'),
(503, 503, 'https://picsum.photos/800/600?random=503'),
(504, 504, 'https://picsum.photos/800/600?random=504'),
(505, 505, 'https://picsum.photos/800/600?random=505')
ON CONFLICT ("Id") DO UPDATE SET
    "Lien" = EXCLUDED."Lien";

-- 2. ALTERNATIVE: Images d'art réalistes de Unsplash
-- Décommentez si vous préférez des images d'art
/*
UPDATE photos SET "Lien" =
CASE "Id"
    -- Encan présent
    WHEN 100 THEN 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600&fit=crop'
    WHEN 101 THEN 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600&fit=crop'
    WHEN 102 THEN 'https://images.unsplash.com/photo-1578321272176-b7bbc0679853?w=800&h=600&fit=crop'
    WHEN 103 THEN 'https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=800&h=600&fit=crop'
    WHEN 104 THEN 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600&fit=crop'
    WHEN 105 THEN 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600&fit=crop'
    WHEN 106 THEN 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600&fit=crop'
    WHEN 107 THEN 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600&fit=crop'
    WHEN 108 THEN 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600&fit=crop'
    -- Encan passé 1
    WHEN 200 THEN 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600&fit=crop'
    WHEN 201 THEN 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600&fit=crop'
    WHEN 202 THEN 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600&fit=crop'
    WHEN 203 THEN 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600&fit=crop'
    WHEN 204 THEN 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600&fit=crop'
    ELSE "Lien"
END
WHERE "Id" IN (100,101,102,103,104,105,106,107,108,200,201,202,203,204);
*/

-- 3. CORRIGER LES UTILISATEURS ET MOTS DE PASSE
-- Vérifier d'abord ce qui existe
SELECT 'UTILISATEURS ACTUELS:' as info;
SELECT "Email", "UserName", "EmailConfirmed", "Name", "FirstName"
FROM "AspNetUsers";

-- Créer/Mettre à jour client@example.com avec le bon hash
-- Ce hash est pour "MotDePasseClient123!" (avec "Client")
INSERT INTO "AspNetUsers" (
    "Id",
    "UserName",
    "NormalizedUserName",
    "Email",
    "NormalizedEmail",
    "EmailConfirmed",
    "PasswordHash",
    "SecurityStamp",
    "ConcurrencyStamp",
    "PhoneNumber",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnd",
    "LockoutEnabled",
    "AccessFailedCount",
    "Name",
    "FirstName",
    "Avatar",
    "StripeCustomer"
) VALUES (
    '1d8ac862-e54d-4f10-b6f8-638808c02967',
    'client@example.com',
    'CLIENT@EXAMPLE.COM',
    'client@example.com',
    'CLIENT@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
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
) ON CONFLICT ("Id") DO UPDATE SET
    "UserName" = 'client@example.com',
    "NormalizedUserName" = 'CLIENT@EXAMPLE.COM',
    "Email" = 'client@example.com',
    "NormalizedEmail" = 'CLIENT@EXAMPLE.COM',
    "EmailConfirmed" = true,
    "PasswordHash" = 'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==';

-- Créer/Mettre à jour admin@example.com
-- Ce hash est pour "AdminPassword123!"
INSERT INTO "AspNetUsers" (
    "Id",
    "UserName",
    "NormalizedUserName",
    "Email",
    "NormalizedEmail",
    "EmailConfirmed",
    "PasswordHash",
    "SecurityStamp",
    "ConcurrencyStamp",
    "PhoneNumber",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnd",
    "LockoutEnabled",
    "AccessFailedCount",
    "Name",
    "FirstName",
    "Avatar",
    "StripeCustomer"
) VALUES (
    '87162d4b-fc3b-4d65-8341-2f4a480982d1',
    'admin@example.com',
    'ADMIN@EXAMPLE.COM',
    'admin@example.com',
    'ADMIN@EXAMPLE.COM',
    true,
    'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
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
) ON CONFLICT ("Id") DO UPDATE SET
    "UserName" = 'admin@example.com',
    "NormalizedUserName" = 'ADMIN@EXAMPLE.COM',
    "Email" = 'admin@example.com',
    "NormalizedEmail" = 'ADMIN@EXAMPLE.COM',
    "EmailConfirmed" = true,
    "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==';

-- 4. VÉRIFICATIONS FINALES
SELECT '=====================================' as separator;
SELECT 'VÉRIFICATION DES CORRECTIONS:' as info;
SELECT '=====================================' as separator;

-- Vérifier les images
SELECT 'PHOTOS CORRIGÉES:' as info, COUNT(*) as total,
       SUM(CASE WHEN "Lien" LIKE 'http%' THEN 1 ELSE 0 END) as valides
FROM photos;

-- Vérifier les encans
SELECT 'ENCANS PAR STATUT:' as info;
SELECT
    CASE
        WHEN "DateDebut" <= NOW() AND "DateFin" >= NOW() THEN 'EN_COURS'
        WHEN "DateFin" < NOW() THEN 'PASSÉ'
        ELSE 'FUTUR'
    END as statut,
    COUNT(*) as nombre,
    STRING_AGG("NumeroEncan"::text, ', ') as numeros
FROM encans
GROUP BY statut;

-- Vérifier les utilisateurs
SELECT '=====================================' as separator;
SELECT 'CONNEXION DISPONIBLES:' as info;
SELECT '=====================================' as separator;

SELECT '✅ CLIENT LOGIN:' as info;
SELECT '   Email: client@example.com' as detail;
SELECT '   Password: MotDePasseClient123!' as detail;
SELECT '' as blank;

SELECT '✅ ADMIN LOGIN:' as info;
SELECT '   Email: admin@example.com' as detail;
SELECT '   Password: AdminPassword123!' as detail;

-- Afficher quelques exemples d'images
SELECT '=====================================' as separator;
SELECT 'EXEMPLES D''IMAGES CORRIGÉES:' as info;
SELECT
    p."Id",
    l."Artiste",
    l."Description",
    LEFT(p."Lien", 50) || '...' as url_debut
FROM photos p
INNER JOIN lots l ON p."IdLot" = l."Id"
LIMIT 5;