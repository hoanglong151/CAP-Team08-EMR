using System.Web;
using System.Web.Optimization;

namespace ElectronicMedicalRecords
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/LayoutMultipleModelScript").Include(
                      "~/Areas/Admin/assets/vendors/jquery/jquery.min.js",
                      "~/Areas/Admin/assets/vendors/dayjs/dayjs.min.js",
                      "~/Areas/Admin/assets/js/bootstrap.bundle.min.js",
                      "~/Areas/Admin/assets/DataTables/datatables.min.js",
                      "~/Areas/Admin/assets/vendors/perfect-scrollbar/perfect-scrollbar.min.js",
                      "~/Areas/Admin/assets/vendors/choices.js/choices.min.js",
                      "~/Areas/Admin/assets/vendors/sweetalert2/sweetalert2.all.min.js",
                      "~/Areas/Admin/assets/js/bootstrap-tagsinput.js",
                      "~/Areas/Admin/assets/vendors/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/bundles/LayoutMultipleModelScriptMain").Include(
                "~/Areas/Admin/assets/js/main.js"));

            bundles.Add(new StyleBundle("~/bundles/LayoutMultipleModel").Include(
                      "~/Areas/Admin/assets/DataTables/datatables.min.css",
                      "~/Areas/Admin/assets/vendors/choices.js/choices.min.css",
                      "~/Areas/Admin/assets/css/bootstrap.css",
                      "~/Areas/Admin/assets/vendors/iconly/bold.css",
                      "~/Areas/Admin/assets/vendors/sweetalert2/sweetalert2.min.css",
                      "~/Areas/Admin/assets/vendors/perfect-scrollbar/perfect-scrollbar.css",
                      "~/Areas/Admin/assets/css/app.css",
                      "~/Areas/Admin/assets/css/bootstrap-tagsinput.css",
                      "~/Areas/Admin/assets/css/customize.css",
                      "~/Areas/Admin/assets/css/loading-page.css")
                .Include("~/Areas/Admin/assets/vendors/bootstrap-icons/bootstrap-icons.css", new CssRewriteUrlTransform()));
            BundleTable.EnableOptimizations = true;
        }
    }
}
