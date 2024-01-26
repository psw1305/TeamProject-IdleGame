using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBGM : AudioSystem<AudioBGM>
{
    #region Properties

    private AudioSource AudioSource { get; set; }

    private float volumeBGMScale = 1.0f;
    public float VolumeBGMScale
    {
        get
        {
            return this.volumeBGMScale;
        }
        set
        {
            this.volumeBGMScale = Mathf.Clamp01(value);
            SetVolume(this.volumeBGMScale);
        }
    }

    #endregion

    #region Sound Fields

    public AudioClip stage;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        this.AudioSource = GetComponent<AudioSource>();
    }

    protected override void SetVolume(float volumeScale)
    {
        //SetVolume("BGM", volumeScale);
        this.AudioSource.volume = volumeScale;
    }

    public void Play(AudioClip clip)
    {
        this.AudioSource.clip = clip;
        this.AudioSource.loop = true;
        this.AudioSource.Play();
    }
}
