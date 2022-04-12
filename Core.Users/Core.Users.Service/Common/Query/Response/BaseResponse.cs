using System;

namespace Core.Users.Service
{
    public abstract class BaseResponse
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
