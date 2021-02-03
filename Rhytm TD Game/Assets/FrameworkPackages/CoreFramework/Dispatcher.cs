using System;
using System.Collections.Generic;
using RhytmTD.Data.Models;
using UnityEngine;

namespace CoreFramework
{
    public class Dispatcher
    {
        private static Dispatcher m_Instance;
        private Dictionary<Type, BaseController> m_Controllers = new Dictionary<Type, BaseController>();
        private Dictionary<Type, BaseModel> m_Models = new Dictionary<Type, BaseModel>();

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
            m_Models.Add(type, val);

            return val;
        }

        public T CreateModelFromJson<T>(string json) where T : BaseModel
        {
            Type type = typeof(T);

            T val = JsonUtility.FromJson<T>(json);
            m_Models.Add(type, val);

            return val;
        }


        public T GetModel<T>() where T : BaseModel
        {
            Type type = typeof(T);
            BaseModel model = m_Models[type];

            return (T)model;
        }

        public void InitializeComplete()
        {
            foreach (BaseController controller in m_Controllers.Values)
            {
                controller.InitializeComplete();
            }
        }
    }
}