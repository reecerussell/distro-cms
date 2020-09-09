using System.Threading.Tasks;

namespace Shared
{
    public interface IConnectionStringProvider
    {
        Task<string> GetConnectionString();
        Task<string> GetConnectionString(string name);
    }
}