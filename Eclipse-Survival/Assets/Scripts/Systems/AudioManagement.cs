using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagement : MonoBehaviour
{
    // Singleton
    private static AudioManagement instance;
    public static AudioManagement Instance { get { return instance; } }

    // Audio Source (Registered by Start method)
    public AudioSource backgroundTrack1;
    public AudioSource backgroundTrack2;
    private bool isPlayingTrack1;

    public AudioSource chaseBackgroundMusic;
    public AudioSource xanderFootsteps;

    [Space]

    [Header("Set in Inspector: Audio Clips")]
    public AudioClip menuHoldBGM;
    public AudioClip primaryBGM;
    public AudioClip chaseBGM;   

    [Header("Footsteps")]
    public AudioClip[] xanderFootstepsWood;

    [Header("Ambiance")]
    public AudioClip[] ambiance;


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
        backgroundTrack1 = gameObject.AddComponent<AudioSource>();
        backgroundTrack2 = gameObject.AddComponent<AudioSource>();

        // Set to loop
        backgroundTrack1.loop = true;
        backgroundTrack2.loop = true;

        //
        backgroundTrack1.playOnAwake = false;
        backgroundTrack2.playOnAwake = false;

        SwitchBackgroundMusic(BackgroundMusicType.Menu);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            backgroundTrack2.clip = newClip;
            backgroundTrack2.Play();

            while (timeElapsed < fadeDuration)
            {
                backgroundTrack2.volume = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
                backgroundTrack1.volume = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            backgroundTrack1.Stop();
        }
        else
        {
            backgroundTrack1.clip = newClip;
            backgroundTrack1.Play();

            while (timeElapsed < fadeDuration)
            {
                backgroundTrack1.volume = Mathf.Lerp(0, 1, timeElapsed / fadeDuration);
                backgroundTrack2.volume = Mathf.Lerp(1, 0, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            backgroundTrack2.Stop();
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

    #endregion
}

public enum BackgroundMusicType
{
    Menu,
    Normal,
    UnderAttack
}
