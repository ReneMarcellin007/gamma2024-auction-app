using Gamma2024.Server.Data;
using Gamma2024.Server.Models;
using Gamma2024.Server.Services;
using Gamma2024.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;

namespace Gamma2024.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaiementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FactureService _factureService;
        private readonly FactureLivraisonService _factureLivraisonService;
        private readonly ILogger<PaiementController> _logger;

        public PaiementController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, FactureService factureService, FactureLivraisonService factureLivraisonService, ILogger<PaiementController> logger)
        {
            _userManager = userManager;
            _factureService = factureService;
            _factureLivraisonService = factureLivraisonService;
            _logger = logger;
        }

        [Authorize(Roles = "Client")]
        [HttpPost("CreerSetupIntent")]
        public async Task<ActionResult> CreerSetupIntent()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID utilisateur non trouvé");
            }

            var user = await _userManager.Users
                .OfType<ApplicationUser>()
                .Include(u => u.Adresses.Where(a => a.EstDomicile))
                .FirstOrDefaultAsync(u => u.Id == userId);

            var options = new SetupIntentCreateOptions
            {
                Customer = user.StripeCustomer,
                AutomaticPaymentMethods = new SetupIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new SetupIntentService();
            SetupIntent intent = service.Create(options);
            return Ok(new { clientSecret = intent.ClientSecret });
        }

        [Authorize(Roles = "Client")]
        [HttpGet("ChercherCartes")]
        public async Task<ActionResult> ChercherCartes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID utilisateur non trouvé");
            }

            var user = await _userManager.Users
                .OfType<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.Id == userId);

            var service = new PaymentMethodService();
            var options = new PaymentMethodListOptions
            {
                Type = "card",
                Limit = 100,
                Customer = user.StripeCustomer,
            };

            StripeList<PaymentMethod> paymentMethods = service.List(options);
            List<CarteCreditInfoVM> cartes = new();

            foreach (var paymentMethod in paymentMethods.Data)
            {
                cartes.Add(new CarteCreditInfoVM()
                {
                    Dernier4Numero = paymentMethod.Card.Last4,
                    ExpirationDate = paymentMethod.Card.ExpMonth + "/" + paymentMethod.Card.ExpYear,
                    Marque = paymentMethod.Card.Brand,
                    PaymentMethodId = User.IsInRole("Client") ? paymentMethod.Id : null,
                });
            }
            return Ok(cartes);
        }

        [HttpPost("ChargerClients/{numeroEncan}")]
        public async Task<IActionResult> ChargerClients(int numeroEncan)
        {
            _logger.LogInformation($"Début traitement paiements pour encan #{numeroEncan}");
            var factures = _factureService.ChercherFacturesParEncan(numeroEncan);
            var productService = new ProductService();
            var invoiceService = new InvoiceService();
            var invoiceItemService = new InvoiceItemService();
            var paymentMethodService = new PaymentMethodService();

            foreach (var facture in factures)
            {
                if (!facture.estPaye)
                {
                    var customerId = facture.Client.StripeCustomer;
                    var paymentMethodOptions = new PaymentMethodListOptions
                    {
                        Type = "card",
                        Limit = 1,
                        Customer = customerId,
                    };

                    StripeList<PaymentMethod> paymentMethods = paymentMethodService.List(paymentMethodOptions);

                    var invoiceOptions = new InvoiceCreateOptions
                    {
                        Customer = customerId,
                        CollectionMethod = "charge_automatically",
                        DefaultPaymentMethod = paymentMethods.Data[0].Id,
                        AutomaticTax = new InvoiceAutomaticTaxOptions(),
                    };

                    var invoice = invoiceService.Create(invoiceOptions);

                    foreach (var lot in facture.Lots)
                    {
                        var productOptions = new ProductCreateOptions
                        {
                            Name = lot.Description,
                            DefaultPriceData = new ProductDefaultPriceDataOptions
                            {
                                UnitAmount = (long)(lot.Mise.Value * 100),
                                Currency = "cad",
                            },
                            Expand = new List<string> { "default_price" },
                        };
                        var product = productService.Create(productOptions);

                        var invoiceItemOptions = new InvoiceItemCreateOptions
                        {
                            Customer = customerId,
                            Invoice = invoice.Id,
                            Description = lot.Description,
                            Price = product.DefaultPriceId
                        };

                        invoiceItemService.Create(invoiceItemOptions);
                    }

                    var fraisProdOptions = new ProductCreateOptions
                    {
                        Name = "Frais d'encanteur",
                        DefaultPriceData = new ProductDefaultPriceDataOptions
                        {
                            UnitAmount = (long)(facture.FraisEncanteur * 100),
                            Currency = "cad",
                        },
                        Expand = new List<string> { "default_price" },
                    };
                    var fraisProd = productService.Create(fraisProdOptions);

                    var fraisInvoiceItemOptions = new InvoiceItemCreateOptions
                    {
                        Customer = customerId,
                        Invoice = invoice.Id,
                        Description = "Frais d'encanteur",
                        Price = fraisProd.DefaultPriceId
                    };

                    invoiceItemService.Create(fraisInvoiceItemOptions);

                    var tpsProdOptions = new ProductCreateOptions
                    {
                        Name = "TPS",
                        DefaultPriceData = new ProductDefaultPriceDataOptions
                        {
                            UnitAmount = (long)(facture.TPS * 100),
                            Currency = "cad",
                        },
                        Expand = new List<string> { "default_price" },
                    };
                    var tpsProd = productService.Create(tpsProdOptions);

                    var tpsInvoiceItemOptions = new InvoiceItemCreateOptions
                    {
                        Customer = customerId,
                        Invoice = invoice.Id,
                        Description = "TPS",
                        Price = tpsProd.DefaultPriceId
                    };

                    invoiceItemService.Create(tpsInvoiceItemOptions);

                    var tvqProdOptions = new ProductCreateOptions
                    {
                        Name = "TVQ",
                        DefaultPriceData = new ProductDefaultPriceDataOptions
                        {
                            UnitAmount = (long)(facture.TVQ * 100),
                            Currency = "cad",
                        },
                        Expand = new List<string> { "default_price" },
                    };
                    var tvqProd = productService.Create(tvqProdOptions);

                    var tvqInvoiceItemOptions = new InvoiceItemCreateOptions
                    {
                        Customer = customerId,
                        Invoice = invoice.Id,
                        Description = "TVQ",
                        Price = tvqProd.DefaultPriceId
                    };

                    invoiceItemService.Create(tvqInvoiceItemOptions);

                    invoiceService.Pay(invoice.Id);

                    _factureService.PayerFacture(facture.Id);
                }
            }

            _logger.LogInformation($"Fin traitement paiements pour encan #{numeroEncan}");
            return Ok();
        }

        [HttpPost("SupprimerCarte/{idPaymentMethod}")]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult> SupprimerCarte(string idPaymentMethod)
        {
            var service = new PaymentMethodService();

            var carteSupprime = service.Detach(idPaymentMethod);

            if (carteSupprime != null)
            {
                return Ok();
            }

            return BadRequest("Erreur lors de la suppression de la carte");
        }
    }
}
