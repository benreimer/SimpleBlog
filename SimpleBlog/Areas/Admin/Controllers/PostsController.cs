﻿using System.Web.Mvc;
using SimpleBlog.Infrastructure;

namespace SimpleBlog.Areas.Admin.Controllers
{

[AuthorizeEnum(Role.Administrator)]

    public class PostsController : Controller
    {
        public ActionResult Index()
        {
            return Content("ADMIN POSTS!");
        }
    }
}