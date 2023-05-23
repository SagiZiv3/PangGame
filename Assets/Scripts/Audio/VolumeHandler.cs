using EditorTools.AudioMixerAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Pang.Audio
{
    internal sealed class VolumeHandler : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        [SerializeField, AudioMixerGroup(nameof(audioMixer))]
        private string masterGroupName;

        [SerializeField, AudioMixerGroup(nameof(audioMixer))]
        private string backgroundGroupName;

        [SerializeField, AudioMixerGroup(nameof(audioMixer))]
        private string sfxGroupName;

        private bool masterVolumeOn = true;

        public void SetMasterVolume(float volumePercentage)
        {
            audioMixer.SetFloat(GetVolumeVariableName(masterGroupName), VolumePercentageToDb(volumePercentage));
        }

        public void SetBackgroundVolume(float volumePercentage)
        {
            audioMixer.SetFloat(GetVolumeVariableName(backgroundGroupName), VolumePercentageToDb(volumePercentage));
        }

        public void SeSfxVolume(float volumePercentage)
        {
            audioMixer.SetFloat(GetVolumeVariableName(sfxGroupName), VolumePercentageToDb(volumePercentage));
        }

        private static float VolumePercentageToDb(float linearValue) =>
            Mathf.Log10(Mathf.Max(linearValue, 0.0001f)) * 20f;

        // For this function, we assume that the naming convention is {groupName}Volume.
        private static string GetVolumeVariableName(string groupName) => $"{groupName}Volume";
    }
}