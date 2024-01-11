using Entities.DTO;
using FluentValidation;

namespace Business.ValidationRules.FluentValidationDto
{
    public class OperationClaimDtoValidator : AbstractValidator<OperationClaimDto>
    {
        public OperationClaimDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty().MaximumLength(100).WithMessage("Rol ismi giriniz");
        }
    }
}
