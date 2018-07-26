
namespace DevKit.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/css/bootstrap.css",
                "~/css/bootstrap-theme.css",
                "~/css/bootstrap-additions.min.css",
                "~/css/perfect-scrollbar.css",
                "~/css/application.css",
                "~/css/loading-bar.min.css",
                "~/css/toastr.css",
                "~/css/dialogs.css",
                "~/css/jquery.fileupload.css",
                "~/css/jquery.fileupload-ui.css",
                "~/css/blueimp-gallery.min.css",
                "~/css/angular-ui-tree.min.css",
                "~/css/angular-motion.min.css",
                "~/css/select.css",
                "~/css/select2.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/MyApp").Include(
                "~/app/home/HomeController.js",
                "~/app/system/associado/ListingAssociadosController.js",
                "~/app/system/venda/VendaController.js",
                "~/app/system/venda/VendaMobileController.js",
                "~/app/home/CacheController.js",
                "~/app/system/loja/ListingLojasController.js",
                "~/app/system/loja/LojaController.js",
                "~/app/system/loja/LojaMensagensController.js",
                "~/app/system/admopers/opers.js",
                "~/app/system/admopers/limites.js",
                "~/app/associado/LimitesController.js",
                "~/app/associado/LimitesMobileController.js",
                "~/app/associado/ExtratosController.js",
                "~/app/associado/ExtratosMobileController.js",
                "~/app/associado/associadoMenuController.js",
                "~/app/gestaoLogista/GLTransController.js",
                "~/app/gestaoLogista/GLDemonstrativo.js",
                "~/app/emissora/EmissoraNovoCartao.js",
                "~/app/emissora/EmissoraAltSenha.js",
                "~/app/emissora/EmissoraAltSenhaUsuario.js",
                "~/app/emissora/EmissoraAltSegVia.js",
                "~/app/emissora/EmissoraAltLimite.js",
                "~/app/emissora/EmissoraAltCota.js",
                "~/app/emissora/EmissoraAltBloq.js",
                "~/app/emissora/EmissoraAltDesbloq.js",
                "~/app/emissora/EmissoraRelatoriosController.js",
                "~/app/emissora/EmissoraListagemCartaoController.js",
                "~/app/emissora/EmissoraListagemLojaController.js",
                "~/app/emissora/EmissoraListagemUsuarioController.js",
                "~/app/emissora/EmissoraListagemFechamentoController.js",
                "~/app/emissora/EmissoraExpFechamentoController.js",
                "~/app/emissora/EmissoraRelExtratSaldosController.js",
                "~/app/emissora/EmissoraRelExtratFornController.js",
                "~/app/emissora/EmissoraRelExtratTransController.js",
                "~/app/emissora/EmissoraRelExtratTransLojasController.js",
                "~/app/emissora/EmissoraRelRepFinancController.js",
                "~/app/associado/LojistasAssociadoController.js",
                "~/app/associado/LojistasAssociadoMobileController.js",
                "~/app/system/relatorios/ListingRelatoriosController.js",
                "~/app/system/relatorios/ListingRelAssociadosController.js",
                "~/app/system/relatorios/ListingLojistaTransController.js",                
                "~/app/system/relatorios/ListingLojistaPendsController.js",
                "~/app/system/cancelamento/CancelamentoController.js"
                ));
        }
    }
}
