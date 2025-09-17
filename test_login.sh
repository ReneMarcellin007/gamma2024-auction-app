#!/bin/bash

# Test de connexion pour l'API Gamma2024

echo "===== TEST DE CONNEXION GAMMA2024 ====="
echo ""

# Test 1: Vérifier que l'API répond
echo "1. Test de santé de l'API..."
curl -s https://gamma2024-auction-app.onrender.com/api/health | head -n 1
echo ""

# Test 2: Tester la connexion client
echo "2. Test connexion client@example.com..."
curl -X POST https://gamma2024-auction-app.onrender.com/api/home/login \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "emailOuPseudo": "client@example.com",
    "motDePasse": "MotDePasseClient123!"
  }' \
  -v 2>&1 | grep -E "(< HTTP|message|token|error)"
echo ""

# Test 3: Tester avec mauvais mot de passe
echo "3. Test avec mauvais mot de passe (devrait échouer)..."
curl -X POST https://gamma2024-auction-app.onrender.com/api/home/login \
  -H "Content-Type: application/json" \
  -H "Accept: application/json" \
  -d '{
    "emailOuPseudo": "client@example.com",
    "motDePasse": "MauvaisMotDePasse"
  }' \
  -s | grep -E "(message|error)"
echo ""

echo "===== FIN DES TESTS ====="