using System.Threading.Tasks;

namespace RoutingSlipTests
{
    public interface IResultProcessor<TResult>
    {
        Task<bool> Process(TResult result);
    }
}