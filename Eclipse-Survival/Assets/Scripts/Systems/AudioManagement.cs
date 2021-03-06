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
    public float timeBetweenXanderFootstepsWalking;
    public float timeBetweenXanderFootstepsRunning;
    public float timeBetweenXanderSqueaks;
    public float timeBetweenCatSfx;
    public float timeBetweenCatAttackSfx;

    // Private controls
    private float backgroundMusicVolume;
    private float ambientSFXVolume;
    private float xanderFootstepsVolume;
    private float fryingPanVolume;
    private float xanderSqueaksVolume;
    private bool isPlayingAmbiantSound;
    private float catSFXVolume;
    private float xanderFallDamageVolume;
    private float xanderEatVolume;
    private float xanderTakeDamageVolume;
    private float spiderWebVolume;
    public float xanderDeathVolume;

    #region Public Controls
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

    public float FryingPanVolume
    {
        get { return fryingPanVolume; }
        set
        {
            fryingPanVolume = value;
            fryingPanChannel.volume = fryingPanVolume;
        }
    }

    public float XanderSqueaksVolume
    {
        get { return xanderSqueaksVolume; }
        set
        {
            xanderSqueaksVolume = value;
            xanderSqueaksChannel.volume = xanderSqueaksVolume;
        }
    }

    public float CatSFXVolume
    {
        get { return catSFXVolume; }
        set
        {
            catSFXVolume = value;
            catChannel.volume = catSFXVolume;
        }
    }

    public float XanderFallDamageVolume
    {
        get { return xanderFallDamageVolume; }
        set
        {
            xanderFallDamageVolume = value;
            xanderFallDamageChannel.volume = xanderFallDamageVolume;
        }
    }

    public float XanderEatVolume
    {
        get { return xanderEatVolume; }
        set
        {
            xanderEatVolume = value;
            xanderEatChannel.volume = xanderEatVolume;
        }
    }

    public float XanderTakeDamageVolume
    {
        get { return xanderTakeDamageVolume; }
        set
        {
            xanderTakeDamageVolume = value;
            xanderTakeDamageChannel.volume = xanderTakeDamageVolume;
        }
    }

    public float SpiderWebVolume
    {
        get { return spiderWebVolume; }
        set
        {
            spiderWebVolume = value;
            spiderWebChannel.volume = spiderWebVolume;
        }
    }

    public float XanderDeathVolume
    {
        get { return xanderDeathVolume; }
        set
        {
            xanderDeathVolume = value;
            xanderDeathChannel.volume = xanderDeathVolume;
        }
    }
    #endregion
    // Audio Source (Registered by Start method)
    public AudioSource backgroundMusicChannel1;
    public AudioSource backgroundMusicChannel2;
    public AudioSource ambienceChannel;
    public AudioSource xanderFootstepsChannel;
    public AudioSource fryingPanChannel;
    public AudioSource xanderSqueaksChannel;
    public AudioSource catChannel;
    public AudioSource xanderFallDamageChannel;
    public AudioSource xanderEatChannel;
    public AudioSource xanderTakeDamageChannel;
    public AudioSource spiderWebChannel;
    public AudioSource xanderDeathChannel;

    private bool isPlayingTrack1;

    private float timeBetweenAmbientSounds; // randomized
    private float timeRemaining;

    [Space]

    [Header("Set in Inspector: Audio Clips")]
    public AudioClip menuHoldBGM;
    public AudioClip primaryBGM;
    public AudioClip chaseBGM;   
    public AudioClip[] xanderFootstepsWoodClips;
    public AudioClip[] ambientSoundClips;
    public AudioClip[] fryingPanClips;
    public AudioClip[] xanderSqueaksClips;
    public AudioClip[] catSoundClips;
    public AudioClip[] catAttackingSoundClips;
    public AudioClip xanderFallDamageClip;
    public AudioClip xanderEatClip;
    public AudioClip xanderTakeDamageClip;
    public AudioClip spiderWebClip;
    public AudioClip xanderDeathClip;

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
        fryingPanChannel = gameObject.AddComponent<AudioSource>();
        xanderSqueaksChannel = gameObject.AddComponent<AudioSource>();
        catChannel = gameObject.AddComponent<AudioSource>();
        xanderFallDamageChannel = gameObject.AddComponent<AudioSource>();
        xanderEatChannel = gameObject.AddComponent<AudioSource>();
        xanderTakeDamageChannel = gameObject.AddComponent<AudioSource>();
        spiderWebChannel = gameObject.AddComponent<AudioSource>();
        xanderDeathChannel = gameObject.AddComponent<AudioSource>();

        // Set to loop
        backgroundMusicChannel1.loop = true;
        backgroundMusicChannel2.loop = true;
        ambienceChannel.loop = false;
        xanderFootstepsChannel.loop = false;
        fryingPanChannel.loop = false;
        xanderSqueaksChannel.loop = false;
        catChannel.loop = false;
        xanderFallDamageChannel.loop = false;
        xanderEatChannel.loop = false;
        xanderTakeDamageChannel.loop = false;
        spiderWebChannel.loop = false;
        xanderDeathChannel.loop = false;

        // Set play on awake to false
        backgroundMusicChannel1.playOnAwake = false;
        backgroundMusicChannel2.playOnAwake = false;
        ambienceChannel.playOnAwake = false;
        xanderFootstepsChannel.playOnAwake = false;
        fryingPanChannel.playOnAwake = false;
        xanderSqueaksChannel.playOnAwake = false;
        catChannel.playOnAwake = false;
        xanderFallDamageChannel.playOnAwake = false;
        xanderEatChannel.playOnAwake = false;
        xanderTakeDamageChannel.playOnAwake = false;
        spiderWebChannel.playOnAwake = false;
        xanderDeathChannel.playOnAwake = false;

        // ------- Initialize Voume Mix Properties ----------
        AmbientSFXVolume = 0.5f;
        XanderFootstepsVolume = 0.25f;
        FryingPanVolume = 0.4f;
        XanderSqueaksVolume = 0.01f;
        CatSFXVolume = 0.14f;
        xanderFallDamageVolume = 0.2f;
        xanderEatVolume = 0.02f;
        xanderTakeDamageVolume = 0.1f;
        spiderWebVolume = 0.07f;
        XanderDeathVolume = 0.2f;

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
        AudioClip currentClipPlaying;
        if (isPlayingTrack1)
        {
            currentClipPlaying = backgroundMusicChannel1.clip;
        }
        else
        {
            currentClipPlaying = backgroundMusicChannel2.clip;
        }

        switch (type)
        {
            case BackgroundMusicType.Menu:  
                if (currentClipPlaying != null)
                {
                    if (currentClipPlaying.name != menuHoldBGM.name)
                    {
                        SwapTrack(menuHoldBGM, 5.0f);
                    }                       
                }
                else
                {
                    SwapTrack(menuHoldBGM, 5.0f);
                }               
                             
                break;
            case BackgroundMusicType.Normal:
                if (currentClipPlaying != null)
                {
                    if (currentClipPlaying.name != primaryBGM.name)
                    {
                        SwapTrack(primaryBGM, 10.0f);
                    }
                }
                else
                {
                    SwapTrack(primaryBGM, 10.0f);
                }                           
                break;
            case BackgroundMusicType.UnderAttack:
                if (currentClipPlaying != null)
                {
                    if (currentClipPlaying.name != chaseBGM.name)
                    {
                        SwapTrack(chaseBGM, 0.1f);
                    }
                }
                else
                {
                    SwapTrack(chaseBGM, 0.1f);
                }            
                break;
        }
    }

    // --------------------------------XANDER FOOTSTEPS-------------------------------------
    bool canPlayXanderFootstep;
    bool canPlayXanderSqueak;
    float xanderFootstepTimer;
    float xanderSqueakTimer;
    public void PlayXanderSFX(ActionState state)
    {
        xanderFootstepTimer -= Time.deltaTime;
        xanderSqueakTimer -= Time.deltaTime;

        if (xanderFootstepTimer <= 0)
        {
            canPlayXanderFootstep = true;
        }

        if (xanderSqueakTimer <= 0)
        {
            canPlayXanderSqueak = true;
        }

        if (canPlayXanderFootstep) // play a footstep sfx
        {
            canPlayXanderFootstep = false;

            if (state == ActionState.Walking || state == ActionState.Climbing)
            {
                xanderFootstepTimer = timeBetweenXanderFootstepsWalking;
                int clipToPlay = (int)Random.Range(0, xanderFootstepsWoodClips.Length - 1);
                xanderFootstepsChannel.PlayOneShot(xanderFootstepsWoodClips[clipToPlay]);
            }
            if (state == ActionState.Running)
            {
                xanderFootstepTimer = timeBetweenXanderFootstepsRunning;
                int clipToPlay = (int)Random.Range(0, xanderFootstepsWoodClips.Length - 1);
                xanderFootstepsChannel.PlayOneShot(xanderFootstepsWoodClips[clipToPlay]);
            }                 
        }

        // ------------------------XANDER SQUEAKS ------------------------------------------ (not triggered when Xander is Idle)
        if (state != ActionState.Idle)
        {
            if (canPlayXanderSqueak) // play a squeak sfx
            {
                canPlayXanderSqueak = false;
                xanderSqueakTimer = timeBetweenXanderSqueaks;
                int clipToPlay = (int)Random.Range(0, xanderSqueaksClips.Length - 1);
                xanderSqueaksChannel.PlayOneShot(xanderSqueaksClips[clipToPlay]);
            }
        }
    }

    //------------------------------ FRYING PAN --------------------------------------------
    public void PlayFryingPanSFX() // Does not need a timer control here because the calling method is already timer controlled
    {
        int clipToPlay = (int)Random.Range(0, fryingPanClips.Length - 1);
        fryingPanChannel.PlayOneShot(fryingPanClips[clipToPlay]);       
    }

    //------------------------------ CAT ----------------------------------------------------
    bool canPlayCatSfx;
    float catSfxTimer;
    bool catState = false;
    public void PlayCatSFX(bool isAttacking, bool isInRoom)
    {
        catSfxTimer -= Time.deltaTime;
        if (catState != isAttacking && isAttacking == true)
        {
            catChannel.Stop();
            catSfxTimer = 0;
        }

        catState = isAttacking;

        if (catSfxTimer <= 0)
        {
            canPlayCatSfx = true;
        }

        if (isInRoom)
        {
            if (canPlayCatSfx)
            {
                canPlayCatSfx = false;

                if (isAttacking)
                {
                    catSfxTimer = timeBetweenCatAttackSfx;
                    int clipToPlay = (int)Random.Range(0, catAttackingSoundClips.Length - 1);
                    catChannel.PlayOneShot(catAttackingSoundClips[clipToPlay]);
                }
                else
                {
                    catSfxTimer = timeBetweenCatSfx;
                    int clipToPlay = (int)Random.Range(0, catSoundClips.Length - 1);
                    catChannel.PlayOneShot(catSoundClips[clipToPlay]);
                }
            }
        }       
    }

    //--------------------------- FALL DAMAGE -------------------------------------------
    public void PlayFallDamage()
    {
        xanderFallDamageChannel.PlayOneShot(xanderFallDamageClip);
    }

    //------------------------- EAT FOOD ------------------------------------------------
    public void PlayXanderEatFood()
    {
        xanderEatChannel.PlayOneShot(xanderEatClip);
    }
    
    //------------------------ TAKE DAMAGE ----------------------------------------------
    public void PlayXanderTakeDamage()
    {
        xanderTakeDamageChannel.PlayOneShot(xanderTakeDamageClip);
    }

    //---------------------- SPIDER WEB ------------------------------------------------
    public void PlaySpiderWeb()
    {
        spiderWebChannel.PlayOneShot(spiderWebClip);
    }

    //----------------------- XANDER DEATH ---------------------------------------------
    public void PlayXanderDeath()
    {
        xanderDeathChannel.PlayOneShot(xanderDeathClip);
    }

    #endregion

    public void ResetSoundsOnSceneChange()
    {
        SwitchBackgroundMusic(BackgroundMusicType.Normal);
    }
}

public enum BackgroundMusicType
{
    Menu,
    Normal,
    UnderAttack
}
