// thank you my GOAT Ethan Yoshino for this code 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public EventInstance currentPlaying;
    protected PLAYBACK_STATE playbackState;

    private EventInstance currentLetterSFX;


    // Regions here to collapse code
    #region VOLUME CONTROL
    [Header("Volumes (sliders)")]
    [Range(0f, 1f)]
    public float masterVolume = 0.5f;
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    [Range(0f, 1f)]
    public float sfxVolume = 0.5f;

    // [Header("Bus Paths")]
    private string masterBusPath = "bus:/";
    private string musicBusPath = "bus:/Music";
    private string sfxBusPath = "bus:/SFX";

    //BUSES
    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    #endregion

    #region OST PATHS
    // private string masterOST = "event:/BGM";
    #endregion

    #region SFX PATHS
    private string playerShootSFX = "event:/shootingaud";
    private string playerTakeDamageSFX  = "event:/takedamage3aud";
    private string punchingSFX = "event:/punchingaud";
    #endregion

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        if (!RuntimeManager.HasBankLoaded("Master"))
        {
            RuntimeManager.LoadBank("Master");
            Debug.Log("Master Bank Loaded");
        }

        masterBus = RuntimeManager.GetBus(masterBusPath);
        musicBus = RuntimeManager.GetBus(musicBusPath);
        sfxBus = RuntimeManager.GetBus(sfxBusPath);
        #if UNITY_WEBGL
                SceneManager.sceneUnloaded += (_) => PauseOST();
                SceneManager.sceneLoaded += (_, _) => UnpauseOST();
        #endif
    }


    /// <summary>
    /// Play any sfx or other sound that isn't music.
    /// </summary>
    /// <param name="path">The path to the sound.</param>
    public void Play(string path)
    {
        FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(path, out _);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogWarning("FMOD event path does not exist: " + path);
            return;
        }

        EventInstance instance = RuntimeManager.CreateInstance(path);
        instance.start();
        instance.release();
    }


    #region SFX Functions
       public void PlayPlayerShootSFX() { Play(playerShootSFX); }
       public void PlayPlayerTakeDamageSFX() { Play(playerTakeDamageSFX); }
       public void PlayPunchingSFX() { Play(punchingSFX); }
    #endregion


}
