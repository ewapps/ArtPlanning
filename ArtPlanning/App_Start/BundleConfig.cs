using System.Web.Optimization;

namespace ArtPlanning
{
    public class BundleConfig
    {        
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            // CSS / bootstrap and inspinia
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min.css",
                                                                 "~/Content/animate.css",
                                                                 "~/Content/style.css",
                                                                 "~/Content/ewapps.css",
                                                                 "~/Content/ewapps.agenda.css",
                                                                 "~/Content/PagedList.css",
                                                                 "~/Content/plugins/metismenu/metisMenu.css"));

            // CSS / font awesome
            bundles.Add(new StyleBundle("~/font-awesome/css").Include("~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.2.1.min.js",
                                                                     "~/Scripts/jquery-ui.min.js",
                                                                     "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            // CSS jquery
            bundles.Add(new StyleBundle("~/jquery-ui/css").Include("~/Content/jquery-ui.css",
                                                                   "~/Content/jquery-ui.structure.css",
                                                                   "~/Content/jquery-ui.theme.css"));

            // bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));

            // inspinia
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include("~/Scripts/plugins/metismenu/metisMenu.min.js",
                                                                       "~/Scripts/plugins/pace/pace.min.js",
                                                                       "~/Scripts/app/inspinia.js"));

            // sclimscroll
            bundles.Add(new ScriptBundle("~/bundles/slimscroll").Include("~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            // validate
            bundles.Add(new ScriptBundle("~/bundles/validate").Include("~/Scripts/plugins/validate/jquery.validate.min.js",
                                                                       "~/Scripts/plugins/validate/jquery.validate.additional-methods.min.js",
                                                                       "~/Scripts/plugins/validate/jquery.validate.unobtrusive.min.js",
                                                                       "~/Scripts/plugins/validate/validate.ewapps.js"));

            // CSS Awesome Checkbox
            bundles.Add(new StyleBundle("~/awesomecheckbox/css").Include("~/Content/plugins/awesome/awesome-bootstrap-checkbox.css"));

            // Ewapps sessiontimeout
            bundles.Add(new ScriptBundle("~/bundles/ewappssessiontimeout").Include("~/Scripts/ewapps.session-timeout.js"));

            // toastr
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/plugins/toastr/toastr.min.js",
                                                                     "~/Scripts/plugins/toastr/ewapps.toastr.js"));

            // CSS toastr
            bundles.Add(new StyleBundle("~/toastr/css").Include("~/Content/plugins/toastr/toastr.min.css"));

            // sweetalert
            //bundles.Add(new ScriptBundle("~/bundles/swal").Include("~/Scripts/plugins/sweetalert/sweetalert.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/swal").Include("~/Scripts/plugins/sweetalert/sweetalert2.min.js"));

            // CSS sweetalert
            //bundles.Add(new StyleBundle("~/swal/css").Include("~/Content/plugins/sweetalert/sweetalert.css"));
            bundles.Add(new StyleBundle("~/swal/css").Include("~/Content/plugins/sweetalert/sweetalert2.css"));

            // bsdatepicker
            bundles.Add(new ScriptBundle("~/bundles/bsdatepicker").Include("~/Scripts/plugins/bsdatepicker/bootstrap-datepicker.min.js",                                                                           
                                                                           "~/Scripts/plugins/bsdatepicker/locales/bootstrap-datepicker.fr.min.js"));

            // CSS bsdatepicker
            bundles.Add(new StyleBundle("~/bsdatepicker/css").Include("~/Content/plugins/bsdatepicker/bootstrap-datepicker.min.css"));

            // clockpicker
            bundles.Add(new ScriptBundle("~/bundles/clockpicker").Include("~/Scripts/plugins/clockpicker/clockpicker.js"));

            // CSS clockpicker
            bundles.Add(new StyleBundle("~/clockpicker/css").Include("~/Content/plugins/clockpicker/clockpicker.css"));

            // fullcalendar
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include("~/Scripts/plugins/fullcalendar/moment.min.js",
                                                                           "~/Scripts/plugins/fullcalendar/fullcalendar.min.js",
                                                                           "~/Scripts/plugins/fullcalendar/locale/fr.js",
                                                                           "~/Scripts/plugins/scheduler/scheduler.min.js"));

            // CSS fullcalendar
            bundles.Add(new StyleBundle("~/fullcalendar/css").Include("~/Content/plugins/fullcalendar/fullcalendar.min.css",
                                                                      "~/Content/plugins/scheduler/scheduler.min.css"));

            // CSS fullcalendar print
            bundles.Add(new StyleBundle("~/fullcalendarprint/css").Include("~/Content/plugins/fullcalendar/fullcalendar.print.min.css"));

            // scheduler
            bundles.Add(new ScriptBundle("~/bundles/scheduler").Include("~/Scripts/plugins/scheduler/scheduler.min.js"));

            // CSS scheduler
            bundles.Add(new StyleBundle("~/scheduler/css").Include("~/Content/plugins/scheduler/scheduler.min.css"));

            // chosen
            bundles.Add(new ScriptBundle("~/bundles/chosen").Include("~/Scripts/plugins/chosen/chosen.jquery.js"));

            // chosen styles
            bundles.Add(new StyleBundle("~/chosen/css").Include("~/Content/plugins/chosen/bootstrap-chosen.css"));

            // touchpunch
            bundles.Add(new ScriptBundle("~/bundles/touchpunch").Include("~/Scripts/plugins/touchpunch/jquery.ui.touch-punch.min.js"));

            // icheck
            bundles.Add(new ScriptBundle("~/bundles/icheck").Include("~/Scripts/plugins/icheck/icheck.min.js"));
            
            // CSS icheck
            bundles.Add(new StyleBundle("~/icheck/css").Include("~/Content/plugins/icheck/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/extensions").Include("~/Scripts/extensions.js"));

            // CSS / material.agenda
            bundles.Add(new StyleBundle("~/material/css").Include("~/Content/ewapps.material.agenda.css"));

            // select2
            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/plugins/select2/select2.full.min.js",
                                                                      "~/Scripts/plugins/select2/locale/fr.js"));

            // CSS select2
            bundles.Add(new StyleBundle("~/select2/css").Include("~/Content/plugins/select2/select2.css"));

            // colorpicker
            bundles.Add(new ScriptBundle("~/bundles/colorpicker").Include("~/Scripts/plugins/colorpicker/bootstrap-colorpicker.min.js"));

            // CSS colorpicker
            bundles.Add(new StyleBundle("~/colorpicker/css").Include("~/Content/plugins/colorpicker/bootstrap-colorpicker.min.css"));

            // jstree
            bundles.Add(new ScriptBundle("~/bundles/jstree").Include("~/Scripts/plugins/jstree/jstree.min.js"));

            // CSS jstree
            bundles.Add(new StyleBundle("~/jstree/css").Include("~/Content/plugins/jstree/themes/default/style.min.css"));

            // qtip
            bundles.Add(new ScriptBundle("~/bundles/qtip").Include("~/Scripts/plugins/qtip/jquery.qtip.min.js"));

            // CSS qtip
            bundles.Add(new StyleBundle("~/qtip/css").Include("~/Content/plugins/qtip/jquery.qtip.min.css"));
        }
    }
}
