using finance_control.Application.ExpenseCQ.Commands;
using FluentValidation;

namespace finance_control.Application.ExpenseCQ.Validators
{
    public class CreateExpenseValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseValidator() 
        {
            RuleFor(e => e.Value)
                  .NotEmpty()
                  .WithMessage("Valor não pode ser vazio");

            RuleFor(e => e.DueDate)
                  .NotEmpty()
                  .WithMessage("Data de vencimento não pode ser vazia.");
        }
    }
}
