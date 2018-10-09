using System.Web;
using System.Web.Optimization;

namespace HiEIS
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                      "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                      "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js",
                      "~/Scripts/plugins/dataTables/datatables.min.js",
                      "~/Scripts/plugins/moment.js",
                      "~/Scripts/plugins/peity/jquery.peity.min.js",
                      "~/Scripts/plugins/pace/pace.min.js",
                      "~/Scripts/bootstrap-tooltip.js",
                      "~/Scripts/plugins/sweetalert/sweetalert.min.js",
                      "~/Scripts/plugins/inspinia.js",
                      "~/Scripts/plugins/iCheck/icheck.min.js",
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js",
                      "~/Scripts/typeahead.bundle.js",
                      "~/Scripts/plugins/toastr/toastr.min.js",
                      "~/Scripts/hieis-custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                      "~/Content/plugins/sweetalert/sweetalert.css",
                      "~/Content/plugins/toastr/toastr.min.css",
                      "~/Content/plugins/dataTables/datatables.min.css",
                      "~/Content/plugins/datapicker/datepicker3.css",
                      "~/Content/plugins/iCheck/custom.css",
                      "~/Content/typehead.css",
                      "~/Content/animate.css",
                      "~/Content/style.css",
                      "~/Content/hieis-custom.css"));
        }
    }
}
