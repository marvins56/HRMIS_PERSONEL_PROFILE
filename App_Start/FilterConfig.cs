using System.Web;
using System.Web.Mvc;

namespace HRMIS_PERSONEL_PROFILE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
