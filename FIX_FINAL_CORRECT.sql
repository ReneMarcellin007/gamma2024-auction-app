-- 🔧 FIX FINAL CORRIGÉ: Garder les mots de passe avec ! et corriger la structure SQL

-- Supprimer et recréer les utilisateurs
DELETE FROM "AspNetUsers" WHERE "Email" IN ('client@example.com', 'admin@example.com');

-- MOTS DE PASSE ORIGINAUX MAINTENUS:
-- CLIENT: client@example.com / MotDePasseClient123!
-- ADMIN: admin@example.com / MotDePasseAdmin123!

INSERT INTO "AspNetUsers" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "Name", "FirstName", "Avatar")
VALUES
-- CLIENT avec MotDePasseClient123!
('client-uuid-final', 'client@example.com', 'CLIENT@EXAMPLE.COM', 'client@example.com', 'CLIENT@EXAMPLE.COM', true,
'AQAAAAIAAYagAAAAEBCLhDAVClAVnNnHmZ3ahe6KYsdJa/tTtcmHC64QlZsy07wt7VRMIl+nfrP0UJ8oKw==',
'security-final-client', 'concurrency-final-client', NULL, false, false, NULL, true, 0,
'Client', 'Test', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150'),

-- ADMIN avec MotDePasseAdmin123!
('admin-uuid-final', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true,
'AQAAAAIAAYagAAAAEImrQqIdpN3WKyTx0Ys/9QQXVKT5jTAyfxsPYj6ljA7MwE8U/IWotqFi5RT5o5V7VQ==',
'security-final-admin', 'concurrency-final-admin', NULL, false, false, NULL, true, 0,
'Admin', 'System', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150');

-- Ajouter les rôles
INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES
('client-uuid-final', (SELECT "Id" FROM "AspNetRoles" WHERE "Name" = 'Client')),
('admin-uuid-final', (SELECT "Id" FROM "AspNetRoles" WHERE "Name" = 'Admin'));

-- RÉSUMÉ FINAL:
-- 📧 CLIENT: client@example.com / MotDePasseClient123!
-- 📧 ADMIN: admin@example.com / MotDePasseAdmin123!
-- (AVEC LE ! - on corrigera côté backend)

SELECT 'UTILISATEURS CRÉÉS - MOTS DE PASSE AVEC !' as status;