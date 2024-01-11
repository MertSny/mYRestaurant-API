using Entities.DTO;
using FluentValidation;

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
