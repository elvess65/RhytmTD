﻿
using System;
using System.Collections.Generic;

namespace RhytmTD.Core
{
    public class Dispatcher
    {
        private static Dispatcher m_Instance;
        private Dictionary<System.Type, BaseController> m_Controllers = new Dictionary<System.Type, BaseController>();
        private Dictionary<System.Type, BaseModel> m_Models = new Dictionary<Type, BaseModel>();

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
            System.Type type = typeof(T);

            T val = (T)Activator.CreateInstance(type, this);
            m_Controllers.Add(type, val);

            return val;
        }

        public T GetController<T>() where T : BaseController
        {
            System.Type type = typeof(T);
            BaseController controller = m_Controllers[type];

            return (T)controller;
        }

        public T CreateModel<T>() where T : BaseModel
        {
            System.Type type = typeof(T);

            T val = (T)Activator.CreateInstance(type, this);
            m_Models.Add(type, val);

            return val;
        }

        public T GetModel<T>() where T : BaseModel
        {
            System.Type type = typeof(T);
            BaseModel model = m_Models[type];

            return (T)model;
        }
    }
}