using EditorTools.SerializedReferenceInitializer.Attributes;
using UnityEngine;

namespace Pang.NumericVariables
{
    [GenerateWrapperFor(typeof(IInitializable))]
    [CreateAssetMenu(menuName = "Numeric Variables/Create Float Variable", fileName = "FloatVariable")]
    internal sealed class FloatVariable : NumericVariable<float>
    {
        protected override float PreformAddition(float value, float amount)
        {
            return value + amount;
        }

        protected override float PreformReduction(float value, float amount)
        {
            return value - amount;
        }
    }
}