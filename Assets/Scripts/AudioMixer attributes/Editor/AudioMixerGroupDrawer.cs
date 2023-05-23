using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace EditorTools.AudioMixerAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AudioMixerGroupAttribute))]
    public sealed class AudioMixerGroupDrawer : PropertyDrawer
    {
        private static readonly ISet<SerializedPropertyType> SupportedTypes =
            new HashSet<SerializedPropertyType> { SerializedPropertyType.String };

        private const BindingFlags AudioMixerBindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!SupportedTypes.Contains(property.propertyType))
            {
                UpdatePropertyToInvalidValue(property);
                EditorGUI.LabelField(position, label.text,
                    ErrorMessages.UnsupportedPropertyType(property.propertyType));
                return;
            }

            var audioMixerGroupAttribute = (AudioMixerGroupAttribute)attribute;

            var targetObject = property.serializedObject.targetObject;
            // Find the AudioMixer field.
            var field = targetObject.GetType()
                .GetField(audioMixerGroupAttribute.AudioMixerName, AudioMixerBindingFlags);
            if (field == null)
            {
                UpdatePropertyToInvalidValue(property);
                EditorGUI.LabelField(position, label.text,
                    ErrorMessages.FieldNotFound(audioMixerGroupAttribute.AudioMixerName));
                return;
            }

            var audioMixer = field.GetValue(targetObject) as AudioMixer;
            if (audioMixer == null)
            {
                UpdatePropertyToInvalidValue(property);
                EditorGUI.LabelField(position, label.text,
                    ErrorMessages.AudioMixerNotAssigned(audioMixerGroupAttribute.AudioMixerName));
                return;
            }

            var groupsNames = audioMixer
                .FindMatchingGroups(string.Empty)
                .Select(group => group.name)
                .ToArray();

            int selection = GetSelectionIndex(property, groupsNames);
            // If the variables are not initialized yet, assign them the first parameter.
            if (selection < 0)
            {
                UpdatePropertyToNewSelection(property, groupsNames[0]);
                return;
            }

            position = EditorGUI.PrefixLabel(position, label);
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            selection = EditorGUI.Popup(position, selection, groupsNames);
            if (EditorGUI.EndChangeCheck())
            {
                UpdatePropertyToNewSelection(property, groupsNames[selection]);
            }

            EditorGUI.EndProperty();
        }

        private static int GetSelectionIndex(SerializedProperty property, string[] groupsNames)
        {
            string selectionName = property.stringValue;
            int index = Array.IndexOf(groupsNames, selectionName);
            return index;
        }

        private static void UpdatePropertyToNewSelection(SerializedProperty property, string selectionName)
        {
            property.stringValue = selectionName;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private static void UpdatePropertyToInvalidValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    property.stringValue = null;
                    break;
            }

            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }
    }
}