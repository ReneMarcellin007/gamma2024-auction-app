-- SCRIPT MINIMAL - FONCTIONNE AVEC TOUTE DB POSTGRESQL
-- Utilise les IDs existants ou crée le minimum nécessaire

-- 1. Vérifier ce qui existe déjà
SELECT 'ÉTAT ACTUEL:' as info;
SELECT 'Catégories:' as type, COUNT(*) as total FROM categories;
SELECT 'Médiums:' as type, COUNT(*) as total FROM mediums;
SELECT 'Adresses:' as type, COUNT(*) as total FROM adresses;
SELECT 'Vendeurs:' as type, COUNT(*) as total FROM vendeurs;

-- 2. Utiliser les premiers IDs disponibles OU créer le minimum
DO $$
DECLARE
    cat_id INTEGER;
    med_id INTEGER;
    addr_id INTEGER;
    vend_id INTEGER;
BEGIN
    -- Prendre la première catégorie OU créer une nouvelle
    SELECT "Id" INTO cat_id FROM categories LIMIT 1;
    IF cat_id IS NULL THEN
        INSERT INTO categories ("Nom") VALUES ('Art') RETURNING "Id" INTO cat_id;
    END IF;

    -- Prendre le premier médium OU créer un nouveau
    SELECT "Id" INTO med_id FROM mediums LIMIT 1;
    IF med_id IS NULL THEN
        INSERT INTO mediums ("Type") VALUES ('Peinture') RETURNING "Id" INTO med_id;
    END IF;

    -- Prendre la première adresse OU créer une nouvelle
    SELECT "Id" INTO addr_id FROM adresses LIMIT 1;
    IF addr_id IS NULL THEN
        INSERT INTO adresses ("Numero", "Rue", "Ville", "Province", "Pays", "CodePostal", "EstDomicile")
        VALUES (1, 'Test St', 'Montreal', 'QC', 'Canada', 'H1H1H1', false) RETURNING "Id" INTO addr_id;
    END IF;

    -- Prendre le premier vendeur OU créer un nouveau
    SELECT "Id" INTO vend_id FROM vendeurs LIMIT 1;
    IF vend_id IS NULL THEN
        INSERT INTO vendeurs ("Nom", "Prenom", "Courriel", "Telephone", "AdresseId")
        VALUES ('Test', 'Vendeur', 'test@test.com', '555-0001', addr_id) RETURNING "Id" INTO vend_id;
    END IF;

    -- Nettoyer les données existantes
    DELETE FROM photos WHERE "IdLot" IN (SELECT "Id" FROM lots WHERE "Id" >= 200);
    DELETE FROM encan_lots WHERE "IdLot" IN (SELECT "Id" FROM lots WHERE "Id" >= 200);
    DELETE FROM lots WHERE "Id" >= 200;
    DELETE FROM encans WHERE "Id" >= 200;

    -- Créer 1 ENCAN SIMPLE
    INSERT INTO encans ("Id", "NumeroEncan", "DateDebut", "DateFin", "DateDebutSoireeCloture", "EstPublie", "EstTermine", "PasLot", "PasMise")
    VALUES (200, 999, NOW() - INTERVAL '2 days', NOW() + INTERVAL '7 days', NOW() + INTERVAL '6 days', true, false, 1, 10);

    -- Créer 3 LOTS SIMPLES
    INSERT INTO lots ("Id", "Numero", "Artiste", "Description", "ValeurEstimeMin", "ValeurEstimeMax", "PrixOuverture", "PrixMinPourVente", "Mise", "EstVendu", "EstLivrable", "IdCategorie", "IdMedium", "IdVendeur", "Hauteur", "Largeur", "DateCreation", "DateDepot", "DateDebutDecompteLot", "DateFinDecompteLot")
    VALUES
    (200, 'PICASSO-01', 'Pablo Picasso', 'Les Demoiselles d''Avignon', 50000, 80000, 30000, 40000, 35000, false, true, cat_id, med_id, vend_id, 243, 233, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
    (201, 'MONET-01', 'Claude Monet', 'Nymphéas', 30000, 50000, 20000, 25000, 22000, false, true, cat_id, med_id, vend_id, 200, 425, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days'),
    (202, 'VANGOGH-01', 'Vincent van Gogh', 'La Nuit étoilée', 40000, 60000, 25000, 30000, 0, false, true, cat_id, med_id, vend_id, 73, 92, NOW(), NOW(), NOW() + INTERVAL '5 days', NOW() + INTERVAL '7 days');

    -- Associer les lots à l'encan
    INSERT INTO encan_lots ("IdEncan", "IdLot") VALUES
    (200, 200), (200, 201), (200, 202);

    -- Créer les photos
    INSERT INTO photos ("IdLot", "Lien") VALUES
    (200, 'https://via.placeholder.com/800x600/FF4444/FFFFFF?text=Picasso+Demoiselles'),
    (201, 'https://via.placeholder.com/800x600/4444FF/FFFFFF?text=Monet+Nympheas'),
    (202, 'https://via.placeholder.com/800x600/44FF44/000000?text=Van+Gogh+Nuit');

    RAISE NOTICE 'SUCCESS: Created 1 auction with 3 lots (Picasso, Monet, Van Gogh)';
END $$;

-- 3. Vérification finale
SELECT 'RÉSULTAT FINAL:' as info;
SELECT 'Encans:' as type, COUNT(*) as total FROM encans;
SELECT 'Lots:' as type, COUNT(*) as total FROM lots;
SELECT 'Photos:' as type, COUNT(*) as total FROM photos;
SELECT 'Associations:' as type, COUNT(*) as total FROM encan_lots;

-- 4. Détails de l'encan créé
SELECT
    e."NumeroEncan" as "Encan",
    COUNT(l."Id") as "Lots",
    STRING_AGG(l."Artiste", ', ') as "Artistes"
FROM encans e
LEFT JOIN encan_lots el ON e."Id" = el."IdEncan"
LEFT JOIN lots l ON el."IdLot" = l."Id"
WHERE e."Id" = 200
GROUP BY e."Id", e."NumeroEncan";