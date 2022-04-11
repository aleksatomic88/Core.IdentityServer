using System.Text.Json.Serialization;

namespace Shared.Common.Model
{
    public class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string CustomMessage { get; set; }

        [JsonConstructor]
        public ApiError(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public ApiError(int code, string message, string customMessage) : this(code, message)
        {
            CustomMessage = customMessage;
        }

    }
}
