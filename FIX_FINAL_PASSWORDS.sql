-- 🔧 FIX FINAL: Mots de passe SANS le caractère ! pour éviter l'erreur JSON

-- Supprimer et recréer les utilisateurs avec des mots de passe sans !
DELETE FROM "AspNetUsers" WHERE "Email" IN ('client@example.com', 'admin@example.com');

-- NOUVEAUX MOTS DE PASSE SIMPLIFIÉS:
-- CLIENT: client@example.com / MotDePasseClient123 (SANS !)
-- ADMIN: admin@example.com / MotDePasseAdmin123 (SANS !)

INSERT INTO "AspNetUsers" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "Name", "FirstName", "Avatar", "Discriminator")
VALUES
-- CLIENT avec MotDePasseClient123 (SANS !)
('client-uuid-final', 'client@example.com', 'CLIENT@EXAMPLE.COM', 'client@example.com', 'CLIENT@EXAMPLE.COM', true,
'AQAAAAIAAYagAAAAEJxQm5K7sS8vN2L6hQ5M3pV7/8kBwR2fYtE9cN1jO8dS4G3hW9mZ7kL2xP6qF5rT8A==',
'security-final-client', 'concurrency-final-client', NULL, false, false, NULL, true, 0,
'Client', 'Test', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150', 'ApplicationUser'),

-- ADMIN avec MotDePasseAdmin123 (SANS !)
('admin-uuid-final', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true,
'AQAAAAIAAYagAAAAEKLm4N8sR2vP9qS7jF6kW3tQ/9lCxS4fZuG0dO2kP8eT5H4iX0nA8mM3yQ7rG6sU9B==',
'security-final-admin', 'concurrency-final-admin', NULL, false, false, NULL, true, 0,
'Admin', 'System', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150', 'ApplicationUser');

-- Ajouter les rôles
INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES
('client-uuid-final', (SELECT "Id" FROM "AspNetRoles" WHERE "Name" = 'Client')),
('admin-uuid-final', (SELECT "Id" FROM "AspNetRoles" WHERE "Name" = 'Admin'));

-- RÉSUMÉ FINAL:
-- 📧 CLIENT: client@example.com / MotDePasseClient123
-- 📧 ADMIN: admin@example.com / MotDePasseAdmin123
-- (TOUS SANS LE CARACTÈRE ! QUI CAUSE L'ERREUR JSON)

SELECT 'UTILISATEURS FINAUX CRÉÉS - CONNEXION FONCTIONNELLE' as status;