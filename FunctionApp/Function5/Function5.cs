using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Pipeline;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;

namespace FunctionApp
{
    public class Function5
    {
        private readonly IHttpResponderService _responderService;

        public Function5(IHttpResponderService responderService)
        {
            _responderService = responderService;
        }

        [FunctionName(nameof(Function5))]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
            FunctionExecutionContext executionContext)
        {
            var logger = executionContext.Logger;
            logger.LogInformation("message logged");

            return _responderService.ProcessRequest(req);
        }
    }

    public interface IHttpResponderService
    {
        HttpResponseData ProcessRequest(HttpRequestData httpRequest);
    }

    public class DefaultHttpResponderService : IHttpResponderService
    {
        public DefaultHttpResponderService()
        {

        }

        public HttpResponseData ProcessRequest(HttpRequestData httpRequest)
        {
            var response = new HttpResponseData(HttpStatusCode.OK);
            var headers = new Dictionary<string, string>();
            headers.Add("Date", "Mon, 18 Jul 2016 16:06:00 GMT");
            headers.Add("Content", "Content - Type: text / html; charset = utf - 8");

            response.Headers = headers;

            var responseBuilder = new StringBuilder();

            responseBuilder.AppendLine($"Method: {httpRequest.Method}");
            responseBuilder.AppendLine($"Request URL: {httpRequest.Url}");
            responseBuilder.AppendLine($"Original request: {httpRequest.ReadAsString()}");
            responseBuilder.AppendLine($"Headers:");
            foreach (var item in httpRequest.Headers)
            {
                responseBuilder.AppendLine($"\t{item.Key} = {item.Value}");
            }

            responseBuilder.AppendLine($"Identities:");
            foreach (var item in httpRequest.Identities)
            {
                responseBuilder.AppendLine($"\tAuth type: {item.AuthenticationType}");

                responseBuilder.AppendLine($"\tClaims:");
                foreach (var claim in item.Claims)
                {
                    responseBuilder.AppendLine($"\t\tType: {claim.Type}, Value: {claim.Value}");

                }
            }

            response.Body = responseBuilder.ToString();
            return response;
        }
    }
}
