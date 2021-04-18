using LogoFieldDb.Api_Data;
using LogoFieldDb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LogoFieldDb.Controllers
{
    public class ImagestoreController : ApiController
    {
        LogoFieldDbEntities db = new LogoFieldDbEntities();
        ImagestoreData imagestore = new ImagestoreData();
        List<ImagestoreML> iml = new List<ImagestoreML>();

        [HttpGet]
        [Route("api/Imagestore")]
        public HttpResponseMessage GetImagestore()
        {
            try
            {
                iml = imagestore.GetImagestore();
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, iml);
        }


        [HttpPost]
        [EnableCors(origins: "*", headers: "*", methods: "*" , SupportsCredentials = true)]
        [Route("api/Imageupload")]
        public HttpResponseMessage UploadImage()
        {
            String imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload image
            var postedFile = httpRequest.Files["myFile"];

            //Create custom Filename
            imageName=new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "_");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);

            //Save to Db
            try
            {
                var image = new Imagestore();
                image.Name = imageName;

                db.Imagestores.Add(image);
                db.SaveChanges();
            }
            catch(Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex);
            }


            return Request.CreateResponse(HttpStatusCode.OK);

        }


    }
}
