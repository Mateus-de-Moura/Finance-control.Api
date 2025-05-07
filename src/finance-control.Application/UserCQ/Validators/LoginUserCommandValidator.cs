using finance_control.Application.UserCQ.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finance_control.Application.UserCQ.Validators
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("O campo 'email' não pode estar vazio")
               .EmailAddress().WithMessage("Informe um  email válido");

            RuleFor(x => x.Password).NotEmpty().WithMessage("O campo 'password' não pode estar vazio.");
        }
    }
}
