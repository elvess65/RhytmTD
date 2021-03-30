using System;

namespace CoreFramework
{
    public class ApplicationModel : BaseModel
    {
        public event Action OnPause;
        public event Action OnResume;
        public bool IsPaused = false; 

        public void Paused()
        {
            OnPause?.Invoke();
            IsPaused = true;
        }

        public void Resumed()
        {
            OnResume?.Invoke();
            IsPaused = false;
        }
    }
}
