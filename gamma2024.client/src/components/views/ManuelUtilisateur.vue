<template>
    <div class="d-flex flex-column align-items-center pb-3 w-100">
        <div class="d-flex flex-column align-items-center w-100 col-md-6 fs-5">
            <div class="d-flex flex-column align-items-center col-6 col-sm-9 pb-4">
                <h1>Manuel de l'utilisateur</h1>

                <p class="justifyText">
                    Cette page est dédiée aux utilisateurs qui souhaitent participer aux enchères. Elle offre un mode d'emploi pour miser, faire des surenchères et participer à la soirée de clôture.
                </p>
            </div>

            <div class="w-100 px-5">
                <div class="accordion" id="accordionCommentMiser">
                    <div class="accordion-item">
                        <h2 class="accordion-header">
                            <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseCommentMiser" aria-expanded="true" aria-controls="collapseCommentMiser">
                                <h2>Comment faire des mises</h2>
                            </button>
                        </h2>
                        <div id="collapseCommentMiser" class="accordion-collapse collapse" data-bs-parent="#accordionCommentMiser">
                            <div class="accordion-body">
                                Les étapes des mises suivantes sont disponibles en tout temps lorsqu'il y a un encan en cours et vous avez accès aux mêmes options lors de la soirée de clôture.
                                <div class="w-100 mt-4">
                                    <ol class="list list-group-numbered fs-5 justifyText">
                                        <li class="list-group-item">
                                            Pour faire une mise simple, tout d'abord, vous devez être connecté. Par la suite, vous devez cliquer sur le bouton Miser. Une fenêtre contextuelle va apparaître. Dans celle-ci, vous pouvez faire une enchère en cliquant sur le bouton Miser. Cette mise a une valeur prédéterminée, car il faut respecter les pas de l'enchère. Le pas de l'enchère est le prix minimum pour une nouvelle enchère. Veuillez vous référer à la page des Conditions de la compagnie sous la Majoration de l'enchère pour la valeur du pas selon la valeur du lot.
                                            <div class="w-100 d-flex justify-content-center">
                                                <div class="d-flex flex-column flex-md-row col-lg-5 col-sm-12 justify-content-center">
                                                    <img src="/images/ImageManuelUtilisateur/CliquerSurMiser.png" class="img-fluid" alt="Image qui affiche un lot avec une souris qui clique sur le bouton Miser" />

                                                    <img src="/images/ImageManuelUtilisateur/MiseReguliere.png" class="img-fluid" alt="Image qui affiche une fenêtre contextuelle pour les mises simples" />
                                                </div>
                                            </div>
                                        </li>
                                        <li class="list-group-item">
                                            Lorsque vous voyez que le lot est entouré d'une couleur verte, cela veut dire que vous avez la plus haute mise. De plus, vous allez voir une notification de succès s'afficher en haut à droite de la page.
                                            <div class="w-100 d-flex justify-content-center">
                                                <div class="d-flex col-lg-5 col-sm-9 justify-content-center">
                                                    <img src="/images/ImageManuelUtilisateur/MiseReguliereEffectue.png" class="img-fluid" alt="Image qui affiche un lot dont un membre a misé dessus" />
                                                </div>
                                            </div>
                                        </li>
                                        <li class="list-group-item">
                                            Lorsque vous voyez que le lot est entouré d'une couleur rouge, cela veut dire que votre mise a été dépassée. De plus, vous allez voir une notification s'afficher en haut à droite de la page où se trouve une petite cloche pour vous aviser que quelqu'un vous a dépassé sur le lot dont vous avez misé.
                                            <div class="w-100 d-flex justify-content-center">
                                                <div class="d-flex col-lg-5 col-sm-9 justify-content-center">
                                                    <img src="/images/ImageManuelUtilisateur/MiseReguliereDepasse.png" class="img-fluid" alt="Image qui affiche un lot dont un membre a misé dessus et qui a été dépassé" />
                                                </div>
                                            </div>
                                        </li>
                                        <li class="list-group-item">
                                            Vous avez la possibilité de préparer une mise automatique. Une mise automatique se déclenche lorsqu'un autre membre mise sur un lot dont vous avez placé une enchère. Le système va miser de manière indépendante jusqu'au maximum que vous avez sélectionné. Vous allez voir que le lot sera toujours entouré de vert. Lorsqu'il aura atteint la valeur maximum dont vous avez défini, il va s'arrêter automatiquement et le lot sera entouré de rouge. Si vous souhaitez continuer à participer à l'enchère du lot en question, il vous faudra soit faire une mise simple ou préparer une autre mise automatique.
                                            <div class="w-100 d-flex justify-content-center">
                                                <div class="d-flex col-sm-12 justify-content-center">
                                                    <img src="/images/ImageManuelUtilisateur/MiseAutomatique.png" class="img-fluid" alt="Image qui affiche une fenêtre contextuelle pour les mises automatiques" />
                                                </div>
                                            </div>
                                        </li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="w-100 px-5 pt-4">
                <div class="accordion" id="accordionSoireeCloture">
                    <div class="accordion-item">
                        <h2 class="accordion-header">
                            <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSoireeCloture" aria-expanded="true" aria-controls="collapseSoireeCloture">
                                <h2>Soirée de clôture</h2>
                            </button>
                        </h2>
                        <div id="collapseSoireeCloture" class="accordion-collapse collapse" data-bs-parent="#accordionSoireeCloture">
                            <div class="accordion-body justifyText">
                                Les étapes des mises expliquées dans la partie Comment faire des mises sont également disponibles dans la soirée de clôture. Cependant, votre temps est limité pour obtenir les lots que vous désirez. Une fois le temps accordé pour miser est écoulé, le lot n'est plus disponible.
                                <br />
                                <br />
                                Du moment qu'il y a une surenchère, le système va ajouter une minute sur le lot si le temps restant était de 60 secondes et moins pour permettre la surenchère par les autres participants. Une fois arrivé à 0 seconde, le lot est attribué à la dernière mise si celui est plus haute que la valeur minimale pour la vente.
                                <br />
                                <br />
                                Lors de la fin de la soirée de clôture, qui est déterminée par le temps restant sur le dernier lot qui reste, tous les membres qui auront gagné un ou plusieurs lots seront facturés immédiatement. Il se pourrait qu'il y ait un délai avant que vous puissiez consulter votre facture dans la page Historique des achats. Si l'un des lots n'est pas livrable, vous n'aurez pas le choix de livraison. Vous devrez automatiquement venir le chercher sur place. Cependant, si tous les lots sont livrables, vous aurez à faire votre choix de livraison également dans la page Historique des achats.
                                <div class="w-100 d-flex justify-content-center">
                                    <div class="d-flex flex-column col-lg-8 col-sm-12 justify-content-center">
                                        <img src="/images/ImageManuelUtilisateur/SoireeCloture.png" class="img-fluid" alt="Image qui affiche un lot avec une souris qui clique sur le bouton miser" />

                                        <img src="/images/ImageManuelUtilisateur/SoireeClotureListe.png" class="img-fluid" alt="Image qui affiche un lot avec une souris qui clique sur le bouton miser" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</template>

<script setup>

</script>

<style scoped>
    .accordion-button {
        background-color: transparent;
    }
</style>