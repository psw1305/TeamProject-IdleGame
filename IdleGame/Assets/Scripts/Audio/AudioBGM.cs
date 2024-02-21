using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBGM : AudioSystem<AudioBGM>
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

    protected override void SetVolume(float volumeScale)
    {
        SetVolume("BGM", volumeScale);
    }

    public void Play(AudioClip clip)
    {
        this.AudioSource.clip = clip;
        this.AudioSource.loop = true;
        this.AudioSource.Play();
    }
}
