
using System;

namespace CoreFramework
{
    public abstract class BaseController : DispatcherProvider
    {
        protected BaseController(Dispatcher dispatcher) : base(dispatcher)
        {
        }

        public virtual void InitializeComplete()
        {

        }

        public virtual void Dispose()
        {
        }
    }
}
