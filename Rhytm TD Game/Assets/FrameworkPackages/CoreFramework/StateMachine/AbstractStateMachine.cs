using CoreFramework.Abstract;
using System;
using System.Collections.Generic;

namespace CoreFramework.StateMachine
{
    public class AbstractStateMachine<T> : iUpdatable where T: AbstractState
    {
        protected T m_CurrentState;

        private Dictionary<Type, T> m_InitializedStates;


        public AbstractStateMachine()
        {
            m_InitializedStates = new Dictionary<Type, T>();
        }

        public virtual void PerformUpdate(float deltaTime)
        {
            m_CurrentState.PerformUpdate(deltaTime);
        }

        /// <summary>
        /// Инициализация начальным состоянием
        /// </summary>
        public virtual void Initialize<T>()
        {
            SetState(m_InitializedStates[typeof(T)]);
        }

        /// <summary>
        /// Переход в состояние
        /// </summary>
        public void ChangeState<T>() where T : AbstractState
        {
            ChangeState(m_InitializedStates[typeof(T)]);
        }

        /// <summary>
        /// Добавить состояние для перехода
        /// </summary>
        public void AddState(T state)
        {
            m_InitializedStates.Add(state.GetType(), state);
        }


        /// <summary>
        /// Выход из предыдущего состояния и вход в новое
        /// </summary>
        protected virtual void ChangeState(T state)
        {
            m_CurrentState.ExitState();
            SetState(state);
        }

        /// <summary>
        /// Вход в новое состояние
        /// </summary>
        /// <param name="state"></param>
        private void SetState(T state)
        {
            m_CurrentState = state;
            state.EnterState();
        }
    }
}
