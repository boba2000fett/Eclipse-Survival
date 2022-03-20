using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagement : MonoBehaviour
{
    // Singleton
    private static AudioManagement instance;
    public static AudioManagement Instance { get { return instance; } }

    // Public Controls
    [Header("Set in Inspector")]
    public float timeBetweenXanderFootsteps;

    // Private controls
    private float backgroundMusicVolume;
    private float ambientSFXVolume;
    private float xanderFootstepsVolume;
    private bool isPlayingAmbiantSound;

    // Public Controls
    public float BackgroundMusicVolume
    {
        get { return backgroundMusicVolume; }
        set
        {
            backgroundMusicVolume = value;
            backgroundMusicChannel1.volume = backgroundMusicVolume;
            backgroundMusicChannel2.volume = backgroundMusicVolume;
        }
    }

    public float AmbientSFXVolume
    {
        get { return ambientSFXVolume; }
        set
        {
            ambientSFXVolume = value;
            ambienceChannel.volume = ambientSFXVolume;
        }
    }

    public float XanderFootstepsVolume
    {
        get { return xanderFootstepsVolume; }
        set
        {
            xanderFootstepsVolume = value;
            xanderFootstepsChannel.volume = xanderFootstepsVolume;
        }
    }

    // Audio Source (Registered by Start method)
    public AudioSource backgroundMusicChannel1;
    public AudioSource backgroundMusicChannel2;
    public AudioSource ambienceChannel;
    public AudioSource xanderFootstepsChannel;

    private bool isPlayingTrack1;

    private float timeBetweenAmbientSounds; // randomized
    private float timeRemaining;

    [Space]

    [Header("Set in Inspector: Audio Clips")]
    public AudioClip menuHoldBGM;
    public AudioClip primaryBGM;
    public AudioClip chaseBGM;   
    public AudioClip[] xanderFootstepsWood;
    public AudioClip[] ambientSoundClips;


    // Awake is called before Start()
    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;            
        }
        DontDestroyOnLoad(this);      
    }

    // Start is called before the first frame update
    void Start()
    {
        isPlayingTrack1 = true;

        // Register Audio Sources
        backgroundMusicChannel1 = gameObject.AddComponent<AudioSource>();
        backgroundMusicChannel2 = gameObject.AddComponent<AudioSource>();
        ambienceChannel = gameObject.AddComponent<AudioSource>();
        xanderFootstepsChannel = gameObject.AddComponent<AudioSource>();

        // Set to loop
        backgroundMusicChannel1.loop = true;
        backgroundMusicChannel2.loop = true;
        ambienceChannel.loop = false;
        xanderFootstepsChannel.loop = false;

        // Set play on awake to false
        backgroundMusicChannel1.playOnAwake = false;
        backgroundMusicChannel2.playOnAwake = false;
        ambienceChannel.playOnAwake = false;
        xanderFootstepsChannel.playOnAwake = false;

        // ------- Initialize Voume Mix Properties ----------
        AmbientSFXVolume = 0.5f;
        XanderFootstepsVolume = 0.5f;

        // -------- Fine Tune other AudioSource Attributes ----------
        xanderFootstepsChannel.pitch = 1.8f;

        SwitchBackgroundMusic(BackgroundMusicType.Menu);
        StartCoroutine(PlayAmbientSounds());
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isLoadingLevel && isPlayingAmbiantSound)
        {
            isPlayingAmbiantSound = false;
        }
        else if (!isPlayingAmbiantSound)
        {
            StartCoroutine(PlayAmbientSounds());
        }
    }

    private IEnumerator PlayAmbientSounds()
    {
        isPlayingAmbiantSound = true;
        timeBetweenAmbientSounds = Random.Range(10f, 25f);
        timeRemaining = timeBetweenAmbientSounds;

        yield return new WaitForSeconds(timeBetweenAmbientSounds);

        int clipToPlay = (int)Random.Range(0, ambientSoundClips.Length - 1);
        float volume = Random.Range(0.3f, 0.6f);
        float pan = Random.Range(-1.0f, 1.0f);
        ambienceChannel.panStereo = pan;
        ambienceChannel.PlayOneShot(ambientSoundClips[clipToPlay], volume);

        StopAllCoroutines();
        StartCoroutine(PlayAmbientSounds()); // kick of next iteration
    }

    private void SwapTrack(AudioClip newClip, float fadeDuration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip, fadeDuration));
    }

    private IEnumerator FadeTrack(AudioClip newClip, float fadeDuration)
    {
        float timeElapsed = 0;

        if (!isPlayingTrack1)
        {
            backgroundMusicChannel2.clip = newClip;
            backgroundMusicChannel2.Play();

            while (timeElapsed < fadeDuration)
            {
                backgroundMusicChannel2.volume = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
                backgroundMusicChannel1.volume = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            backgroundMusicChannel1.Stop();
        }
        else
        {
            backgroundMusicChannel1.clip = newClip;
            backgroundMusicChannel1.Play();

            while (timeElapsed < fadeDuration)
            {
                backgroundMusicChannel1.volume = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
                backgroundMusicChannel2.volume = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            backgroundMusicChannel2.Stop();
        }

        isPlayingTrack1 = !isPlayingTrack1;
    }

    #region Public Methods
    public void SwitchBackgroundMusic(BackgroundMusicType type)
    {
        switch (type)
        {
            case BackgroundMusicType.Menu:
                SwapTrack(menuHoldBGM, 5.0f);
                break;
            case BackgroundMusicType.Normal:
                SwapTrack(primaryBGM, 3.0f);
                break;
        }
    }

    // --------------------------------XANDER FOOTSTEPS-------------------------------------
    bool canPlayXanderFootstep;
    float xanderFootstepTimer;
    public void PlayXanderFootsteps()
    {
        xanderFootstepTimer -= Time.deltaTime;
        if (xanderFootstepTimer <= 0)
        {
            canPlayXanderFootstep = true;
        }

        if (canPlayXanderFootstep) // play a footstep
        {
            xanderFootstepTimer = timeBetweenXanderFootsteps;
            canPlayXanderFootstep = false;
            int clipToPlay = (int)Random.Range(0, xanderFootstepsWood.Length - 1);
            xanderFootstepsChannel.PlayOneShot(xanderFootstepsWood[clipToPlay]);
        }
    }

    #endregion
}

public enum BackgroundMusicType
{
    Menu,
    Normal,
    UnderAttack
}