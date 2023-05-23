using System;
using UnityEngine;

namespace EditorTools.AudioMixerAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class AudioMixerGroupAttribute : PropertyAttribute
    {
        public string AudioMixerName { get; }

        public AudioMixerGroupAttribute(string audioMixerName)
        {
            AudioMixerName = audioMixerName;
        }
    }
}