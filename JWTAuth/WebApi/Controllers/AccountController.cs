using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        [Route("api/loginrequest")]
        public HttpResponseMessage ValidLogin(string userName , string userPassword )
        {
            if(userName=="admin" && userPassword =="admin")
            {
                return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(userName));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "user name and password is invalid");
            }
        }


        [HttpGet]
        [Route("api/getEmployee")]
        [CustomAuthenticationFilters]
        public HttpResponseMessage GetEmployee()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "sucessfull valid");

        }


    }
}
