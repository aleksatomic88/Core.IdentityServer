using System.Collections.Generic;

namespace Identity.Shared.Model
{
    public class Response<T>
    {
        public bool IsSuccess =>
            Errors == null || Errors.Count == 0;

        public T Data { get; set; }

        public List<ApiError> Errors { get; set; }

        public Response(T data, List<ApiError> errors)
        {
            Data = data;
            Errors = errors;
        }

        public Response(T data, ApiError error = null)
        {
            Data = data;

            if (error != null)
            {
                Errors = new List<ApiError> { error };
            }
        }

        public Response()
        {

        }
    }
}
