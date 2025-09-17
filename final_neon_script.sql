-- ===== SCRIPT FINAL POUR NEON - DONNÉES CONFORMES AUX REQUÊTES APP =====

-- 1. NETTOYER COMPLÈTEMENT
DELETE FROM photos;
DELETE FROM encan_lots;
DELETE FROM lots;
DELETE FROM encans;
DELETE FROM vendeurs;
DELETE FROM adresses;
DELETE FROM categories WHERE "Id" >= 50;
DELETE FROM mediums WHERE "Id" >= 50;

-- 2. CRÉER LES DONNÉES DE BASE
INSERT INTO adresses ("Id", "Numero", "Rue", "Ville", "Province", "Pays", "CodePostal", "EstDomicile")
VALUES (50, 123, 'Test St', 'Montreal', 'Quebec', 'Canada', 'H1H1H1', false);

INSERT INTO vendeurs ("Id", "Nom", "Prenom", "Courriel", "Telephone", "AdresseId")
VALUES (50, 'Vendeur', 'Test', 'test@test.com', '555-0001', 50);

INSERT INTO categories ("Id", "Nom") VALUES
(50, 'Peinture'),
(51, 'Sculpture'),
(52, 'Photographie');

INSERT INTO mediums ("Id", "Type") VALUES
(50, 'Huile sur toile'),
(51, 'Acrylique'),
(52, 'Bronze');

-- 3. CRÉER ENCANS AVEC DATES CORRECTES
-- Encan en cours (commence il y a 5 jours, finit dans 10 jours)
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (50, 1, NOW() - INTERVAL '5 days', NOW() + INTERVAL '10 days', NOW() + INTERVAL '9 days', true, false, 1, 10);

-- Encan passé récent (fini il y a 3 jours, MAIS non terminé pour tests)
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (51, 2, NOW() - INTERVAL '20 days', NOW() - INTERVAL '3 days', NOW() - INTERVAL '4 days', true, false, 1, 10);

-- Encan futur
INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
VALUES (52, 3, NOW() + INTERVAL '15 days', NOW() + INTERVAL '30 days', NOW() + INTERVAL '29 days', true, false, 1, 10);

-- 4. CRÉER LOTS AVEC MISES SUPÉRIEURES À ZÉRO (CRITICAL!)
INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
VALUES
-- ENCAN EN COURS (Id=50) - AVEC MISES > 0
(50, 'PICASSO-01', 'Pablo Picasso', 'Les Demoiselles d''Avignon', 50000, 80000, 30000, 40000, 45000, false, true, 50, 50, 50, 243, 233, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
(51, 'MONET-01', 'Claude Monet', 'Nymphéas', 30000, 50000, 20000, 25000, 28000, false, true, 50, 50, 50, 200, 425, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
(52, 'VANGOGH-01', 'Vincent van Gogh', 'La Nuit étoilée', 40000, 60000, 25000, 30000, 32000, false, true, 50, 50, 50, 73, 92, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),
(53, 'DAVINCI-01', 'Leonardo da Vinci', 'Mona Lisa Copie', 60000, 100000, 40000, 50000, 55000, false, true, 50, 50, 50, 77, 53, NOW(), NOW(), NOW() + INTERVAL '8 days', NOW() + INTERVAL '10 days'),

-- ENCAN PASSÉ (Id=51) - AVEC MISES > 0 pour déclencher la requête
(54, 'RODIN-01', 'Auguste Rodin', 'Le Penseur', 15000, 25000, 10000, 12000, 18000, false, true, 51, 52, 50, 180, 98, NOW() - INTERVAL '25 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '10 days', NOW() - INTERVAL '3 days'),
(55, 'MICHELANGELO-01', 'Michelangelo', 'David Réplique', 25000, 40000, 18000, 20000, 30000, false, true, 51, 52, 50, 517, 199, NOW() - INTERVAL '25 days', NOW() - INTERVAL '20 days', NOW() - INTERVAL '10 days', NOW() - INTERVAL '3 days'),

-- ENCAN FUTUR (Id=52) - Mises à zéro car pas encore commencé
(56, 'DALI-01', 'Salvador Dalí', 'Persistance Mémoire', 35000, 55000, 25000, 30000, 0, false, true, 50, 50, 50, 24, 33, NOW(), NOW(), NOW() + INTERVAL '18 days', NOW() + INTERVAL '30 days'),
(57, 'POLLOCK-01', 'Jackson Pollock', 'No. 1 Dripping', 45000, 70000, 35000, 40000, 0, false, true, 50, 51, 50, 200, 300, NOW(), NOW(), NOW() + INTERVAL '18 days', NOW() + INTERVAL '30 days');

-- 5. ASSOCIER LOTS AUX ENCANS
INSERT INTO encan_lots ("IdEncan", "IdLot")
VALUES
-- Encan en cours
(50, 50), (50, 51), (50, 52), (50, 53),
-- Encan passé
(51, 54), (51, 55),
-- Encan futur
(52, 56), (52, 57);

-- 6. CRÉER PHOTOS
INSERT INTO photos ("Id", "IdLot", "Lien")
VALUES
(50, 50, 'https://via.placeholder.com/800x600/FF0000/FFFFFF?text=Picasso+Demoiselles'),
(51, 51, 'https://via.placeholder.com/800x600/0000FF/FFFFFF?text=Monet+Nympheas'),
(52, 52, 'https://via.placeholder.com/800x600/00FF00/000000?text=Van+Gogh+Nuit'),
(53, 53, 'https://via.placeholder.com/800x600/FFD700/000000?text=Da+Vinci+Mona'),
(54, 54, 'https://via.placeholder.com/800x600/8B4513/FFFFFF?text=Rodin+Penseur'),
(55, 55, 'https://via.placeholder.com/800x600/FFFFFF/000000?text=Michelangelo+David'),
(56, 56, 'https://via.placeholder.com/800x600/FF69B4/FFFFFF?text=Dali+Persistance'),
(57, 57, 'https://via.placeholder.com/800x600/800080/FFFFFF?text=Pollock+No1');

-- 7. VÉRIFICATION CRITIQUE - SIMULER LES REQUÊTES DE L'APP
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

SELECT 'TOTAL ASSOCIATIONS' as type, COUNT(*) as total FROM encan_lots;

-- 8. AFFICHAGE FINAL DES DONNÉES
SELECT
    e."NumeroEncan" as "Encan",
    e."DateDebut"::date as "Début",
    e."DateFin"::date as "Fin",
    e."EstTermine" as "Terminé",
    COUNT(l."Id") as "Lots",
    SUM(CASE WHEN l."Mise" > 0 THEN 1 ELSE 0 END) as "Avec Mises",
    STRING_AGG(l."Artiste", ', ') as "Artistes"
FROM encans e
LEFT JOIN encan_lots el ON e."Id" = el."IdEncan"
LEFT JOIN lots l ON el."IdLot" = l."Id"
GROUP BY e."Id", e."NumeroEncan", e."DateDebut", e."DateFin", e."EstTermine"
ORDER BY e."NumeroEncan";