using RBACExample.Attribute;
using RBACExample.Common;
using System.Web.Mvc;

namespace RBACExample.Controllers
{
    [AuthBreadcrumb("管理目錄")]
    public class ManageController : Controller
    {
        [AuthBreadcrumb("管理目錄首頁")]
        public ActionResult Index()
        {

            var ctrlActions = BreadcurmbHelper.GetAll();

            return View(ctrlActions);
        }
    }
}