using Common.Model;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Common.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        ILogger<GlobalExceptionFilter> logger = null;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> exceptionLogger,
            IWebHostEnvironment hostingEnvironment)
        {
            logger = exceptionLogger;
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            string content = "";

            var result = new ContentResult();

            if (context.Exception is ValidationError)
            {
                var exception = context.Exception as ValidationError;

                content = JsonConvert
                    .SerializeObject(new Response<object>(null, exception.Errors), new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                string errorMessage = "Something went wrong. ";
                errorMessage += _hostingEnvironment.IsDevelopment() ? context.Exception.Message : string.Empty;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (context.Exception is DbUpdateConcurrencyException)
                {
                    errorMessage = "Someone already changed this record.";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                }

                content = JsonConvert
                    .SerializeObject(new Response<object>(null, new ApiError(context.HttpContext.Response.StatusCode, errorMessage, Severity.Error)), new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
            }

            result.Content = content;
            result.ContentType = "application/json";

            context.Result = result;
        }
    }
}
