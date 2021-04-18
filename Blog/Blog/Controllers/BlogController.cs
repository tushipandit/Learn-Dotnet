using Blog.Api_Data;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{

    [Authorize]
    public class BlogController : Controller
    {

        MYBlogEntities db = new MYBlogEntities();
        blogML model = new blogML();



        // GET: Blog
        

        public ActionResult BlogPost()
        {
            return View("BlogPost");
        }


        [HttpGet]
        public ActionResult Blog()
        {
            List<blogML> res = datalist();
            return View("Blog", res);
        }


        public List<blogML> datalist()
        {
            List<blogML> result = (from li in db.blogs
                                   select new blogML
                                   {
                                       Id = li.Id,
                                       title = li.title,
                                       miniintro = li.miniintro,
                                       description = li.description,
                                       seotitle = li.seotitle,
                                       seodiscription = li.seodiscription,
                                       seokeywords = li.seokeywords,
                                       language = li.language,
                                       createdon = li.createdon,
                                       isalive = li.isalive,
                                       postedby = li.postedby,
                                       photo = li.photo,
                                       position = li.position,
                                       seourl = li.seourl

                                   }
                                   ).ToList();


            return result;

        }




        [HttpGet]
        public ActionResult BlogDatabyid(int? id)
        {
            var result = db.blogs.FirstOrDefault(x => x.Id == id);

            return View("Details", result);
        }





        [HttpPost]
        public ActionResult Blogupdate(blogML data)
        {
       
            try
            {
                blog res = new blog();
                res = db.blogs.FirstOrDefault(x => x.Id == model.Id);

               
                if (res != null)
                {
                    addData(res, data);
                    db.SaveChanges();

                }
                else
                {
                    blog result = new blog();
                    addData(result, data);
                    db.blogs.Add(result);
                    db.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
            return View( "Blog",datalist() );
            
        }


        public ActionResult  deleteDataById(int? id)
        {
            blog result = db.blogs.FirstOrDefault(x => x.Id == id);
                db.blogs.Remove(result);
                db.SaveChanges();

            return View("Blog",datalist());
        }

        public void addData(blog res, blogML model)
        {


            res.Id = model.Id;
            res.title = model.title;
            res.miniintro = model.miniintro;
            res.description = model.description;
            res.seotitle = model.seotitle;
            res.seodiscription = model.seodiscription;
            res.seokeywords = model.seokeywords;
            res.language = model.language;
            res.createdon = model.createdon;
            res.isalive = model.isalive;
            res.postedby = model.postedby;
            res.photo = model.photo;
            res.position = model.position;
            res.seourl = model.seourl;


        }

    }




}

