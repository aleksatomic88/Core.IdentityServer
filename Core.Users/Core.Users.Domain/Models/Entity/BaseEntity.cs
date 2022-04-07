using System;

namespace Core.Users.Domain.Model
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime UpdatedAt { get; set; }

        //public int? CreatedById { get; set; }
        //public int? UpdatedById { get; set; }

        //public byte[] RowVersion { get; set; }
    }
}
