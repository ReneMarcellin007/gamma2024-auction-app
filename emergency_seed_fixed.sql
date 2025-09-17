-- NETTOYER TOUT (NOMS DE COLONNES CORRECTS)
DELETE FROM photos;
DELETE FROM encan_lots;
DELETE FROM lots;
DELETE FROM encans;
DELETE FROM categories WHERE "Id" > 10;
DELETE FROM mediums WHERE "Id" > 10;
DELETE FROM vendeurs WHERE "Id" > 10;

-- CRÉER CATÉGORIES
INSERT INTO categories ("Id", "Nom") VALUES
(100, 'Peinture'),
(101, 'Sculpture'),
(102, 'Photographie')
ON CONFLICT ("Id") DO NOTHING;

-- CRÉER MÉDIUMS
INSERT INTO mediums ("Id", "Type") VALUES
(100, 'Huile sur toile'),
(101, 'Acrylique'),
(102, 'Bronze')
ON CONFLICT ("Id") DO NOTHING;

-- CRÉER VENDEUR
INSERT INTO vendeurs ("Id", "Nom", "Prenom", "Courriel", "Telephone") VALUES
(100, 'Vendeur', 'Test', 'test@test.com', '555-0001')
ON CONFLICT ("Id") DO NOTHING;

-- CRÉER 3 ENCANS
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES
(100, 1, NOW() - INTERVAL '5 days', NOW() + INTERVAL '10 days', NOW() + INTERVAL '9 days', true, false, 1, 10),
(101, 2, NOW() - INTERVAL '30 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '16 days', true, true, 1, 10),
(102, 3, NOW() + INTERVAL '20 days', NOW() + INTERVAL '35 days', NOW() + INTERVAL '34 days', true, false, 1, 10);

-- CRÉER 20 LOTS
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot", "DateFinVente")
VALUES
-- Encan en cours (8 lots)
(100, 'LOT-001', 'Pablo Picasso', 'Nature morte aux fruits', 5000, 8000, 3000, 4000, 3200, false, true, 100, 100, 100, 60, 80, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(101, 'LOT-002', 'Claude Monet', 'Jardin à Giverny', 10000, 15000, 7000, 9000, 7500, false, true, 100, 100, 100, 90, 120, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(102, 'LOT-003', 'Vincent van Gogh', 'Champ de blé', 2000, 3000, 1500, 1800, 1600, false, true, 100, 100, 100, 50, 100, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(103, 'LOT-004', 'Leonardo da Vinci', 'Portrait', 20000, 30000, 15000, 18000, 15500, false, true, 100, 100, 100, 77, 53, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(104, 'LOT-005', 'Henri Matisse', 'Femme au chapeau', 8000, 12000, 6000, 7000, 0, false, true, 100, 100, 100, 80, 65, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(105, 'LOT-006', 'Salvador Dalí', 'Surréalisme', 15000, 25000, 12000, 14000, 0, false, true, 100, 100, 100, 24, 33, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(106, 'LOT-007', 'Jackson Pollock', 'Action painting', 30000, 50000, 25000, 28000, 0, false, true, 100, 100, 100, 200, 300, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),
(107, 'LOT-008', 'Edvard Munch', 'Le Cri', 18000, 28000, 15000, 17000, 0, false, true, 100, 100, 100, 91, 73, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days', NULL),

-- Encan passé (6 lots vendus)
(108, 'LOT-009', 'Auguste Rodin', 'Le Penseur', 3000, 5000, 2000, 2500, 4200, true, true, 101, 102, 100, 40, 30, NOW() - INTERVAL '40 days', NOW() - INTERVAL '35 days', NULL, NULL, NOW() - INTERVAL '15 days'),
(109, 'LOT-010', 'Alberto Giacometti', 'Homme qui marche', 8000, 12000, 6000, 7000, 9500, true, true, 101, 102, 100, 183, 95, NOW() - INTERVAL '40 days', NOW() - INTERVAL '35 days', NULL, NULL, NOW() - INTERVAL '15 days'),
(110, 'LOT-011', 'Henry Moore', 'Figure allongée', 12000, 18000, 10000, 11000, 15000, true, true, 101, 102, 100, 60, 150, NOW() - INTERVAL '40 days', NOW() - INTERVAL '35 days', NULL, NULL, NOW() - INTERVAL '15 days'),
(111, 'LOT-012', 'Ansel Adams', 'Moonrise', 5000, 8000, 4000, 4500, 6200, true, true, 102, 100, 100, 40, 50, NOW() - INTERVAL '40 days', NOW() - INTERVAL '35 days', NULL, NULL, NOW() - INTERVAL '15 days'),
(112, 'LOT-013', 'Georgia OKeeffe', 'Red Canna', 7000, 11000, 5500, 6000, 8000, true, true, 100, 100, 100, 91, 76, NOW() - INTERVAL '40 days', NOW() - INTERVAL '35 days', NULL, NULL, NOW() - INTERVAL '15 days'),
(113, 'LOT-014', 'M.C. Escher', 'Relativity', 3000, 5000, 2500, 2800, 3800, true, true, 100, 100, 100, 28, 29, NOW() - INTERVAL '40 days', NOW() - INTERVAL '35 days', NULL, NULL, NOW() - INTERVAL '15 days'),

-- Encan futur (6 lots)
(114, 'LOT-015', 'Frida Kahlo', 'Autoportrait', 25000, 40000, 20000, 22000, 0, false, true, 100, 100, 100, 61, 47, NOW(), NOW(), NOW() + INTERVAL '25 days', NOW() + INTERVAL '35 days', NULL),
(115, 'LOT-016', 'Andy Warhol', 'Pop Art', 15000, 25000, 12000, 14000, 0, false, true, 100, 101, 100, 51, 41, NOW(), NOW(), NOW() + INTERVAL '25 days', NOW() + INTERVAL '35 days', NULL),
(116, 'LOT-017', 'Banksy', 'Street Art', 10000, 18000, 8000, 9000, 0, false, true, 100, 101, 100, 100, 70, NOW(), NOW(), NOW() + INTERVAL '25 days', NOW() + INTERVAL '35 days', NULL),
(117, 'LOT-018', 'Yves Klein', 'IKB 191', 20000, 30000, 18000, 19000, 0, false, true, 100, 101, 100, 199, 153, NOW(), NOW(), NOW() + INTERVAL '25 days', NOW() + INTERVAL '35 days', NULL),
(118, 'LOT-019', 'Kaws', 'Companion', 8000, 15000, 6000, 7000, 0, false, true, 101, 102, 100, 130, 60, NOW(), NOW(), NOW() + INTERVAL '25 days', NOW() + INTERVAL '35 days', NULL),
(119, 'LOT-020', 'Takashi Murakami', 'Cherry Blossom', 12000, 20000, 10000, 11000, 0, false, true, 100, 101, 100, 150, 150, NOW(), NOW(), NOW() + INTERVAL '25 days', NOW() + INTERVAL '35 days', NULL);

-- CRÉER ASSOCIATIONS ENCAN-LOT
INSERT INTO encan_lots ("IdEncan", "IdLot")
VALUES
-- Encan en cours
(100, 100), (100, 101), (100, 102), (100, 103), (100, 104), (100, 105), (100, 106), (100, 107),
-- Encan passé
(101, 108), (101, 109), (101, 110), (101, 111), (101, 112), (101, 113),
-- Encan futur
(102, 114), (102, 115), (102, 116), (102, 117), (102, 118), (102, 119);

-- CRÉER PHOTOS
INSERT INTO photos ("Id", "IdLot", "Lien")
VALUES
(100, 100, 'https://via.placeholder.com/800x600/FF6B6B/FFFFFF?text=Picasso'),
(101, 101, 'https://via.placeholder.com/800x600/4ECDC4/FFFFFF?text=Monet'),
(102, 102, 'https://via.placeholder.com/800x600/45B7D1/FFFFFF?text=Van+Gogh'),
(103, 103, 'https://via.placeholder.com/800x600/96CEB4/FFFFFF?text=Da+Vinci'),
(104, 104, 'https://via.placeholder.com/800x600/DDA0DD/FFFFFF?text=Matisse'),
(105, 105, 'https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=Dali'),
(106, 106, 'https://via.placeholder.com/800x600/8A2BE2/FFFFFF?text=Pollock'),
(107, 107, 'https://via.placeholder.com/800x600/FFD700/FFFFFF?text=Munch'),
(108, 108, 'https://via.placeholder.com/800x600/DC143C/FFFFFF?text=Rodin'),
(109, 109, 'https://via.placeholder.com/800x600/FF4500/FFFFFF?text=Giacometti'),
(110, 110, 'https://via.placeholder.com/800x600/2E86AB/FFFFFF?text=Moore'),
(111, 111, 'https://via.placeholder.com/800x600/000000/FFFFFF?text=Adams'),
(112, 112, 'https://via.placeholder.com/800x600/FF1493/FFFFFF?text=OKeeffe'),
(113, 113, 'https://via.placeholder.com/800x600/4169E1/FFFFFF?text=Escher'),
(114, 114, 'https://via.placeholder.com/800x600/8B008B/FFFFFF?text=Frida'),
(115, 115, 'https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=Warhol'),
(116, 116, 'https://via.placeholder.com/800x600/DC143C/FFFFFF?text=Banksy'),
(117, 117, 'https://via.placeholder.com/800x600/0033FF/FFFFFF?text=Klein'),
(118, 118, 'https://via.placeholder.com/800x600/FF1493/FFFFFF?text=Kaws'),
(119, 119, 'https://via.placeholder.com/800x600/FFD700/FFFFFF?text=Murakami');

-- VÉRIFICATION FINALE
SELECT 'Encans créés:' as type, COUNT(*) as count FROM encans
UNION ALL
SELECT 'Lots créés:', COUNT(*) FROM lots
UNION ALL
SELECT 'Photos créées:', COUNT(*) FROM photos
UNION ALL
SELECT 'Encan en cours:', COUNT(*) FROM encans WHERE "DateDebut" <= NOW() AND "DateFin" >= NOW();