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
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O nome de usuário não pode estar vazio");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha não pode estar vazia.");
        }
    }
}
