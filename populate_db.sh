#!/bin/bash

echo "🚀 POPULATION DE LA BASE GAMMA2024 - SCRIPT FINAL"
echo "================================================"

# URL de base
BASE_URL="https://gamma2024-auction-app.onrender.com"

echo "1️⃣ Vérification de l'état initial..."
curl -s "$BASE_URL/api/test/check-db" | python3 -c "
import sys, json
data = json.load(sys.stdin)
print(f'✅ DB Status: Connected={data[\"connected\"]}, Encans={data[\"encans\"]}, Lots={data[\"lots\"]}, Photos={data[\"photos\"]}')
"

echo -e "\n2️⃣ Test de l'endpoint de seeding d'urgence..."
curl -s -X POST "$BASE_URL/api/emergencyseed/seed-now" | python3 -c "
import sys, json
try:
    data = json.load(sys.stdin)
    if 'success' in data:
        print('🎉 SUCCESS:', data['message'])
        print('📊 Data:', data['data'])
    else:
        print('❌ ERROR:', data.get('error', 'Unknown error'))
        if 'innerError' in data:
            print('🔍 Detail:', data['innerError'])
except:
    print('❌ Endpoint failed or returned non-JSON')
"

echo -e "\n3️⃣ Vérification finale..."
curl -s "$BASE_URL/api/test/check-db" | python3 -c "
import sys, json
data = json.load(sys.stdin)
print(f'🏁 Final Status: Encans={data[\"encans\"]}, Lots={data[\"lots\"]}, Photos={data[\"photos\"]}')
if data['encans'] > 0:
    print('🎉 SUCCESS! Database populated!')
    print('🌐 Check your website: $BASE_URL')
else:
    print('❌ Database still empty - manual intervention needed')
"

echo -e "\n✅ Script terminé!"