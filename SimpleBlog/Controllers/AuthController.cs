using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            return View(new AuthLogin
                {                    
                });
        }

        [HttpPost]
        public ActionResult Login(AuthLogin form)
        {


            if (!ModelState.IsValid)
                return (View(form));

            if(form.Username != "ben")
            {
                ModelState.AddModelError("Username", "Username or password id not valid");
                return View(form);
            }
            return Content("The form is valid!");
            

        }
    }


}