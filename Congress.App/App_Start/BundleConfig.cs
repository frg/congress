using System.Web;
using System.Web.Optimization;

namespace Congress.App
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/foundation").Include(
                      "~/Content/libs/foundation/foundation-5.5.2/js/foundation.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/libs/foundation/foundation-5.5.2/css/foundation.min.css",
                      "~/Content/site.css",
                      "~/Content/libs/font-awesome/font-awesome-4.3.0/css/font-awesome.min.css"));
        }
    }
}
