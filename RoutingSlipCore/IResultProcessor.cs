using System.Threading.Tasks;

namespace Hdq.Routingslip.Core
{
    public interface IResultProcessor<TResult>
    {
        Task Process(TResult result);
    }
}