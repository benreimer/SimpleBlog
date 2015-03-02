using SimpleBlog.Models;
using System.Linq;
using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using SimpleBlog.Infrastructure.Extensions;

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

           var baseQuery = Database.Session.Query<Post>().OrderByDescending(f => f.CreatedAt);

           var postIds = baseQuery
               .Skip((page - 1) * PostsPerPage)
               .Take(PostsPerPage)
               .Select(p => p.Id)
               .ToArray();

           var currentPostPage = baseQuery
               .Where(p => postIds.Contains(p.Id))
                .FetchMany(f => f.Tags)
                .Fetch(f => f.User)
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
                    isNew = true,
                    Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox
                    {
                        Id = tag.Id,
                        Name = tag.Name,
                        IsChecked = false
                    }).ToList()
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
            Title = post.Title,
                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsChecked = post.Tags.Contains(tag)
                }).ToList()
            });

 
        }

    //to_do: get antiforgerytoken working on this page
        [HttpPost,ValidateInput(false)]
        public ActionResult Form(PostsForm form)
        {
            form.isNew = form.PostId == null;

            if (!ModelState.IsValid)
                return View(form);

            var selectedTags = ReconsileTags(form.Tags).ToList();

            Post post;
                if(form.isNew)
                {
                    post = new Post
                    {
                        CreatedAt = DateTime.UtcNow, 
                        User = Auth.User,
                    };

                    foreach (var tag in selectedTags)
                        post.Tags.Add(tag);
                }
                else
                {
                    post = Database.Session.Load<Post>(form.PostId);

                    if (post == null)
                        return HttpNotFound();

                    post.UpdatedAt = DateTime.UtcNow;

                    foreach (var toAdd in selectedTags.Where(t => !post.Tags.Contains(t)))
                        post.Tags.Add(toAdd);

                    foreach (var toRemove in post.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                        post.Tags.Remove(toRemove);
                }

                post.Title = form.Title;
                post.Slug = form.Slug;
                post.Content = form.Content;

                Database.Session.SaveOrUpdate(post);
                return RedirectToAction("Index");
        }


        //to_do: figure out how to make the trash, delete, restore below into 2-3 lines of code
        //passing lambdas and expressions?
    //also once any of these actions are complete, I am returned to the index page.  I don't necessarily want this.
    //I want to remain on the current page - see if I can get this to work


    [HttpPost]
        public ActionResult Trash (int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            post.DeletedAt = DateTime.UtcNow;
            Database.Session.Update(post);
            return RedirectToAction("Index");
        }

    [HttpPost]
        public ActionResult Delete(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            Database.Session.Delete(post);
            return RedirectToAction("Index");
        }

    [HttpPost]
        public ActionResult Restore(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            post.DeletedAt = null;
            Database.Session.Update(post);
            return RedirectToAction("Index");
        }


    private IEnumerable<Tag> ReconsileTags(IEnumerable<TagCheckbox> tags)
    {
        foreach (var tag in tags.Where(t => t.IsChecked))
        {
            if (tag.Id != null)
            {
                yield return Database.Session.Load<Tag>(tag.Id);
                continue;
            }

            var existingTag = Database.Session.Query<Tag>().FirstOrDefault(t => t.Name == tag.Name);
            if(existingTag != null)
            {
                yield return existingTag;
                continue;
            }

            var newTag = new Tag
            {
                Name = tag.Name, 
                Slug = tag.Name.Slugify()
            };

            Database.Session.Save(newTag);
            yield return newTag;
        }
    }


    }
}