using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SportClub_Bancu.Domain.ModelsDb;

namespace SportClub_Bancu.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        { 
        RuleFor(user => user.Password).NotEmpty().WithMessage("Пароль не может быть пустым")
          .MinimumLength(6).WithMessage("Пароль должен быть не менее 6 символов");
            RuleFor(user => user.Email).NotEmpty().WithMessage("Email не может быть пустым")
           .EmailAddress().WithMessage("Некорректный адрес электронной почты");
        }
    }
}
