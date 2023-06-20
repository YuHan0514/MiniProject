
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> GetList();
    }
}
