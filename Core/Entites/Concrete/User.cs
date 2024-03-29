﻿using System.ComponentModel.DataAnnotations;

namespace Core.Entites.Concrete
{
    public class User : EntityMain , IEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
