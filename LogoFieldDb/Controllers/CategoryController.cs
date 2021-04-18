using LogoFieldDb.Api_Data;
using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogoFieldDb.Controllers
{
    public class CategoryController : ApiController
    {
        LogoFieldDbEntities db = new LogoFieldDbEntities();
        CategoryData categoryData = new CategoryData();
        List<CategoryML> cat = new List<CategoryML>();
        List<CategoryML> vals = new List<CategoryML>();

        [HttpGet]
        [Route("api/Category")]
        public HttpResponseMessage GetCategory()
        {
            try
            {
                cat = categoryData.GetCategory();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, cat);
        }


        [HttpGet]
        [Route("api/Barval")]
        public HttpResponseMessage Getbarval()
        {
            try
            {
                vals = categoryData.Getbarval();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, vals);
        }

        [HttpPost]
        [Route("api/databarval")]
        public HttpResponseMessage Postbarval(CategoryML model)
        {
            long id = 0;
            try
            {

                if (model != null)
                {
                    id = categoryData.Postbarval(model);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, id);

        }


        [HttpPost]
        [Route("api/updatebarval")]
        public HttpResponseMessage Updatebarval(CategoryML model)
        {
            //List<CategoryML> res = new List<CategoryML>();
            Category res = new Category();
            long id = 0;
            try
            {
                res = db.Categories.FirstOrDefault(x => x.Id == model.Id);
                if (res != null)
                {

                    res.Name = model.Name;
                    db.SaveChanges();
                    id = res.Id;

                }
                else
                {

                    id = categoryData.Postbarval(model);


                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, id);


        }

        [HttpPost]
        [Route("api/deletebarval")]
        public HttpResponseMessage Deletebarval(CategoryML model)
        {
            Category res = new Category();
            long id = 0;
            try
            {
                res = db.Categories.FirstOrDefault(x=>x.Id==model.Id);
                db.Categories.Remove(res);
                db.SaveChanges();
                id = res.Id;
                

            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
            return Request.CreateResponse(HttpStatusCode.OK, id);

        }





    }
}
