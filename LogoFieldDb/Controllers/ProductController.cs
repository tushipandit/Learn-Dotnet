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
    public class ProductController : ApiController
    {
        LogoFieldDbEntities db = new LogoFieldDbEntities();
        ProductData productData = new ProductData();
        List<ProductML> pal = new List<ProductML>();
        List<ProductML> mal = new List<ProductML>();


        [HttpGet]
        [Route("api/product")]
        public HttpResponseMessage GetProduct()
        {
            try
            {
                pal = productData.GetProduct();

            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, pal);
        }

        [HttpGet]
        [Route("api/getproductcoupon")]
        public HttpResponseMessage Getproductcoupon()
        {
            try
            {
                mal = productData.Getproductcoupon();
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, mal);
        }


    }
}
