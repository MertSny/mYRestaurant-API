using Core.Entites;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class ExceptionLog : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
