using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioSystem<T> : BehaviourSingleton<T> where T : AudioSystem<T>
{
    [SerializeField] private AudioMixer audioMixer = null;

    protected abstract void SetVolume(float volumeScale);

    protected void SetVolume(string volumeParameterName, float volumeScale)
    {
        volumeScale = Mathf.Clamp01(volumeScale);

        float min = 0.0001f;    // 0.0001 = -80dB
        float max = 1;          // 1 = 0dB

        // Scale Volume
        float linearValue = Mathf.Lerp(min, max, volumeScale);
        // Convert volume to dB
        float dBValue = 20 * Mathf.Log10(linearValue);

        audioMixer.SetFloat(volumeParameterName, dBValue);
    }
}
