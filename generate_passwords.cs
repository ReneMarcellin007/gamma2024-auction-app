using Microsoft.AspNetCore.Identity;
using System;

// Script pour générer les hashes des nouveaux mots de passe (sans le caractère !)
public class Program
{
    public static void Main()
    {
        var hasher = new PasswordHasher<object>();

        // Nouveaux mots de passe sans caractères spéciaux problématiques
        string clientPassword = "MotDePasseClient123";  // Sans le !
        string adminPassword = "MotDePasseAdmin123";    // Sans le !

        string clientHash = hasher.HashPassword(null, clientPassword);
        string adminHash = hasher.HashPassword(null, adminPassword);

        Console.WriteLine("=== NOUVEAUX MOTS DE PASSE SANS CARACTÈRES SPÉCIAUX ===");
        Console.WriteLine();
        Console.WriteLine("CLIENT:");
        Console.WriteLine($"Email: client@example.com");
        Console.WriteLine($"Mot de passe: {clientPassword}");
        Console.WriteLine($"Hash: {clientHash}");
        Console.WriteLine();
        Console.WriteLine("ADMIN:");
        Console.WriteLine($"Email: admin@example.com");
        Console.WriteLine($"Mot de passe: {adminPassword}");
        Console.WriteLine($"Hash: {adminHash}");
        Console.WriteLine();
        Console.WriteLine("=== SCRIPT SQL ===");
        Console.WriteLine($"UPDATE \"AspNetUsers\" SET \"PasswordHash\" = '{clientHash}' WHERE \"Email\" = 'client@example.com';");
        Console.WriteLine($"UPDATE \"AspNetUsers\" SET \"PasswordHash\" = '{adminHash}' WHERE \"Email\" = 'admin@example.com';");
    }
}