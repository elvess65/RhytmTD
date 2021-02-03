
namespace RhytmTD.Core
{
    public abstract class BaseController : DispatcherProvider
    {
        protected BaseController(Dispatcher dispatcher) : base(dispatcher)
        {
        }
    }
}
