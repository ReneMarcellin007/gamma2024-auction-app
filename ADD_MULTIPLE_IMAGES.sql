-- ===== AJOUTER PLUSIEURS IMAGES PAR LOT (2-5 IMAGES) =====
-- Script pour ajouter les images locales depuis /images/lots/

-- 1. SUPPRIMER LES ANCIENNES PHOTOS
DELETE FROM photos;

-- 2. AJOUTER 3-5 PHOTOS PAR LOT DEPUIS LES IMAGES LOCALES

-- ENCAN 234 (Présent) - Lots 100-108
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Lot 100 (Léon Bellefleur) - 4 images
(1000, 100, '/images/lots/ImagesEncan234/1_1.jpg'),
(1001, 100, '/images/lots/ImagesEncan234/1_2.jpg'),
(1002, 100, '/images/lots/ImagesEncan234/1_3.jpg'),
(1003, 100, '/images/lots/ImagesEncan234/1_4.jpg'),
-- Lot 101 (Claude Dulude) - 4 images
(1010, 101, '/images/lots/ImagesEncan234/2_1.jpg'),
(1011, 101, '/images/lots/ImagesEncan234/2_2.jpg'),
(1012, 101, '/images/lots/ImagesEncan234/2_3.jpg'),
(1013, 101, '/images/lots/ImagesEncan234/2_4.jpg'),
-- Lot 102 (Guy Paquet) - 3 images
(1020, 102, '/images/lots/ImagesEncan234/3_1.jpg'),
(1021, 102, '/images/lots/ImagesEncan234/3_2.jpg'),
(1022, 102, '/images/lots/ImagesEncan234/3_3.jpg'),
-- Lot 103 (Paul Tex Lecor - Solitude) - 4 images
(1030, 103, '/images/lots/ImagesEncan234/4_1.jpg'),
(1031, 103, '/images/lots/ImagesEncan234/4_2.webp'),
(1032, 103, '/images/lots/ImagesEncan234/4_3.jpg'),
(1033, 103, '/images/lots/ImagesEncan234/4_4.jpg'),
-- Lot 104 (Paul Tex Lecor - Campagne) - 3 images
(1040, 104, '/images/lots/ImagesEncan234/4a_1.jpg'),
(1041, 104, '/images/lots/ImagesEncan234/4a_2.jpg'),
(1042, 104, '/images/lots/ImagesEncan234/4a_3.jpg'),
-- Lot 105 (Paul Tex Lecor - Ormes) - 3 images
(1050, 105, '/images/lots/ImagesEncan234/4b_1.jpg'),
(1051, 105, '/images/lots/ImagesEncan234/4b_2.jpg'),
(1052, 105, '/images/lots/ImagesEncan234/4b_3.jpg'),
-- Lot 106 (Christian Bergeron) - 2 images
(1060, 106, '/images/lots/ImagesEncan234/5_1.jpg'),
(1061, 106, '/images/lots/ImagesEncan234/5_2.jpg'),
-- Lot 107 (Bruno Côté) - 4 images
(1070, 107, '/images/lots/ImagesEncan234/6_1.jpg'),
(1071, 107, '/images/lots/ImagesEncan234/6_2.webp'),
(1072, 107, '/images/lots/ImagesEncan234/6_3.jpg'),
(1073, 107, '/images/lots/ImagesEncan234/6_4.jpg'),
-- Lot 108 (Serge Lemoyne) - 4 images
(1080, 108, '/images/lots/ImagesEncan234/7_1.webp'),
(1081, 108, '/images/lots/ImagesEncan234/7_2.jpg'),
(1082, 108, '/images/lots/ImagesEncan234/7_3.jpg'),
(1083, 108, '/images/lots/ImagesEncan234/7_4.jpg');

-- ENCAN 232 (Passé) - Lots 200-208
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Lot 200 (Guy Paquet) - 3 images
(2000, 200, '/images/lots/ImagesEncan232/1_1.jfif'),
(2001, 200, '/images/lots/ImagesEncan232/1_2.jfif'),
(2002, 200, '/images/lots/ImagesEncan232/1_3.jfif'),
-- Lot 201 (Vladimir Horik - Messe) - 2 images
(2010, 201, '/images/lots/ImagesEncan232/1a_1.jfif'),
(2011, 201, '/images/lots/ImagesEncan232/1a_2.jfif'),
-- Lot 202 (Vladimir Horik - Paysage) - 3 images
(2020, 202, '/images/lots/ImagesEncan232/1b_1.jfif'),
(2021, 202, '/images/lots/ImagesEncan232/1b_2.jfif'),
(2022, 202, '/images/lots/ImagesEncan232/1b_3.jfif'),
-- Lot 203 (Marcel Poirier) - 3 images
(2030, 203, '/images/lots/ImagesEncan232/2_1.jfif'),
(2031, 203, '/images/lots/ImagesEncan232/2_2.jfif'),
(2032, 203, '/images/lots/ImagesEncan232/2_3.jfif'),
-- Lot 204 (Normand Hudon - Bonhomme) - 2 images
(2040, 204, '/images/lots/ImagesEncan232/3_1.jfif'),
(2041, 204, '/images/lots/ImagesEncan232/3_2.jfif'),
-- Lot 205 (Normand Hudon - Vent) - 2 images
(2050, 205, '/images/lots/ImagesEncan232/4_1.jfif'),
(2051, 205, '/images/lots/ImagesEncan232/4_2.jfif'),
-- Lot 206 (Normand Hudon - Saint Esprit) - 3 images
(2060, 206, '/images/lots/ImagesEncan232/5_1.jfif'),
(2061, 206, '/images/lots/ImagesEncan232/5_2.jfif'),
(2062, 206, '/images/lots/ImagesEncan232/5_3.jfif'),
-- Lot 207 (Claude Langevin) - 3 images
(2070, 207, '/images/lots/ImagesEncan232/6_1.jfif'),
(2071, 207, '/images/lots/ImagesEncan232/6_2.jfif'),
(2072, 207, '/images/lots/ImagesEncan232/6_3.jfif'),
-- Lot 208 (Raymond Girard) - 2 images
(2080, 208, '/images/lots/ImagesEncan232/7_1.jfif'),
(2081, 208, '/images/lots/ImagesEncan232/7_2.jfif');

-- ENCAN 233 (Passé) - Lots 300-304
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Lot 300 (Jean Gaudreau) - 3 images
(3000, 300, '/images/lots/ImagesEncan233/1_1.jfif'),
(3001, 300, '/images/lots/ImagesEncan233/1_2.jfif'),
(3002, 300, '/images/lots/ImagesEncan233/1_3.jfif'),
-- Lot 301 (Marc Siméon) - 2 images
(3010, 301, '/images/lots/ImagesEncan233/2_1.jfif'),
(3011, 301, '/images/lots/ImagesEncan233/2_2.jfif'),
-- Lot 302 (Denis Juneau) - 2 images
(3020, 302, '/images/lots/ImagesEncan233/3_1.jfif'),
(3021, 302, '/images/lots/ImagesEncan233/3_2.jfif'),
-- Lot 303 (Jean-Paul Jérome) - 2 images
(3030, 303, '/images/lots/ImagesEncan233/4_1.jfif'),
(3031, 303, '/images/lots/ImagesEncan233/4_2.jfif'),
-- Lot 304 (Claude Dulude) - 2 images
(3040, 304, '/images/lots/ImagesEncan233/5_1.jfif'),
(3041, 304, '/images/lots/ImagesEncan233/5_2.jfif');

-- ENCAN 235 (Futur) - Lots 400-407
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Lot 400 (Denis Juneau) - 2 images
(4000, 400, '/images/lots/ImagesEncan235/1_1.jfif'),
(4001, 400, '/images/lots/ImagesEncan235/1_2.jfif'),
-- Lot 401 (Serge Lemoyne) - 3 images
(4010, 401, '/images/lots/ImagesEncan235/2_1.jfif'),
(4011, 401, '/images/lots/ImagesEncan235/2_2.jfif'),
(4012, 401, '/images/lots/ImagesEncan235/2_3.jfif'),
-- Lot 402 (Marc Siméon) - 2 images
(4020, 402, '/images/lots/ImagesEncan235/3_1.jfif'),
(4021, 402, '/images/lots/ImagesEncan235/3_2.jfif'),
-- Lot 403 (Jean Gaudreau) - 3 images
(4030, 403, '/images/lots/ImagesEncan235/4_1.jfif'),
(4031, 403, '/images/lots/ImagesEncan235/4_2.jfif'),
(4032, 403, '/images/lots/ImagesEncan235/4_3.jfif'),
-- Lot 404 (Jean-Paul Jérome) - 2 images
(4040, 404, '/images/lots/ImagesEncan235/5_1.jfif'),
(4041, 404, '/images/lots/ImagesEncan235/5_2.jfif'),
-- Lot 405 (Riopelle Jazz) - 2 images
(4050, 405, '/images/lots/ImagesEncan235/6_1.jfif'),
(4051, 405, '/images/lots/ImagesEncan235/6_2.jfif'),
-- Lot 406 (Riopelle Oies) - 3 images
(4060, 406, '/images/lots/ImagesEncan235/7_1.jfif'),
(4061, 406, '/images/lots/ImagesEncan235/7_2.jfif'),
(4062, 406, '/images/lots/ImagesEncan235/7_3.jfif'),
-- Lot 407 (Riopelle Ours) - 3 images
(4070, 407, '/images/lots/ImagesEncan235/8_1.jfif'),
(4071, 407, '/images/lots/ImagesEncan235/8_2.jfif'),
(4072, 407, '/images/lots/ImagesEncan235/8_3.jfif');

-- ENCAN 236 (Futur) - Lots 500-505 - Utiliser des images variées
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Lot 500 - 3 images
(5000, 500, '/images/lots/ImagesEncan234/10_1.jpg'),
(5001, 500, '/images/lots/ImagesEncan234/10_2.jpg'),
(5002, 500, '/images/lots/ImagesEncan234/10_3.jpg'),
-- Lot 501 - 2 images
(5010, 501, '/images/lots/ImagesEncan234/11_1.jpg'),
(5011, 501, '/images/lots/ImagesEncan234/11_2.jpg'),
-- Lot 502 - 3 images
(5020, 502, '/images/lots/ImagesEncan234/12_1.jpg'),
(5021, 502, '/images/lots/ImagesEncan234/12_2.jpg'),
(5022, 502, '/images/lots/ImagesEncan234/12_3.jpg'),
-- Lot 503 - 3 images
(5030, 503, '/images/lots/ImagesEncan234/13_1.jpg'),
(5031, 503, '/images/lots/ImagesEncan234/13_2.jpg'),
(5032, 503, '/images/lots/ImagesEncan234/13_3.jpg'),
-- Lot 504 - 2 images
(5040, 504, '/images/lots/ImagesEncan234/14_1.jpg'),
(5041, 504, '/images/lots/ImagesEncan234/14_2.jpg'),
-- Lot 505 - 3 images
(5050, 505, '/images/lots/ImagesEncan234/15_1.jpg'),
(5051, 505, '/images/lots/ImagesEncan234/15_2.jpg'),
(5052, 505, '/images/lots/ImagesEncan234/15_3.webp');

-- 3. VÉRIFICATIONS
SELECT '======================================' as info;
SELECT 'PHOTOS AJOUTÉES PAR LOT:' as titre;
SELECT '======================================' as info;

SELECT
    l."Numero" as numero_lot,
    l."Artiste" as artiste,
    COUNT(p."Id") as nb_photos,
    STRING_AGG(RIGHT(p."Lien", 15), ', ') as fichiers
FROM lots l
LEFT JOIN photos p ON l."Id" = p."IdLot"
GROUP BY l."Id", l."Numero", l."Artiste"
ORDER BY l."Id"
LIMIT 10;

SELECT '======================================' as info;
SELECT 'TOTAL PHOTOS:' as titre, COUNT(*) as total FROM photos;
SELECT '======================================' as info;