using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SwaggerTest.Models;

namespace SwaggerTest
{
    public static class Function1
    {

        //https://www.codit.eu/blog/adding-swagger-ui-azure-functions/?country_sel=be
        //https://github.com/Azure/azure-functions-openapi-extension/blob/main/docs/openapi.md
        //https://github.com/aliencube/AzureFunctions.Extensions/blob/aca0051b4f7c5b8823457d26defa1f1cd68492ab/docs/openapi-core.md
        [FunctionName(nameof(Function1))]
        [OpenApiOperation("addDummy", "dummy")]
        [OpenApiRequestBody("application/json", typeof(TestRequest))]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(TestRequest))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
