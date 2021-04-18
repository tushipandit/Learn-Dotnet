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
    public class Sub_CategoryController : ApiController
    {

        LogoFieldDbEntities db = new LogoFieldDbEntities();
        Sub_CateogyData sub_CateogyData = new Sub_CateogyData();
        List<Sub_CategoryML> scat = new List<Sub_CategoryML>();

        [HttpGet]
        [Route("api/Sub_Category")]
        public HttpResponseMessage GetSub_Category()
        {
            try
            {
                scat = sub_CateogyData.GetSub_Category();


            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, scat);


        }




    }
}
