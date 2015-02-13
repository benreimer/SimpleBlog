using System.Web.Mvc;
using SimpleBlog.Infrastructure;


namespace SimpleBlog.Areas.Admin.Controllers
{
    

    [AuthorizeEnum(Role.Administrator)]
    [SelectedTab("users")]

    
    public class UsersController: Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}