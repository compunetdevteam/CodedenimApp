using System.Web.Optimization;

namespace CodedenimWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Assets1/js/jquery-{version}.js"
                        ));
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    //  "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //Create bundel for jQueryUI  
            //js  
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));
            //css  
            bundles.Add(new StyleBundle("~/Content/cssjqryUi").Include(
                "~/Content/themes/base/jquery-ui.css"));


            //create bundle for LandingPage, Login and SignUp

            bundles.Add(new StyleBundle("~/Content/css/LandingPageAssets").Include(
                    //"~/Content/bootstrap.css",
                    //"~/Content/site.css"));
                    "~/Content/LandingPageAssets/css/bootstrap.min.css",
                    "~/Content/LandingPageAssets/css/extralayers.css",
                    "~/Content/LandingPageAssets/css/settings.css",
                    "~/Content/LandingPageAssets/font-awesome/css/font-awesome.min.css",
                    "~/Content/LandingPageAssets/css/nivo-lightbox.css",
                    "~/Content/LandingPageAssets/css/nivo-lightbox-theme/default/default.css",
                    "~/Content/LandingPageAssets/css/animate.css",
                    "~/Content/LandingPageAssets/css/main.css",
                    "~/Content/LandingPageAssets/color/default.css"

                    ));


            bundles.Add(new ScriptBundle("~/bundles/jquery/LandingPageAssets").Include(
                      //"~/Scripts/jquery-{version}.js",
                      "~/Content/LandingPageAssets/js/libs/jquery-2.0.2.min.js",
                      "~/Content/LandingPageAssets/js/libs/jquery-ui.min.html",
                      "~/Content/LandingPageAssets/js/bootstrap/bootstrap.min.js",
                      "~/Content/LandingPageAssets/js/libs/jquery-{version}.js",
                      "~/Content/LandingPageAssets/js/plugins/classie.js",
                      "~/Content/LandingPageAssets/js/plugins/gnmenu.js",
                      "~/Content/LandingPageAssets/js/plugins/jquery.scrollUp.js",
                      "~/Content/LandingPageAssets/js/plugins/nivo-lightbox.min.js",
                      "~/Content/LandingPageAssets/js/plugins/smoothscroll.js",
                      "~/Content/LandingPageAssets/js/plugins/jquery.themepunch.plugins.min.js",
                      "~/Content/LandingPageAssets/js/plugins/jquery.themepunch.revolution.min.js",
                      "~/Content/LandingPageAssets/js/custom.js"
                      ));



            bundles.Add(new StyleBundle("~/Content/css/LoginAssets").Include(
                   //"~/Content/bootstrap.css",
                   //"~/Content/site.css"));
                  // "~/Content/LoginAssets/css/bootstrap.min.css",
                  // "~/Content/LoginAssets/css/font-awesome.min.css",
                  // //"~/Content/LoginAssets/css/smartadmin-production-plugins.min.css",
                  ////"~/Content/LoginAssets/css/smartadmin-production.min.css",
                  // "~/Content/LoginAssets/css/smartadmin-skins.min.css",
                  // "~/Content/LoginAssets/css/smartadmin-rtl.min.css",
                  // "~/Content/LoginAssets/css/demo.min.css"

                   ));

            bundles.Add(new ScriptBundle("~/bundles/js/LoginAssets").Include(
                 "~/Content/LoginPageAssets/js/libs/jquery-3.2.1.min.js",
                 "~/Content/LoginPageAssets/js/libs/jquery-ui.min.js",
                 "~/Content/LoginPageAssets/js/app.config.js",
                 "~/Content/LoginPageAssets/js/bootstrap/bootstrap.min.js",
                 "~/Content/LoginPageAssets/js/plugin/jquery-validate/jquery.validate.min.js",
                 "~/Content/LoginPageAssets/js/plugin/masked-input/jquery.maskedinput.min.js",
                 "~/Content/LoginPageAssets/js/app.min.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/js/Slider").Include(
                    "~/Content/Assets1/js/responsiveslides.min.js"
            ));




            //// New UI /////////////////

            //external javascripts
            bundles.Add(new ScriptBundle("~/bundles/ExternalJs").Include(
                     "~/Content/Assets1/js/jquery-2.2.4.min.js",
                     "~/Content/Assets1/js/jquery-ui.min.js",
                     "~/Content/Assets1/js/bootstrap.min.js"
                     ));

            //JS | jquery plugin collection for this theme 

            bundles.Add(new ScriptBundle("~/bundles/JQueryPluginCollection").Include(
                     "~/Content/Assets1/js/jquery-plugin-collection.js"
                     ));
            //Revolution Slider 5.x SCRIPTS
            bundles.Add(new ScriptBundle("~/bundles/SliderScript").Include(
               "~/Content/Assets1/js/jquery.themepunch.tools.min.js",
               "~/Content/Assets1/js/jquery.themepunch.revolution.min.js"
               ));






            /////this is the CSS style sheet bundle code///////////////////////////////


            // Stylesheet 
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/site.css"
                      "~/Content/Assets1/css/bootstrap.min.css",
                      "~/Content/Assets1/css/jquery-ui.min.css",
                      "~/Content/Assets1/css/animate.css",
                      "~/Content/Assets1/css/css-plugin-collections.css",
                         "~/Content/LandingPageAssets/font-awesome/css/font-awesome.min.css"
                      ));
            //CSS | menuzord megamenu skins 
            bundles.Add(new StyleBundle("~/Content/megamenu/skins").Include(
                     "~/Content/Assets1/css/menuzord-megamenu.css",
                     "~/Content/Assets1/css/menuzord-skins/menuzord-boxed.css"
                      ));
            //Slider from 30billion
            bundles.Add(new StyleBundle("~/Content/Slider").Include(
            "~/Content/Assets1/css/responsiveslides.css",
                     "~/Content/Assets1/css/demo.css"
                      ));
            //Main style File
            bundles.Add(new StyleBundle("~/Content/MainStyleFile").Include(
                 "~/Content/Assets1/css/style-main.css",
                 "~/Content/Assets1/css/utility-classes.css"
                ));
            //Pre-Loader
            bundles.Add(new StyleBundle("~/Content/PreLoader").Include(
               "~/Content/Assets1/css/preloader.css"
               ));

            //Custom Margin Padding Collection
            bundles.Add(new StyleBundle("~/Content/MarginPadding").Include(
            "~/Content/Assets1/css/custom-bootstrap-margin-padding.css"
            ));
            // Responsive media queries
            bundles.Add(new StyleBundle("~/Content/MediaQueries").Include(
         "~/Content/Assets/css/responsive.css"
         ));
            //Revolution Slider 5.x CSS settings
            bundles.Add(new StyleBundle("~/Content/SliderSettings").Include(
                  "~/Content/Assets1/css/settings.css",
                  "~/Content/Assets1/css/layers.css",
                  "~/Content/Assets1/css/navigation.css"
      ));

            //CSS | Theme Color
            bundles.Add(new StyleBundle("~/Content/ThemeColor").Include(
                 "~/Content/Assets1/css/colors/theme-skin-color-set4.css"
     ));
            ////////////End of CSS style sheet bundle code///////////////////////////////

        }
    }
}
