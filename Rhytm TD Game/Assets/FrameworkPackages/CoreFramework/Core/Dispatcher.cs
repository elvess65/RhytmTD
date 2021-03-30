using System;
using System.Collections.Generic;

namespace CoreFramework
{
    public class Dispatcher
    {
        private static Dispatcher m_Instance;
        private Dictionary<Type, BaseController> m_Controllers = new Dictionary<Type, BaseController>();
        private Dictionary<Type, BaseModel> m_Models = new Dictionary<Type, BaseModel>();

        private List<IDisposable> m_Disposables = new List<IDisposable>();
        private List<IDisposable> m_RemovedDisposables = new List<IDisposable>();

        public static Dispatcher Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Dispatcher();

                return m_Instance;
            }
        }

        private Dispatcher()
        {
        }

        public T CreateController<T>() where T : BaseController
        {
            Type type = typeof(T);

            T val = (T)Activator.CreateInstance(type, this);
            m_Controllers.Add(type, val);

            return val;
        }

        public T GetController<T>() where T : BaseController
        {
            Type type = typeof(T);
            BaseController controller = m_Controllers[type];

            return (T)controller;
        }

        public T CreateModel<T>() where T : BaseModel
        {
            Type type = typeof(T);
            T val = (T)Activator.CreateInstance(type);

            return CreateModel(val);
        }

        public T CreateModel<T>(T value) where T : BaseModel
        {
            m_Models.Add(value.GetType(), value);
            value.Initialize();

            return value;
        }

        public T GetModel<T>() where T : BaseModel
        {
            Type type = typeof(T);

            try
            {
                BaseModel model = m_Models[type];

                return (T)model;
            }
            catch(KeyNotFoundException)
            {
                UnityEngine.Debug.LogError($"Can't find model of type {type}");
                throw new KeyNotFoundException();
            }
        }

        public void InitializeComplete()
        {
            foreach (BaseController controller in m_Controllers.Values)
            {
                controller.InitializeComplete();
            }
        }

        public void AddDisposable(IDisposable disposable)
        {
            m_Disposables.Add(disposable);
        }

        public void RemoveDisposable(IDisposable disposable)
        {
            if (m_Disposables.Contains(disposable))
                m_RemovedDisposables.Remove(disposable);
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in m_Disposables)
                disposable.Dispose();

            for (int i = 0; i < m_RemovedDisposables.Count; i++)
            {
                if (m_Disposables.Contains(m_RemovedDisposables[i]))
                    m_Disposables.Remove(m_RemovedDisposables[i]);
            }

            m_RemovedDisposables.Clear();
        }
    }
}