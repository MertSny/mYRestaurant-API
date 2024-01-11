using Entities.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidationDto
{
    public class UserOperationClaimDtoValidator : AbstractValidator<UserOperationClaimDto>
    {
        public UserOperationClaimDtoValidator()
        {
            RuleFor(p => p.UserId).NotEmpty().GreaterThan(0).WithMessage("Lütfen kullanıcı türü giriniz");
            RuleFor(p => p.OperationClaimId).NotEmpty().GreaterThan(0).WithMessage("Lütfen rol bilgisi giriniz");
        }
    }
}
