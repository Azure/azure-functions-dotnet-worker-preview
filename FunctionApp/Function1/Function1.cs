using System.Collections.Generic;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;
using Microsoft.Azure.Functions.Worker.Extensions.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Storage;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp
{
    public static class Function1
    {

        [Function("Function1")]
        public static Result Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
            [BlobInput("test-samples/sample1.txt", Connection = "AzureWebJobsStorage")] string myBlob)
        {
            var bookVal = (Book)JsonSerializer.Deserialize(myBlob, typeof(Book));
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Date", "Mon, 18 Jul 2016 16:06:00 GMT");
            response.Headers.Add("Content", "Content - Type: text / html; charset = utf - 8");

            response.WriteString("Book Sent to Queue!");

            return new Result { Response = response, Book = bookVal };
        }

        public class Book
        {
            public string name { get; set; }
            public string id { get; set; }
        }

        public class Result
        {
            [QueueOutput("books", Connection = "AzureWebJobsStorage")]
            public Book Book { get; set; }

            public HttpResponseData Response { get; set; }
        }

    }
}
