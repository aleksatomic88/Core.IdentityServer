using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
using System.Text;

namespace Common.Filters
{
    public class LogFilter : IActionFilter
    {
        public LogFilter()
        {

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            LogAction(context);
        }

        private void LogAction(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];
            var parameters = filterContext.ActionArguments;
            var paramsLabel = parameters.Count > 0 ? "with params:\n " : "";

            StringBuilder message = new();

            message.Append($"Invoked action: {controllerName}/{actionName} {paramsLabel}");

            foreach (var elem in parameters)
            {
                var newLine = elem.Key != parameters.Last().Key ? "\n" : "";
                message.Append($"{elem.Key} : {JsonConvert.SerializeObject(elem.Value, Formatting.Indented)}{newLine} ");
            }

            Log.Information(message.ToString().Substring(0, message.Length - 1));
        }
    }
}
