-- ===== PARTIE 1: CORRIGER TOUTES LES IMAGES =====
-- Exécutez ce script EN PREMIER dans Neon Console

-- 1. Supprimer les photos cassées
DELETE FROM photos;

-- 2. Recréer TOUTES les photos avec URLs FONCTIONNELLES (picsum.photos)
INSERT INTO photos ("Id", "IdLot", "Lien") VALUES
-- Encan 234 (présent)
(100, 100, 'https://picsum.photos/800/600?random=100'),
(101, 101, 'https://picsum.photos/800/600?random=101'),
(102, 102, 'https://picsum.photos/800/600?random=102'),
(103, 103, 'https://picsum.photos/800/600?random=103'),
(104, 104, 'https://picsum.photos/800/600?random=104'),
(105, 105, 'https://picsum.photos/800/600?random=105'),
(106, 106, 'https://picsum.photos/800/600?random=106'),
(107, 107, 'https://picsum.photos/800/600?random=107'),
(108, 108, 'https://picsum.photos/800/600?random=108'),
-- Encan 232 (passé)
(200, 200, 'https://picsum.photos/800/600?random=200'),
(201, 201, 'https://picsum.photos/800/600?random=201'),
(202, 202, 'https://picsum.photos/800/600?random=202'),
(203, 203, 'https://picsum.photos/800/600?random=203'),
(204, 204, 'https://picsum.photos/800/600?random=204'),
(205, 205, 'https://picsum.photos/800/600?random=205'),
(206, 206, 'https://picsum.photos/800/600?random=206'),
(207, 207, 'https://picsum.photos/800/600?random=207'),
(208, 208, 'https://picsum.photos/800/600?random=208'),
-- Encan 233 (passé)
(300, 300, 'https://picsum.photos/800/600?random=300'),
(301, 301, 'https://picsum.photos/800/600?random=301'),
(302, 302, 'https://picsum.photos/800/600?random=302'),
(303, 303, 'https://picsum.photos/800/600?random=303'),
(304, 304, 'https://picsum.photos/800/600?random=304'),
-- Encan 235 (futur)
(400, 400, 'https://picsum.photos/800/600?random=400'),
(401, 401, 'https://picsum.photos/800/600?random=401'),
(402, 402, 'https://picsum.photos/800/600?random=402'),
(403, 403, 'https://picsum.photos/800/600?random=403'),
(404, 404, 'https://picsum.photos/800/600?random=404'),
(405, 405, 'https://picsum.photos/800/600?random=405'),
(406, 406, 'https://picsum.photos/800/600?random=406'),
(407, 407, 'https://picsum.photos/800/600?random=407'),
-- Encan 236 (futur)
(500, 500, 'https://picsum.photos/800/600?random=500'),
(501, 501, 'https://picsum.photos/800/600?random=501'),
(502, 502, 'https://picsum.photos/800/600?random=502'),
(503, 503, 'https://picsum.photos/800/600?random=503'),
(504, 504, 'https://picsum.photos/800/600?random=504'),
(505, 505, 'https://picsum.photos/800/600?random=505')
ON CONFLICT ("Id") DO UPDATE SET "Lien" = EXCLUDED."Lien";

-- 3. Vérification
SELECT 'IMAGES CORRIGÉES!' as status, COUNT(*) as total FROM photos;
SELECT "IdLot", LEFT("Lien", 40) as url FROM photos LIMIT 5;