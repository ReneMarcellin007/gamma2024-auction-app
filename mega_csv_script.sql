-- ===== SCRIPT MÉGA AVEC DONNÉES CSV COMPLÈTES =====
-- Configuration : 2 futurs, 2 passés, 1 présent
-- BEAUCOUP de lots avec images placeholder fonctionnelles

-- 1. NETTOYER COMPLÈTEMENT TOUT
DELETE FROM photos;
DELETE FROM encan_lots;
DELETE FROM lots;
DELETE FROM encans;
DELETE FROM vendeurs;
DELETE FROM adresses;
DELETE FROM categories WHERE "Id" >= 100;
DELETE FROM mediums WHERE "Id" >= 100;

-- 2. CRÉER LES DONNÉES DE BASE
INSERT INTO adresses ("Id", "Numero", "Rue", "Ville", "Province", "Pays", "CodePostal", "EstDomicile")
VALUES (100, 123, 'Rue CSV', 'Montreal', 'Quebec', 'Canada', 'H2X1Y2', false);

INSERT INTO vendeurs ("Id", "Nom", "Prenom", "Courriel", "Telephone", "AdresseId")
VALUES (100, 'Vendeur', 'CSV', 'csv@gamma.com', '555-0100', 100);

INSERT INTO categories ("Id", "Nom") VALUES
(100, 'Art-Paintings'),
(101, 'Art-Prints/Lithographs'),
(102, 'Sculpture');

INSERT INTO mediums ("Id", "Type") VALUES
(100, 'Huile sur toile'),
(101, 'Huile sur panneau'),
(102, 'Lithographie'),
(103, 'Aquarelle'),
(104, 'Sérigraphie'),
(105, 'Encre');

-- 3. CRÉER 5 ENCANS SELON LA DEMANDE
-- ENCAN PRÉSENT (ID=100) - En cours maintenant
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (100, 234, NOW() - INTERVAL '3 days', NOW() + INTERVAL '7 days', NOW() + INTERVAL '6 days', true, false, 1, 10);

-- ENCAN PASSÉ 1 (ID=101) - Terminé récemment, non marqué terminé
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (101, 232, NOW() - INTERVAL '15 days', NOW() - INTERVAL '2 days', NOW() - INTERVAL '3 days', true, false, 1, 10);

-- ENCAN PASSÉ 2 (ID=102) - Terminé il y a longtemps, non marqué terminé
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (102, 233, NOW() - INTERVAL '30 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '16 days', true, false, 1, 10);

-- ENCAN FUTUR 1 (ID=103) - Dans 2 semaines
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (103, 235, NOW() + INTERVAL '14 days', NOW() + INTERVAL '28 days', NOW() + INTERVAL '27 days', true, false, 1, 10);

-- ENCAN FUTUR 2 (ID=104) - Dans 1 mois
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (104, 236, NOW() + INTERVAL '30 days', NOW() + INTERVAL '45 days', NOW() + INTERVAL '44 days', true, false, 1, 10);

-- 4. LOTS ENCAN PRÉSENT (234) - Données CSV Encan234
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
VALUES
(100, '1', 'Léon Bellefleur', '1968/Deux personnages, bon augure', 500, 800, 325, 400, 400, false, true, 100, 102, 100, 20, 17, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(101, '2', 'Claude Dulude', '1972/Abstraction', 200, 300, 80, 150, 150, false, true, 100, 100, 100, 9, 12, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(102, '3', 'Guy Paquet', 'Un marcheur est passé', 650, 950, 250, 400, 450, false, true, 100, 100, 100, 14, 20, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(103, '4', 'Paul Tex Lecor', 'Solitude', 4500, 7000, 2500, 3500, 3800, false, true, 100, 100, 100, 36, 30, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(104, '4a', 'Paul Tex Lecor', 'Campagne', 3000, 4500, 1250, 2000, 2200, false, true, 100, 100, 100, 24, 20, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(105, '4b', 'Paul Tex Lecor', 'Les grands ormes', 1500, 2500, 800, 1200, 1300, false, true, 100, 100, 100, 10, 12, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(106, '5', 'Christian Bergeron', 'Paysage automne', 800, 1200, 450, 600, 650, false, false, 100, 100, 100, 31, 23, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(107, '6', 'Bruno Côté', 'Vallée rivière Malbaie', 6000, 8000, 3250, 4500, 4800, false, false, 100, 101, 100, 36, 30, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
(108, '7', 'Serge Lemoyne', '1978/Road Runner', 2500, 3000, 1000, 1800, 1900, false, true, 101, 104, 100, 17, 23, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days');

-- 5. LOTS ENCAN PASSÉ 1 (232) - Avec mises élevées car terminé
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
VALUES
(200, '1', 'Guy Paquet', '1984/La nouvelle pratique', 600, 800, 200, 400, 850, false, true, 100, 101, 100, 16, 12, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(201, '1a', 'Vladimir Horik', 'La messe du soir', 3500, 5000, 2200, 3000, 5200, false, true, 100, 101, 100, 30, 20, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(202, '1b', 'Vladimir Horik', '1979/Paysage', 700, 1200, 400, 600, 1350, false, true, 100, 101, 100, 16, 12, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(203, '2', 'Marcel Poirier', '1979/Début d automne', 250, 350, 150, 200, 380, false, true, 100, 100, 100, 20, 16, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(204, '3', 'Normand Hudon', '1989/Le beau bonhomme', 5500, 6500, 3000, 4000, 6800, false, true, 100, 101, 100, 20, 16, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(205, '4', 'Normand Hudon', '1988/Coup d vent', 3000, 4000, 1500, 2500, 4200, false, true, 100, 101, 100, 12, 16, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(206, '5', 'Normand Hudon', '1990/Le père, le fils et le saint esprit. Amen', 1500, 2000, 800, 1200, 2100, false, true, 100, 101, 100, 16, 12, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(207, '6', 'Claude Langevin', 'Sous-bois', 1500, 2000, 800, 1200, 2050, false, true, 100, 100, 100, 24, 20, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days'),
(208, '7', 'Raymond Girard', 'Village Charlevoix', 600, 1000, 300, 500, 1100, false, true, 100, 100, 100, 40, 30, NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days', NOW() - INTERVAL '8 days', NOW() - INTERVAL '2 days');

-- 6. LOTS ENCAN PASSÉ 2 (233) - Aussi avec mises élevées
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
VALUES
(300, '8', 'Jean Gaudreau', 'Portrait mystique', 800, 1200, 450, 650, 1250, false, true, 100, 100, 100, 24, 18, NOW() - INTERVAL '35 days', NOW() - INTERVAL '30 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days'),
(301, '9', 'Marc Siméon', 'Abstraction bleue', 1000, 1500, 600, 900, 1600, false, true, 100, 100, 100, 30, 24, NOW() - INTERVAL '35 days', NOW() - INTERVAL '30 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days'),
(302, '10', 'Denis Juneau', 'Paysage hivernal', 500, 800, 300, 450, 850, false, true, 100, 103, 100, 20, 16, NOW() - INTERVAL '35 days', NOW() - INTERVAL '30 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days'),
(303, '11', 'Jean-Paul Jérome', 'Composition moderne', 400, 600, 250, 350, 650, false, true, 100, 105, 100, 7, 5, NOW() - INTERVAL '35 days', NOW() - INTERVAL '30 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days'),
(304, '12', 'Claude Dulude', 'Étude de couleurs', 300, 500, 180, 280, 520, false, true, 100, 100, 100, 12, 9, NOW() - INTERVAL '35 days', NOW() - INTERVAL '30 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '15 days');

-- 7. LOTS ENCAN FUTUR 1 (235) - Pas de mises car pas commencé
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
VALUES
(400, '1', 'Denis Juneau', '1979/Envolée', 750, 1250, 200, 500, 0, false, true, 100, 103, 100, 20, 26, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(401, '2', 'Serge Lemoyne', '1996/Hommage à Matis', 1400, 1800, 600, 1000, 0, false, true, 100, 101, 100, 12, 12, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(402, '3', 'Marc Siméon', 'Pointe bleue', 400, 600, 120, 300, 0, false, true, 100, 100, 100, 40, 48, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(403, '4', 'Jean Gaudreau', '1996/Visage', 400, 600, 150, 300, 0, false, true, 100, 100, 100, 28, 40, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(404, '5', 'Jean-Paul Jérome', '1996/Abstraction', 600, 800, 350, 500, 0, false, true, 100, 105, 100, 7, 5, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(405, '6', 'Jean-Paul Riopelle', 'Jazz', 5000, 7000, 3500, 4500, 0, false, true, 101, 102, 100, 41, 29, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(406, '7', 'Riopelle', '1981/Les oies 1', 2500, 3500, 1750, 2000, 0, false, true, 101, 102, 100, 28, 23, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days'),
(407, '8', 'Jean-Paul Riopelle', '1985-89/où un ours est chassé debout', 2000, 3000, 1250, 1800, 0, false, true, 101, 102, 100, 18, 25, NOW(), NOW(), NOW() + INTERVAL '16 days', NOW() + INTERVAL '28 days');

-- 8. LOTS ENCAN FUTUR 2 (236) - Pas de mises car pas commencé
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
VALUES
(500, '13', 'Guy Paquet', 'Scène de rue moderne', 800, 1200, 450, 650, 0, false, true, 100, 100, 100, 18, 24, NOW(), NOW(), NOW() + INTERVAL '32 days', NOW() + INTERVAL '45 days'),
(501, '14', 'Vladimir Horik', 'Coucher de soleil', 1200, 1800, 700, 1000, 0, false, true, 100, 100, 100, 20, 30, NOW(), NOW(), NOW() + INTERVAL '32 days', NOW() + INTERVAL '45 days'),
(502, '15', 'Normand Hudon', 'La famille réunie', 2000, 3000, 1200, 1800, 0, false, true, 100, 101, 100, 16, 20, NOW(), NOW(), NOW() + INTERVAL '32 days', NOW() + INTERVAL '45 days'),
(503, '16', 'Claude Langevin', 'Forêt enchantée', 1500, 2200, 900, 1300, 0, false, true, 100, 100, 100, 24, 30, NOW(), NOW(), NOW() + INTERVAL '32 days', NOW() + INTERVAL '45 days'),
(504, '17', 'Christian Bergeron', 'Printemps éternel', 600, 900, 350, 500, 0, false, false, 100, 100, 100, 28, 20, NOW(), NOW(), NOW() + INTERVAL '32 days', NOW() + INTERVAL '45 days'),
(505, '18', 'Bruno Côté', 'Rivière en cascade', 3000, 4500, 1800, 2500, 0, false, false, 100, 101, 100, 30, 36, NOW(), NOW(), NOW() + INTERVAL '32 days', NOW() + INTERVAL '45 days');

-- 9. ASSOCIATIONS ENCANS-LOTS
INSERT INTO encan_lots ("IdEncan", "IdLot")
VALUES
-- Encan présent (100) - Encan234
(100, 100), (100, 101), (100, 102), (100, 103), (100, 104), (100, 105), (100, 106), (100, 107), (100, 108),
-- Encan passé 1 (101) - Encan232
(101, 200), (101, 201), (101, 202), (101, 203), (101, 204), (101, 205), (101, 206), (101, 207), (101, 208),
-- Encan passé 2 (102) - Encan233
(102, 300), (102, 301), (102, 302), (102, 303), (102, 304),
-- Encan futur 1 (103) - Encan235
(103, 400), (103, 401), (103, 402), (103, 403), (103, 404), (103, 405), (103, 406), (103, 407),
-- Encan futur 2 (104) - Encan236
(104, 500), (104, 501), (104, 502), (104, 503), (104, 504), (104, 505);

-- 10. PHOTOS AVEC URLS PLACEHOLDER RÉALISTES
INSERT INTO photos ("Id", "IdLot", "Lien")
VALUES
-- Photos Encan présent (234)
(100, 100, 'https://via.placeholder.com/800x600/FF6B6B/FFFFFF?text=Leon+Bellefleur+Lithographie'),
(101, 100, 'https://via.placeholder.com/800x600/FF6B6B/000000?text=Deux+Personnages'),
(102, 101, 'https://via.placeholder.com/800x600/4ECDC4/FFFFFF?text=Claude+Dulude+Abstraction'),
(103, 102, 'https://via.placeholder.com/800x600/45B7D1/FFFFFF?text=Guy+Paquet+Marcheur'),
(104, 103, 'https://via.placeholder.com/800x600/96CEB4/000000?text=Paul+Tex+Lecor+Solitude'),
(105, 104, 'https://via.placeholder.com/800x600/FFEAA7/000000?text=Paul+Tex+Campagne'),
(106, 105, 'https://via.placeholder.com/800x600/DDA0DD/000000?text=Paul+Tex+Ormes'),
(107, 106, 'https://via.placeholder.com/800x600/98D8C8/000000?text=Christian+Bergeron+Automne'),
(108, 107, 'https://via.placeholder.com/800x600/F7DC6F/000000?text=Bruno+Cote+Malbaie'),
(109, 108, 'https://via.placeholder.com/800x600/BB8FCE/FFFFFF?text=Serge+Lemoyne+Road+Runner'),

-- Photos Encan passé 1 (232)
(200, 200, 'https://via.placeholder.com/800x600/E74C3C/FFFFFF?text=Guy+Paquet+1984'),
(201, 201, 'https://via.placeholder.com/800x600/3498DB/FFFFFF?text=Vladimir+Horik+Messe'),
(202, 202, 'https://via.placeholder.com/800x600/2ECC71/FFFFFF?text=Vladimir+Horik+Paysage'),
(203, 203, 'https://via.placeholder.com/800x600/F39C12/FFFFFF?text=Marcel+Poirier+Automne'),
(204, 204, 'https://via.placeholder.com/800x600/9B59B6/FFFFFF?text=Normand+Hudon+Bonhomme'),
(205, 205, 'https://via.placeholder.com/800x600/1ABC9C/FFFFFF?text=Normand+Hudon+Vent'),
(206, 206, 'https://via.placeholder.com/800x600/E67E22/FFFFFF?text=Normand+Hudon+Saint+Esprit'),
(207, 207, 'https://via.placeholder.com/800x600/34495E/FFFFFF?text=Claude+Langevin+Sous+Bois'),
(208, 208, 'https://via.placeholder.com/800x600/95A5A6/000000?text=Raymond+Girard+Charlevoix'),

-- Photos Encan passé 2 (233)
(300, 300, 'https://via.placeholder.com/800x600/8E44AD/FFFFFF?text=Jean+Gaudreau+Portrait'),
(301, 301, 'https://via.placeholder.com/800x600/2980B9/FFFFFF?text=Marc+Simeon+Abstraction'),
(302, 302, 'https://via.placeholder.com/800x600/27AE60/FFFFFF?text=Denis+Juneau+Hiver'),
(303, 303, 'https://via.placeholder.com/800x600/D35400/FFFFFF?text=Jean+Paul+Jerome+Moderne'),
(304, 304, 'https://via.placeholder.com/800x600/C0392B/FFFFFF?text=Claude+Dulude+Couleurs'),

-- Photos Encan futur 1 (235)
(400, 400, 'https://via.placeholder.com/800x600/16A085/FFFFFF?text=Denis+Juneau+Envolee'),
(401, 401, 'https://via.placeholder.com/800x600/8E44AD/FFFFFF?text=Serge+Lemoyne+Matis'),
(402, 402, 'https://via.placeholder.com/800x600/2980B9/FFFFFF?text=Marc+Simeon+Pointe+Bleue'),
(403, 403, 'https://via.placeholder.com/800x600/E74C3C/FFFFFF?text=Jean+Gaudreau+Visage'),
(404, 404, 'https://via.placeholder.com/800x600/F39C12/FFFFFF?text=Jean+Paul+Jerome+Abstraction'),
(405, 405, 'https://via.placeholder.com/800x600/1ABC9C/000000?text=Jean+Paul+Riopelle+Jazz'),
(406, 406, 'https://via.placeholder.com/800x600/9B59B6/FFFFFF?text=Riopelle+Les+Oies'),
(407, 407, 'https://via.placeholder.com/800x600/E67E22/FFFFFF?text=Riopelle+Ours+Chasse'),

-- Photos Encan futur 2 (236)
(500, 500, 'https://via.placeholder.com/800x600/34495E/FFFFFF?text=Guy+Paquet+Scene+Rue'),
(501, 501, 'https://via.placeholder.com/800x600/E74C3C/FFFFFF?text=Vladimir+Horik+Coucher+Soleil'),
(502, 502, 'https://via.placeholder.com/800x600/2ECC71/FFFFFF?text=Normand+Hudon+Famille'),
(503, 503, 'https://via.placeholder.com/800x600/3498DB/FFFFFF?text=Claude+Langevin+Foret'),
(504, 504, 'https://via.placeholder.com/800x600/F39C12/FFFFFF?text=Christian+Bergeron+Printemps'),
(505, 505, 'https://via.placeholder.com/800x600/9B59B6/FFFFFF?text=Bruno+Cote+Riviere+Cascade');

-- 11. VÉRIFICATIONS FINALES
SELECT 'ENCANS TOTAL' as type, COUNT(*) as total FROM encans;
SELECT 'LOTS TOTAL' as type, COUNT(*) as total FROM lots;
SELECT 'PHOTOS TOTAL' as type, COUNT(*) as total FROM photos;
SELECT 'ASSOCIATIONS TOTAL' as type, COUNT(*) as total FROM encan_lots;

SELECT 'ENCANS EN COURS' as type, COUNT(*) as total
FROM encans e
WHERE e."DateDebut" <= NOW() AND e."DateFin" >= NOW() AND e."EstPublie" = true;

SELECT 'ENCANS PASSÉS NON TERMINÉS AVEC MISES' as type, COUNT(*) as total
FROM encans e
WHERE NOT e."EstTermine" AND e."DateFin" < NOW() AND EXISTS (
    SELECT 1 FROM encan_lots el
    INNER JOIN lots l ON el."IdLot" = l."Id"
    WHERE el."IdEncan" = e."Id" AND l."Mise" > 0.0
);

SELECT 'LOTS AVEC MISES > 0' as type, COUNT(*) as total
FROM lots WHERE "Mise" > 0;

-- 12. RÉSUMÉ PAR ENCAN
SELECT
    e."NumeroEncan" as "Encan",
    e."DateDebut"::date as "Début",
    e."DateFin"::date as "Fin",
    CASE
        WHEN e."DateDebut" <= NOW() AND e."DateFin" >= NOW() THEN 'EN_COURS'
        WHEN e."DateFin" < NOW() THEN 'PASSÉ'
        ELSE 'FUTUR'
    END as "Statut",
    COUNT(l."Id") as "Lots",
    SUM(CASE WHEN l."Mise" > 0 THEN 1 ELSE 0 END) as "Avec_Mises",
    STRING_AGG(DISTINCT l."Artiste", ', ') as "Artistes"
FROM encans e
LEFT JOIN encan_lots el ON e."Id" = el."IdEncan"
LEFT JOIN lots l ON el."IdLot" = l."Id"
GROUP BY e."Id", e."NumeroEncan", e."DateDebut", e."DateFin", e."EstTermine"
ORDER BY e."NumeroEncan";