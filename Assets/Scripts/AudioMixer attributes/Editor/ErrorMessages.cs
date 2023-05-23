using UnityEditor;

namespace EditorTools.AudioMixerAttributes.Editor
{
    internal static class ErrorMessages
    {
        public static string UnsupportedPropertyType(SerializedPropertyType propertyType) =>
            $"Unsupported field type: {propertyType}.";

        public static string FieldNotFound(string fieldName) =>
            $"Unable to find field named \"{fieldName}\".";

        public static string AudioMixerNotAssigned(string audioMixerName) =>
            $"AudioMixer \"{audioMixerName}\" is not assigned.";

    }
}