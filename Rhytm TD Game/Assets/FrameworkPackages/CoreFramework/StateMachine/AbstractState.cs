
namespace CoreFramework.StateMachine
{
    public abstract class AbstractState
    {
        public bool StateIsActive { get; private set; }


        public virtual void EnterState()
        {
            StateIsActive = true;
        }

        public virtual void ExitState()
        {
            StateIsActive = false;
        }

        public abstract void Update(float deltaTime);
    }
}
