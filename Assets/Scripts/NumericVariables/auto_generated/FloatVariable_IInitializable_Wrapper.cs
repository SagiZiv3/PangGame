using System;
using EditorTools.SerializedReferenceInitializer.Attributes;
using UnityEngine;

namespace Pang.NumericVariables.auto_generated
{
    [Serializable()]
    [AutoGeneratedWrapper(typeof(Pang.NumericVariables.FloatVariable))]
    internal sealed class FloatVariable_IInitializable_Wrapper : Pang.IInitializable
    {
        
        [SerializeField()]
        private Pang.NumericVariables.FloatVariable _instance;
        
        private FloatVariable_IInitializable_Wrapper(Pang.NumericVariables.FloatVariable instance)
        {
            this._instance = instance;
        }
        
        public void Initialize()
        {
            this._instance.Initialize();
        }
    }
}
