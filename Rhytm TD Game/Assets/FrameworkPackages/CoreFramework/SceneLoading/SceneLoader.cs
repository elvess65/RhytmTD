using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreFramework.SceneLoading
{
    public partial class SceneLoader : BaseView
    {
        public System.Action OnSceneLoadingComplete;
        public System.Action OnSceneUnloadingComplete;

        private string m_CurrentLoadingLevel = string.Empty;
        private string m_CurrentUnloadingLevel = string.Empty;

        private void Awake()
        {
            
        }

        public SceneLoader()
        {
            OnSceneLoadingComplete += SceneLoadCompleteHandler;

            PartialConstructorCall();
        }

        partial void PartialConstructorCall();


        public void LoadLevel(string levelName, bool noFade = false)
        {
            m_CurrentLoadingLevel = levelName;

            if (!noFade && SceneLoadingManager.Instance != null)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnLoadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else
                Load(levelName);
        }

        private void FadeInFinishedOnLoadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnLoadHandler;
            Load(m_CurrentLoadingLevel);
        }

        private void Load(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            if (asyncOperation != null)
                asyncOperation.completed += LoadOperationComplete;
            else
                Debug.LogError($"Unable to load level {levelName}");
        }

        private void LoadOperationComplete(AsyncOperation asyncOperation)
        {
            OnSceneLoadingComplete?.Invoke();
        }

        partial void SceneLoadCompleteHandler();


        public void UnloadLevel(string levelName, bool noFade = false)
        {
            m_CurrentUnloadingLevel = levelName;

            if (!noFade)
            {
                SceneLoadingManager.Instance.OnFadeIn += FadeInFinishedOnUnloadHandler;
                SceneLoadingManager.Instance.FadeIn();
            }
            else
            {
                ExecuteSceneUnloadTransition();
            }
        }

        private void FadeInFinishedOnUnloadHandler()
        {
            SceneLoadingManager.Instance.OnFadeIn -= FadeInFinishedOnUnloadHandler;
            ExecuteSceneUnloadTransition();
        }

        private void ExecuteSceneUnloadTransition()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_BOOT_SCENE_NAME));
            Unload(m_CurrentUnloadingLevel);
        }

        private void Unload(string levelName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(levelName);
            if (asyncOperation != null)
                asyncOperation.completed += UnloadOperationComplete;
            else
                Debug.LogError($"Unable to unload level {levelName}");
        }

        private void UnloadOperationComplete(AsyncOperation asyncOperation)
        {
            OnSceneUnloadingComplete?.Invoke();
        }
    }
}
