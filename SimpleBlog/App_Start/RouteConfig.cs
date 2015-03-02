using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleBlog.Controllers;


namespace SimpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var namespaces = new[] { typeof(PostsController).Namespace };
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("TagForRealThisTime", "tag/{idAndSlug}", new { Controller = "Posts", Action = "Tag" }, namespaces); 
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { Controller = "Posts", Action = "Tag" }, namespaces);

            routes.MapRoute("PostForRealThisTime", "post/{idAndSlug}", new { Controller = "Posts", Action = "Show" }, namespaces);
            routes.MapRoute("Post", "post/{id}-{slug}", new { Controller = "Posts", Action = "Show" }, namespaces);

            
            routes.MapRoute("Login", "login", new { Controller ="Auth", Action="Login"},namespaces);

            routes.MapRoute("Logout", "logout", new { Controller = "Auth", Action = "Logout" }, namespaces);

            routes.MapRoute("Home", "", new { Controller = "Posts", Action = "Index" },namespaces);

            routes.MapRoute("Sidebar", "", new { controller = "Layout", action = "Sidebar" }, namespaces);
        }
    }
}