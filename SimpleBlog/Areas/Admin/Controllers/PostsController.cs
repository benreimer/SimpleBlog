using SimpleBlog.Models;
using System.Linq;
using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;


namespace SimpleBlog.Areas.Admin.Controllers
{

[AuthorizeEnum(UserRights.admin)]
[SelectedTab("posts")]

    public class PostsController : Controller
    {
        private const int PostsPerPage = 5;

        public ActionResult Index(int page = 1)
        {
           var totalPostCount = Database.Session.Query<Post>().Count();
            
           var currentPostPage = Database.Session.Query<Post>()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();

           return View(new PostsIndex
               {
                   Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage)
               });
        }
    }
}