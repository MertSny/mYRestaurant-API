using Entities.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
