using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Interface
{
    public interface IStockRepository
    {
        public Task<IEnumerable<JoinTable>> GetList();
    }
}
