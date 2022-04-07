using System.ComponentModel.DataAnnotations;

namespace Core.Users.DAL.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
