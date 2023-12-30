using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
