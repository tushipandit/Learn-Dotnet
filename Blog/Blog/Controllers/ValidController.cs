using Blog.Api_Data;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Blog.Controllers
{
    public class ValidController : Controller
    {

        MYBlogEntities db = new MYBlogEntities();



        [HttpPost]
        public ActionResult Login(userML data)
        {
            try
            {
                user profile = new user();
                profile = db.users.FirstOrDefault(x => x.username == data.username && x.password==data.password);
                if (profile == null)
                {

                    ModelState.AddModelError("","Invalid username and password");
                    return RedirectToAction("Loginuser");
                    
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(profile.username , false);
                    return RedirectToAction("Blog" ,"Blog");
                   

                }
            }catch(Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult Loginuser()
        {
            return View("ValidityLogin");
        }


        [HttpPost]
        public ActionResult Signup(user data)
        {
            try
            {

                db.users.Add(data);
                db.SaveChanges();


                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Loginuser");

        }

        public ActionResult Signupuser()
        {
            return View("Signupuser");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Loginuser");
        }








    }
}