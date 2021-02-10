
namespace CoreFramework.StateMachine
{
    public abstract class AbstractState
    {
        public bool StateIsActive { get; private set; }

        protected Dispatcher Dispatcher => Dispatcher.Instance;


        public virtual void EnterState()
        {
            StateIsActive = true;
        }

        public virtual void ExitState()
        {
            StateIsActive = false;
        }
    }
}
