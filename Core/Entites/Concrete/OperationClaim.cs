using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites.Concrete
{
    public class OperationClaim : EntityMain, IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
