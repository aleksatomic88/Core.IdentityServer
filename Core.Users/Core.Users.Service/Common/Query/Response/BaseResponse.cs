using System;

namespace Core.Users.Service.Response
{
    public abstract class BaseResponse
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
