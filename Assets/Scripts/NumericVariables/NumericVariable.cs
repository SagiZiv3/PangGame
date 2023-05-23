using System;
using UnityEngine;

namespace Pang.NumericVariables
{
    internal abstract class NumericVariable<T> : ScriptableObject, IInitializable
    {
        public event Action<T> OnValueChanged;
        [SerializeField] private T initialValue;
        [SerializeField] private bool sendInitialValue;
        private bool fireEvent = true;
        public T Value { get; private set; }

        public void Initialize()
        {
            UpdateCurrentValue(initialValue, sendInitialValue);
            fireEvent = true;
        }

        public void Initialize(T customInitialValue)
        {
            UpdateCurrentValue(customInitialValue, sendInitialValue);
            fireEvent = true;
        }

        public void Add(T amount)
        {
            T newValue = PreformAddition(Value, amount);
            UpdateCurrentValue(newValue, fireEvent);
        }

        public void Reduce(T amount)
        {
            T newValue = PreformReduction(Value, amount);
            UpdateCurrentValue(newValue, fireEvent);
        }

        public void BeginModification()
        {
            fireEvent = false;
        }

        public void EndModification()
        {
            fireEvent = true;
            UpdateCurrentValue(Value, true);
        }

        protected abstract T PreformAddition(T value, T amount);

        protected abstract T PreformReduction(T value, T amount);

        private void UpdateCurrentValue(T newValue, bool invokeEvent)
        {
            Value = newValue;
            if (invokeEvent)
                OnValueChanged?.Invoke(newValue);
        }
    }
}