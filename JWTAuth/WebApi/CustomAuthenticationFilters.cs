using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace WebApi
{
    public class CustomAuthenticationFilters : AuthorizeAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }



        public  async Task  AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParameter = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            String[] TokenandUser = null;

            if(authorization==null)
            {

                context.ErrorResult = new AuthenticationFailureResult("Missing Authorization Header",request);
                return;
            }

            if( authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("invalid authentication schema", request);
                return;
            }

            TokenandUser = authorization.Parameter.Split(':');

            String Token = TokenandUser[0];
            String userName = TokenandUser[1];

            string ValidateUserName = TokenManager.validateToken(Token);

            if(userName !=  ValidateUserName)
            {
                context.ErrorResult = new AuthenticationFailureResult("invalid token for user", request);
                return;
            }




            if(String.IsNullOrEmpty(Token))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                return;
            }
            context.Principal = TokenManager.GetPrincipal(Token);



        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            if(result.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", "realm=localhost"));
            }
            context.Result = new ResponseMessageResult(result);
        }

        public class AuthenticationFailureResult : IHttpActionResult
        {

            public string ReasonPhrase;
            public HttpRequestMessage Request { get; set; }
            
            public AuthenticationFailureResult(string reasonPhrase , HttpRequestMessage request)
            {
                ReasonPhrase = reasonPhrase;
                Request = request;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(Execute());
            }

            private HttpResponseMessage Execute()
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                responseMessage.RequestMessage = Request;
                responseMessage.ReasonPhrase = ReasonPhrase;
                return responseMessage;


            }
        }



    }
}