<template>
    <div class="d-flex flex-column align-items-center pb-3">
        <div class="d-flex flex-column align-items-center w-75 fs-5">
            <h1 class="pb-4">Conditions de la compagnie</h1>

            <div class="w-100">
                <div class="accordion" id="accordionMajoration">
                    <div class="accordion-item">
                        <h2 class="accordion-header p-0">
                            <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseMajoration" aria-expanded="true" aria-controls="collapseMajoration">
                                <h2 class="ps-0 pe-4">Majoration de l'enchère</h2>
                            </button>
                        </h2>
                        <div id="collapseMajoration" class="accordion-collapse collapse" data-bs-parent="#accordionMajoration">
                            <div class="accordion-body">
                                <table class="table table-striped table-bordered table-sm text-center">
                                    <thead class="table-dark">
                                        <tr>
                                            <th scope="col">Offre (en dollar)</th>
                                            <th scope="col">Pas de l'enchère</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td scope="col">0.00 - 199.00$</td>
                                            <td scope="col">10.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">199.01 - 499.00$</td>
                                            <td scope="col">25.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">499.01 - 999.00$</td>
                                            <td scope="col">50.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">999.01 - 1,999.00$</td>
                                            <td scope="col">100.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">1,999.01 - 4,999.00$</td>
                                            <td scope="col">200.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">4,999.01 - 9,999.00$</td>
                                            <td scope="col">250.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">9,999.01 - 19,999.00$</td>
                                            <td scope="col">500.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">19,999.01 - 49,999.00$</td>
                                            <td scope="col">1,000.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">49,999.01 - 99,999.00$</td>
                                            <td scope="col">2,000.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">99,999.01 - 499,999.00$</td>
                                            <td scope="col">5,000.00$ CAD</td>
                                        </tr>
                                        <tr>
                                            <td scope="col">499,999.01 - 9,999,999.99$</td>
                                            <td scope="col">10,000.00$ CAD</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>


            </div>

            <div class="pt-3 w-100">
                <div class="accordion" id="accordionModalite">
                    <div class="accordion-item">
                        <h2 class="accordion-header p-0">
                            <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseModalite" aria-expanded="true" aria-controls="collapseModalite">
                                <h2 class="ps-0 pe-4">Modalités et conditions</h2>
                            </button>
                        </h2>
                        <div id="collapseModalite" class="accordion-collapse collapse" data-bs-parent="#accordionModalite">
                            <div class="accordion-body">
                                <ol class="list-group list-group-numbered fs-7">
                                    <li class="list-group-item">
                                        Toute personne désirant miser en ligne doit être inscrite au préalable en indiquant les informations requises ainsi que numéro de carte de crédit.
                                    </li>
                                    <li class="list-group-item">
                                        Un frais de 15% et les taxes applicables, des frais supplémentaires s’ajouteront pour l’expédition.
                                    </li>
                                    <li class="list-group-item">
                                        La facturation se fera automatiquement après l'encan et la solvabilité de la carte de crédit de l'acheteur sera nécessaire.
                                    </li>
                                    <li class="list-group-item">
                                        Expédition disponible par la poste pour les colis de grosseur et de poids raisonnable et prix sera chargée en conséquence sur approbation en nous communiquant.
                                    </li>
                                    <li class="list-group-item">
                                        Un délai de 2 semaines sera accordé pour prise de possession de vos achats.
                                    </li>
                                    <li class="list-group-item">
                                        Pour les encans fermées, telles que nous utiliserons jusqu'à nouvelle ordre, elles auront une durée déterminée d’une semaine afin de pouvoir miser.
                                    </li>
                                    <li class="list-group-item">
                                        Chaque enchérisseur recevra une alerte par courriel s'il a été surenchéri.
                                    </li>
                                    <li class="list-group-item">
                                        Possibilité de laisser une mise maximum que le système sécurisé s'occupera pour l'enchérisseur.
                                    </li>
                                    <li class="list-group-item">
                                        La fermeture des lots se fera par intervalle de 30 secondes chacun, sans tenir compte des surenchères possibles à la fin de l'encan de certains lots. Tout acheteur aura alors un délai de 60 secondes entre chaque surenchère pour miser et ceci aura pour effet de repousser le moment de la fermeture de ce lot.
                                    </li>
                                    <li class="list-group-item">
                                        Inspection possible des items sur les heures d'ouverture du commerce. 1 rue Church, Stanstead, Québec.
                                    </li>
                                </ol>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="pt-3 w-100">
                <div class="accordion" id="accordionExample">
                    <div class="accordion-item">
                        <h2 class="accordion-header p-0" @click="expand">
                            <button id="btnAccordionArtiste" class="accordion-button d-flex" type="button" data-bs-toggle="collapse" data-bs-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                <div class="col">
                                    <h2 class="ps-0 pe-4">Artistes</h2>
                                </div>
                                <div class="fs-7" v-show="estOuvert">
                                    Serge Lemoyne, Charles Daudelin, John Little, Jean-Paul Jérome, Yves Trudeau, Yves Gaucher, Claude Tousignant, Jacques Hurtubise, Jean-Paul Lemieux, Jean-Philippe Dallaire, René Richard, Lorne Bouchard, Vasarely, Normand Hudon, Dali, Albini Leblanc, Vladimir Horik, Stefan Horik, Umberto Bruni, Alfred Laliberté, Jean-Paul Riopelle, Roger Cantin, Bruno Côté, Paul Tex Lecor, Frère Jérome, Stanley Cosgrove, Alleyn ...
                                </div>
                            </button>
                        </h2>
                        <div id="collapseOne" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
                            <div class="accordion-body justifyText fs-6">
                                Serge Lemoyne, Charles Daudelin, John Little, Jean-Paul Jérome, Yves Trudeau, Yves Gaucher, Claude Tousignant, Jacques Hurtubise, Jean-Paul Lemieux, Jean-Philippe Dallaire, René Richard, Lorne Bouchard, Vasarely, Normand Hudon, Dali, Albini Leblanc, Vladimir Horik, Stefan Horik, Umberto Bruni, Alfred Laliberté, Jean-Paul Riopelle, Roger Cantin, Bruno Côté, Paul Tex Lecor, Frère Jérome, Stanley Cosgrove, Alleyn, Louis Belzile, Kittie Bruneau, Allen Sapp, Guy Paquet, Fernand Toupin, Jean Gaudreau, Réal Fournier, Fritz Brandtner, Fleurimont et Jean Constantineau, Jean Lefebvre, Lise Gervais, Albert Dumouchel, Pauline T. Paquin, Yves Rajotte, Alfred Pellan, Tom Roberts, Louise Scott, Dzigurski, F.S. Coburn, Claude Dulude, Miyuki Tanobé, Maurice Agnan, Iacurto, Christian Bergeron, Hébert, Poirier, Léo Ayotte, Albert Rousseau, Leclerc, Tatossian, Tremblay, Lacroix, Lebon, Simard, Roland Pichet, Leduc, Pranke, Boisvert, Labelle, Cusan, Gingras, Berne, Gianolla, Barrett, Genest, L'Archevèque, Fecteau, Dufour, Blier, Auger, Provost, Pierre Bédard, Gilles Bédard, David Wong, Rafael Tchetyshov, Mostafa Keyhani, Hanneke VanDer Sluys, Mahin, Angela Colesnicenco, Nemo, David Chen, Mahey, Goulet, Garo, Lapensée, Paule Lagacé, René Gagnon, Garside, Mario Beaudoin, Breton, DeBagossy, Jean-Guy Desrosiers, Jacques Poirier, Stuart, Lemieux, Bluteau, Gécin, Dufour, Henri Masson, Favreau, Raymond Caouette, Dubreuil, Mihaly Von Zichy, Paul Émile Bécat, Viateur Lapierre, Klapschenski, Halpin, Soulikias, Schnobb, Michel Leroux, Morris, Cécile Grondin, Gamache, Bruce Heggtveit, Adrienne Paradis, Robert Deschênes, Jean-Paul Lapointe, Delsignore, Roland Giguère, Stanley Lewis, Jean-Paul Pépin, André Besse, Michèle Pelland, Marc Lincourt, Yves Lahey, Laurent Bonet, Claude Tétu, Georges Hrabe, Néré B. Paul, Sucsan, Gordon Pfeiffer, A.V. Breau, Jenine Leroux-Guillaume, Robert Boucher, Jeanne D'Arc Leclerc, Paul Livernois, R.L. Smiley, Liliane Morgan, Jacques Lisée, Marc Poissant, Vittorio, Susan Hubley, Ron Jamieson, Robert Savoie, Jean Grenier, Funnekotter, Albric Soly, Lise Lacaille, Raynald Piché, Norman Laliberté, Everist Prokofiev, Françoise Pascals, Lucienne Zegray, Raynald Leclerc, Gilles Simard, Stefanoff, Gerometta, Gaston Rebry, Claude Langevin, Bruno Lord, Jean Blanchet, André Dufour, Terry Tomalty, Richard Hétu, Joyce Kellock, Marie Laberge, Graham Norwell, J-P Ladouceur, André Besse, Diane Desmarais, Pierre Tougas, C.Y. Bates, Palmearts, Monique Benoit, Alice Winant, Martin Blanchet, Leblond, Bousquet, Claude Carette, Louise Carrier, Jimmy Perron, Roger Alexandre, Robert Savignac, Claude Bleynie, DeGarthe, Montpetit, Bellefleur, Duveyre, Simpkins, P.V. Beaulieu etc…
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</template>

<script setup>
    import { ref } from 'vue'
    const estOuvert = ref("false")

    const expand = ref(() => {
        const btn = document.querySelector("#btnAccordionArtiste")


        if (btn.classList.contains('collapsed') == true) {
            estOuvert.value = true;
        }
        else {
            estOuvert.value = false;
        }
    });

</script>

<style scoped>
    .list-group {
        --bs-list-group-border-color: none
    }

    .accordion-button {
        background-color: transparent;
    }
</style>