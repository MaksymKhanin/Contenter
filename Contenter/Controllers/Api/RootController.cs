using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;


namespace Contenter.Controllers.Api
{
    public class RootController : ApiController
    {
       
        
        public ResponseMessageResult MakeResponseError(int statusCode, string message)
        {
            return new ResponseMessageResult(Request.CreateErrorResponse((HttpStatusCode)statusCode, new HttpError(message)));
        }

        
        public ResponseMessageResult MakeCustomResponse(int statusCode, object message)
        {
            return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)statusCode, message));
        }

       
    }
}
