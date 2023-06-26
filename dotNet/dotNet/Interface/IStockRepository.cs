using dotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> GetList();
    }
}
