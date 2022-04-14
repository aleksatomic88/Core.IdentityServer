using System;

namespace Common.Response
{
    public abstract class BaseResponse
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
