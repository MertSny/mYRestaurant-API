using Core.Entites;

namespace Entities.Concrete
{
    public class PerformanceLog : IEntity
    {
        public int Id { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public int Duration { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}
