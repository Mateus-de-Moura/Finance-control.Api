using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using finance_control.Domain.Entity;

namespace finance_control.Domain.Interfaces.Repositories
{
    public interface ILoginLocationDataRepository
    {
        Task<Result<LoginLocationData>> Create(LoginLocationData loginLocationData);
        Task<Result<List<LoginLocationData>>> GetLocations(Guid UserId);
    }
}
