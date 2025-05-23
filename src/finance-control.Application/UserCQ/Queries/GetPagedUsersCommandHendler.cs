﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using finance_control.Application.Common.Models;
using finance_control.Application.Extensions;
using finance_control.Application.Response;
using finance_control.Application.RevenuesCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Entity;
using finance_control.Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Application.UserCQ.Queries
{
    public class GetPagedUsersCommandHendler(FinanceControlContex context, IMapper mapper) : IRequestHandler<GetPagedUsersCommand, ResponseBase<PaginatedList<UserViewModel>>>
    {
        private readonly FinanceControlContex _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseBase<PaginatedList<UserViewModel>>> Handle(GetPagedUsersCommand request, CancellationToken cancellationToken)
        {
            var queryable = _context.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .OrderBy(x => x.Name)
                .AsQueryable();


            if (!string.IsNullOrEmpty(request.Name))
            {
                queryable = queryable.Where(x => x.Name.Contains(request.Name));
            }
            ;

            var response = await queryable
                 .Select(x => _mapper.Map<UserViewModel>(x))
                .PaginatedListAsync(request.PageNumber, request.PageSize);


            if (response == null)
                return ResponseBase<PaginatedList<UserViewModel>>.Fail("Falha ao buscar dados", "Nenhum item localizado", 404);

            return ResponseBase<PaginatedList<UserViewModel>>.Success(response);

        }
    }
}
