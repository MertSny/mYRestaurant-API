using Entities.DTO;
using FluentValidation;

namespace Business.ValidationRules.FluentValidationDto
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(p => p.AccountId).NotEmpty().WithMessage("Lütfen hesap türü girininiz");
            RuleFor(p => p.FirstName).NotEmpty().MaximumLength(100).WithMessage("Lütfen kullanıcı adı giriniz");
            RuleFor(p => p.LastName).NotEmpty().MaximumLength(100).WithMessage("Lütfen kullanıcı soyadını giriniz");
            RuleFor(p => p.Email).NotEmpty().EmailAddress().WithMessage("Lütfen geçerli Eposta adresi giriniz");

        }
    }
}
