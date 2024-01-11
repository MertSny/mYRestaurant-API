using System.ComponentModel.DataAnnotations;

namespace Core.Entites.Concrete
{
    public class UserOperationClaim : EntityMain, IEntity
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
    }
}
