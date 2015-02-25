using SimpleBlog.Models;
using System.Linq;
using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using System;


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

        public  ActionResult New()
        {
            return View("Form", new PostsForm
                {
                    isNew = true
                });
        }

        public ActionResult Edit(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            return View("Form", new PostsForm
            {
                isNew = false,
                PostId = id,
            Content = post.Content,
            Slug = post.Slug,
            Title = post.Title
            });
        }

        [HttpPost]
        public ActionResult Form(PostsForm form)
        {
            form.isNew = form.PostId == null;

            if (!ModelState.IsValid)
                return View(form);

            Post post;
                if(form.isNew)
                {
                    post = new Post
                    {
                        CreatedAt = DateTime.UtcNow, 
                        User = Auth.User,
                    };
                }
                else
                {
                    post = Database.Session.Load<Post>(form.PostId);
                    if (post == null)
                        return HttpNotFound();
                    post.UpdatedAt = DateTime.UtcNow;
                }

            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;

            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("Index");
        }
    }
}