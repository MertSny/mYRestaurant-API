using Core.Entites;

namespace Entities.DTO
{
    public class UserDto : IDto
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<UserOperationClaimDto> UserOperationClaims { get; set; }
    }

}
