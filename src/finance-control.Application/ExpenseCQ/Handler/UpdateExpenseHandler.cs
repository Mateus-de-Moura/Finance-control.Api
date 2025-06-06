﻿using finance_control.Application.ExpenseCQ.Commands;
using finance_control.Application.Response;
using finance_control.Domain.Entity;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.ExpenseCQ.Handler
{
    public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, ResponseBase<Expenses>>
    {
        private readonly FinanceControlContex _context;

        public UpdateExpenseHandler(FinanceControlContex context)
        {
            _context = context;
        }

        public async Task<ResponseBase<Expenses>> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id.Equals(request.IdExpense))
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return new ResponseBase<Expenses>
                {
                    ResponseInfo = new ResponseInfo
                    {
                        Title = "Nenhuma despesa encontrada.",
                        ErrorDescription = "não foi possivel localizar um registro com o Id informado, tente novamente",
                        HttpStatus = 404
                    },
                    Value = null,
                };
            }

            expense.Value = request.Value;
            expense.DueDate = request.DueDate;
            expense.Status = request.Status;
         
            _context.Entry(expense).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return new ResponseBase<Expenses>
            {
                ResponseInfo = null,
                Value = expense,
            };
        }
    }
}
