using FluentValidation;
using System.Text.Json.Serialization;

namespace Common.Model
{
    public class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public Severity Severity { get; set; }
        public string CustomMessage { get; set; }

        [JsonConstructor]
        public ApiError(int code, string message, Severity severity = Severity.Error)
        {
            Code = code;
            Message = message;
            Severity = severity;
        }

        public ApiError(int code, string message, Severity severity, string customMessage) : this(code, message, severity)
        {
            CustomMessage = customMessage;
        }

    }
}
