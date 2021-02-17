using System.Net;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Storage;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp
{
    public static class Function3
    {

        [Function("Function3")]
        public static Result Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString("Success!!");

            return new Result { Response = response, Name = "Azure Functions" };
        }

        public class Result
        {
            [QueueOutput("myQueue", Connection = "AzureWebJobsStorage")]
            public string Name { get; set; }

            public HttpResponseData Response { get; set; }
        }
    }

}
