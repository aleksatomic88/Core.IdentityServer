using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Users.DAL.Entity
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public DateTime UpdatedAt { get; set; }

        //public int? CreatedById { get; set; }
        //public int? UpdatedById { get; set; }


        // A timestamp/rowversion is a property for which a new value is automatically generated by the database every time a row is inserted or updated.
        // The property is also treated as a concurrency token, ensuring that you get an exception if a row you are updating has changed since you queried it.
        // SQL Server  - byte[] property is usually used, which will be set up as a ROWVERSION column in the database.
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}