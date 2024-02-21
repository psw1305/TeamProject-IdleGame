using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSFX : AudioSystem<AudioSFX>
{
    #region Properties

    private AudioSource AudioSource { get; set; }

    private float volumeScale = 1.0f;
    public float VolumeScale
    {
        get
        {
            return this.volumeScale;
        }
        set
        {
            this.volumeScale = Mathf.Clamp01(value);
            SetVolume(this.volumeScale);
        }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        this.AudioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1.0f)
    {
        this.AudioSource.PlayOneShot(clip, volumeScale);
    }

    public void Play(AudioClip clip, float pitch)
    {
        this.AudioSource.pitch = pitch;
        this.AudioSource.PlayOneShot(clip);
    }

    protected override void SetVolume(float volumeScale)
    {
        SetVolume("SFX", volumeScale);
    }
}
