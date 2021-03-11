using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Pipeline;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class SampleMiddleware
    {
        public Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            context.Items.Add("Greeting", "Hello from our middleware");
            return next(context);
        }
    }
}
