using dotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> JoinAllTable();
        public Task<JoinTable> JoinTableById(int id);
    }
}
