using finance_control.Application.UserCQ.Commands;
using FluentValidation;

namespace finance_control.Application.UserCQ.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Informe um e-mail válido")
              .NotEmpty().WithMessage("O campo 'email'não pode ser vazio")
              .WithErrorCode("400");

            RuleFor(x => x.Name).NotEmpty().WithMessage("O campo 'name' não pode estar vazio")
                .MinimumLength(2).WithMessage("o campo 'name' precisa  ter mais que 2 caracteres.");


            RuleFor(x => x.Surname).NotEmpty().WithMessage("O campo 'name' não pode estar vazio")
                .MinimumLength(2).WithMessage("o campo 'name' precisa  ter mais que 2 caracteres.");
        }

    }
}
