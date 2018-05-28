using System.Threading.Tasks;

namespace Hdq.Routingslip.Core
{
    public interface IResultProcessor<TResult>
    {
        Task<bool> Process(TResult result);
    }
}