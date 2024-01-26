using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSFX : AudioSystem<AudioSFX>
{
    #region Properties

    private AudioSource AudioSource { get; set; }

    private float volumeSFXScale = 1.0f;
    public float VolumeSFXScale
    {
        get
        {
            return this.volumeSFXScale;
        }
        set
        {
            this.volumeSFXScale = Mathf.Clamp01(value);
            SetVolume(this.volumeSFXScale);
        }
    }

    #endregion

    #region Sound Fields

    public AudioClip uiClick;

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
