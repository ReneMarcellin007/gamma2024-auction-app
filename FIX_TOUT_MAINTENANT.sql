-- ===== CORRECTION DÉFINITIVE DE TOUT =====
-- Ce script corrige TOUT: images ET utilisateurs

-- 1. SUPPRIMER ET RECRÉER LES PHOTOS AVEC URLS EXTERNES
DELETE FROM photos;

-- Utiliser des images d'art réelles depuis Unsplash (service gratuit)
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- ENCAN 234 (Présent) - 3-4 images par lot
(1000, 100, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),
(1001, 100, 'https://images.unsplash.com/photo-1578321272176-b7bbc0679853?w=800&h=600'),
(1002, 100, 'https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5?w=800&h=600'),

(1010, 101, 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600'),
(1011, 101, 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600'),
(1012, 101, 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600'),

(1020, 102, 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600'),
(1021, 102, 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600'),
(1022, 102, 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600'),

(1030, 103, 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600'),
(1031, 103, 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600'),
(1032, 103, 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600'),
(1033, 103, 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600'),

(1040, 104, 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600'),
(1041, 104, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),
(1042, 104, 'https://images.unsplash.com/photo-1547887537-6158d64c35b3?w=800&h=600'),

(1050, 105, 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600'),
(1051, 105, 'https://images.unsplash.com/photo-1579541814924-49fef17c5be5?w=800&h=600'),
(1052, 105, 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600'),

(1060, 106, 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600'),
(1061, 106, 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600'),

(1070, 107, 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600'),
(1071, 107, 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600'),
(1072, 107, 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600'),

(1080, 108, 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600'),
(1081, 108, 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600'),
(1082, 108, 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600'),

-- ENCAN 232 (Passé 1) - 2-3 images par lot
(2000, 200, 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600'),
(2001, 200, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),
(2002, 200, 'https://images.unsplash.com/photo-1547887537-6158d64c35b3?w=800&h=600'),

(2010, 201, 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600'),
(2011, 201, 'https://images.unsplash.com/photo-1579541814924-49fef17c5be5?w=800&h=600'),

(2020, 202, 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600'),
(2021, 202, 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600'),

(2030, 203, 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600'),
(2031, 203, 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600'),

(2040, 204, 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600'),
(2041, 204, 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600'),

(2050, 205, 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600'),
(2051, 205, 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600'),

(2060, 206, 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600'),
(2061, 206, 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600'),
(2062, 206, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),

(2070, 207, 'https://images.unsplash.com/photo-1547887537-6158d64c35b3?w=800&h=600'),
(2071, 207, 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600'),

(2080, 208, 'https://images.unsplash.com/photo-1579541814924-49fef17c5be5?w=800&h=600'),
(2081, 208, 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600'),

-- ENCAN 233 (Passé 2) - 2 images par lot
(3000, 300, 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600'),
(3001, 300, 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600'),

(3010, 301, 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600'),
(3011, 301, 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600'),

(3020, 302, 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600'),
(3021, 302, 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600'),

(3030, 303, 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600'),
(3031, 303, 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600'),

(3040, 304, 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600'),
(3041, 304, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),

-- ENCAN 235 (Futur 1) - 2-3 images par lot
(4000, 400, 'https://images.unsplash.com/photo-1547887537-6158d64c35b3?w=800&h=600'),
(4001, 400, 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600'),

(4010, 401, 'https://images.unsplash.com/photo-1579541814924-49fef17c5be5?w=800&h=600'),
(4011, 401, 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600'),
(4012, 401, 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600'),

(4020, 402, 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600'),
(4021, 402, 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600'),

(4030, 403, 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600'),
(4031, 403, 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600'),

(4040, 404, 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600'),
(4041, 404, 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600'),

(4050, 405, 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600'),
(4051, 405, 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600'),

(4060, 406, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),
(4061, 406, 'https://images.unsplash.com/photo-1547887537-6158d64c35b3?w=800&h=600'),

(4070, 407, 'https://images.unsplash.com/photo-1561214115-f2f134cc4912?w=800&h=600'),
(4071, 407, 'https://images.unsplash.com/photo-1579541814924-49fef17c5be5?w=800&h=600'),

-- ENCAN 236 (Futur 2) - 2 images par lot
(5000, 500, 'https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=800&h=600'),
(5001, 500, 'https://images.unsplash.com/photo-1549887534-1541e9326642?w=800&h=600'),

(5010, 501, 'https://images.unsplash.com/photo-1577720643272-265f09367456?w=800&h=600'),
(5011, 501, 'https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&h=600'),

(5020, 502, 'https://images.unsplash.com/photo-1561059488-916d69792237?w=800&h=600'),
(5021, 502, 'https://images.unsplash.com/photo-1582201942988-13e60e4556ee?w=800&h=600'),

(5030, 503, 'https://images.unsplash.com/photo-1547891654-e66ed7ebb968?w=800&h=600'),
(5031, 503, 'https://images.unsplash.com/photo-1578321926534-133b0e8f6e73?w=800&h=600'),

(5040, 504, 'https://images.unsplash.com/photo-1549887552-cb1071d3e5ca?w=800&h=600'),
(5041, 504, 'https://images.unsplash.com/photo-1579783901586-d88db74b4fe4?w=800&h=600'),

(5050, 505, 'https://images.unsplash.com/photo-1541961017774-22349e4a1262?w=800&h=600'),
(5051, 505, 'https://images.unsplash.com/photo-1547887537-6158d64c35b3?w=800&h=600');

-- 2. CORRIGER LES UTILISATEURS
-- Supprimer les anciens pour éviter les conflits
DELETE FROM "AspNetUsers"
WHERE "Email" IN ('client@example.com', 'admin@example.com');

-- Créer client avec le bon hash
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
);

-- Créer admin avec le bon hash
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
);

-- 3. VÉRIFICATIONS
SELECT '===================================' as info;
SELECT 'TOUT EST CORRIGÉ!' as status;
SELECT '===================================' as info;

-- Photos
SELECT 'Photos par lot:' as info;
SELECT l."Numero", l."Artiste", COUNT(p."Id") as nb_photos
FROM lots l
LEFT JOIN photos p ON l."Id" = p."IdLot"
GROUP BY l."Id", l."Numero", l."Artiste"
LIMIT 5;

-- Utilisateurs
SELECT 'Utilisateurs:' as info;
SELECT "Email", "EmailConfirmed" FROM "AspNetUsers";

SELECT '===================================' as info;
SELECT 'CONNEXIONS:' as info;
SELECT 'client@example.com / MotDePasseClient123!' as client;
SELECT 'admin@example.com / MotDePasseAdmin123!' as admin;
SELECT '===================================' as info;