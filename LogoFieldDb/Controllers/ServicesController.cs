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
    public class ServicesController : ApiController
    {

        LogoFieldDbEntities db = new LogoFieldDbEntities();
        serviceData serviceData = new serviceData();
        List<servicesML> serv = new List<servicesML>();

        [HttpGet]
        [Route("api/services")]

        public HttpResponseMessage Getservices()
        {
            try
            {
                serv = serviceData.getservices();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, serv);


        }
    }
}
