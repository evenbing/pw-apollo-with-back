using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services.Hubs
{
    public interface IBalanceHubClient
    {
        Task UpdateBalance(int balance);
    }
}
