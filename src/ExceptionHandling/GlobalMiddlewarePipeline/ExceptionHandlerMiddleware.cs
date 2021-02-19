using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GlobalMiddlewarePipeline {
    public class GlobalExceptionFilter : IAsyncExceptionFilter {
        public async Task OnExceptionAsync(ExceptionContext context) {
            if (context.Exception is CustomExceptionBase ex) {
                Console.WriteLine("damn");
                context.Result = new BadRequestObjectResult(ex.TransformToJson());
            }
        }
    }
}