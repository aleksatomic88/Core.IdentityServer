using System;

namespace Common.Response
{
    public abstract class BaseResponse
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
