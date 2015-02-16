using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Models;
using NHibernate.Linq;
using System.Linq;


namespace SimpleBlog.Areas.Admin.Controllers
{
    

    [AuthorizeEnum(Role.Administrator)]
    [SelectedTab("users")]

    
    public class UsersController: Controller
    {
        public ActionResult Index()
        {
            return View(new UsersIndex 
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }
    }
}