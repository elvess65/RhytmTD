
namespace CoreFramework
{
    public abstract class DispatcherProvider
    {
        public Dispatcher Dispatcher { get; private set; }

        public DispatcherProvider(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
    }
}
