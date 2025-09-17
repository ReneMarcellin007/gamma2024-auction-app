-- ===== SCRIPT SIMPLE: JUSTE METTRE À JOUR LES MOTS DE PASSE =====
-- Utilisez ce script si les utilisateurs existent déjà

-- 1. CLIENT: Mettre à jour le mot de passe pour "MotDePasseClient123!"
UPDATE "AspNetUsers"
SET "PasswordHash" = 'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
    "EmailConfirmed" = true
WHERE "Email" = 'client@example.com';

-- 2. ADMIN: Mettre à jour le mot de passe pour "MotDePasseAdmin123!"
UPDATE "AspNetUsers"
SET "PasswordHash" = 'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
    "EmailConfirmed" = true
WHERE "Email" = 'admin@example.com';

-- 3. VÉRIFICATION
SELECT '✅ MOTS DE PASSE MIS À JOUR!' as status;
SELECT 'client@example.com' as email, 'MotDePasseClient123!' as password
UNION ALL
SELECT 'admin@example.com', 'MotDePasseAdmin123!';

-- 4. Vérifier que les UPDATE ont fonctionné
SELECT "Email", "EmailConfirmed",
       CASE WHEN LENGTH("PasswordHash") > 0 THEN 'Hash présent' ELSE 'Pas de hash!' END as status
FROM "AspNetUsers"
WHERE "Email" IN ('client@example.com', 'admin@example.com');