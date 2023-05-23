using EditorTools.SerializedReferenceInitializer.Attributes;
using UnityEngine;

namespace Pang.NumericVariables
{
    [GenerateWrapperFor(typeof(IInitializable))]
    [CreateAssetMenu(menuName = "Numeric Variables/Create Int Variable", fileName = "IntVariable")]
    internal sealed class IntVariable : NumericVariable<int>
    {
        protected override int PreformAddition(int value, int amount)
        {
            return value + amount;
        }

        protected override int PreformReduction(int value, int amount)
        {
            return value - amount;
        }
    }
}