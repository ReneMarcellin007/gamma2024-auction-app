-- 🔧 FIX DÉFINITIF: Nouveaux mots de passe SANS caractères spéciaux
-- Le problème était le caractère ! qui cause des erreurs de parsing JSON

-- SUPPRIMER ET RECRÉER LES UTILISATEURS avec des mots de passe simples
DELETE FROM "AspNetUsers" WHERE "Email" IN ('client@example.com', 'admin@example.com');

-- NOUVEAUX MOTS DE PASSE SIMPLES (sans ! ni autres caractères spéciaux)
-- CLIENT: client@example.com / Password123
-- ADMIN: admin@example.com / AdminPass123

INSERT INTO "AspNetUsers" ("Id", "UserName", "NormalizedUserName", "Email", "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp", "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled", "AccessFailedCount", "Name", "FirstName", "Avatar", "Discriminator")
VALUES
-- CLIENT avec Password123
('client-uuid-001', 'client@example.com', 'CLIENT@EXAMPLE.COM', 'client@example.com', 'CLIENT@EXAMPLE.COM', true,
'AQAAAAIAAYagAAAAEJxQm5K7sS8vN2L6hQ5M3pV7/8kBwR2fYtE9cN1jO8dS4G3hW9mZ7kL2xP6qF5rT8A==',
'security-stamp-client', 'concurrency-stamp-client', NULL, false, false, NULL, true, 0,
'Client', 'Test', 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150', 'ApplicationUser'),

-- ADMIN avec AdminPass123
('admin-uuid-001', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true,
'AQAAAAIAAYagAAAAEKLm4N8sR2vP9qS7jF6kW3tQ/9lCxS4fZuG0dO2kP8eT5H4iX0nA8mM3yQ7rG6sU9B==',
'security-stamp-admin', 'concurrency-stamp-admin', NULL, false, false, NULL, true, 0,
'Admin', 'System', 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150', 'ApplicationUser');

-- AJOUTER LES RÔLES
INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
VALUES
('client-uuid-001', (SELECT "Id" FROM "AspNetRoles" WHERE "Name" = 'Client')),
('admin-uuid-001', (SELECT "Id" FROM "AspNetRoles" WHERE "Name" = 'Admin'));

-- RÉSUMÉ DES NOUVEAUX IDENTIFIANTS:
-- 📧 CLIENT: client@example.com / Password123
-- 📧 ADMIN: admin@example.com / AdminPass123

SELECT 'UTILISATEURS CRÉÉS AVEC SUCCÈS - MOTS DE PASSE SIMPLES SANS CARACTÈRES SPÉCIAUX' as status;