// Native Audio
// 5argon - Exceed7 Experiments
// Problems/suggestions : 5argon@exceed7.com

// BONUS ! For owner of Native Touch (http://exceed7.com/native-touch/)
// If you uncomment this + add reference to E7.NativeTouch on the NativeAudio's asmdef you can see how they work together!
// The red "Stop latest play" button will instead activate NativeTouch and disable all Unity touch. 
// From this point, any touch on the screen will be equal to touching the right side. (To reset you have to restart the app)
// But the touch has been speed up! So we can get even less perceived latency.
// #define NATIVE_TOUCH_INTEGRATION

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

using E7.Native;
using UnityEngine.SceneManagement;

public class NativeAudioDemo : MonoBehaviour {

#pragma warning disable 0649
    [SerializeField] private AudioSource audioSource;

    [Header("Native Audio now works with AudioClip from Unity's importer!")]
    [SerializeField] private AudioClip nativeClip1;
    [SerializeField] private AudioClip nativeClip2;

    [Space]

    [SerializeField] private Text fpsText;
    [SerializeField] private Text playbackTimeText;
    [SerializeField] private Text descriptionText;

    [Tooltip("Repeatedly play native audio every frame after the scene start without any input. Useful for remote testing on Firebase Test Lab, etc.")]
    [SerializeField] private bool autoRepeat;

    [Space]

    [SerializeField] private Text bufferSizeText;
    [SerializeField] private Slider bufferSizeSlider;

    [SerializeField] private Text estimatedLatencyText;
#pragma warning restore 0649

    private NativeAudioPointer latestLoadedAudioPointer;
    private NativeSource latestUsedNativeSource;

    public void Start()
    {
        if (NotRealDevice()) return;

        AudioSettings.OnAudioConfigurationChanged += (bool changed) =>
        {
            Debug.Log("Audio configuration change detected! (Changed : " + changed + " )");
        };
        
        Application.targetFrameRate = 60;
        string unityOutputSampleRateString = "Unity output sample rate : " + AudioSettings.outputSampleRate;

        DeviceAudioInformation deviceAudioInformation = NativeAudio.GetDeviceAudioInformation();
        descriptionText.text = descriptionText.text + "\n" + unityOutputSampleRateString + "\n" + deviceAudioInformation.ToString();

#if UNITY_ANDROID
        var optimalBufferSize = deviceAudioInformation.optimalBufferSize;
        bufferSizeSlider.value = optimalBufferSize;
        bufferSizeText.text = optimalBufferSize.ToString();
#else
        bufferSizeSlider.value = 0;
        bufferSizeText.text = "---";
#endif

        if(autoRepeat)
        {
            Initialize();
            LoadAudio1();
            StartCoroutine(RepeatedPlayRoutine());
        }
    }

    IEnumerator RepeatedPlayRoutine()
    {
        while (true)
        {
            PlayAudio1();
            //PlayUnityAudioSource();
            yield return null;
        }
    }

    public void Update()
    {
        fpsText.text = (1/Time.deltaTime).ToString("0.00");
    }

    public void UpdateBufferSizeText()
    {
#if UNITY_ANDROID
        bufferSizeText.text = Mathf.RoundToInt(bufferSizeSlider.value).ToString();
#endif
    }

    public void Initialize()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
#else
        NativeAudio.Initialize(new NativeAudio.InitializationOptions { 
            androidAudioTrackCount = 2, 
            androidMinimumBufferSize = Mathf.RoundToInt(bufferSizeSlider.value) 
        });
#endif
    }

    public void NextScene()
    {
        SceneManager.LoadScene("NativeAudioDemo-StressTest");
    }

    public void DisposeNativeAudio()
    {
        NativeAudio.Dispose();
    }

    public void PlayUnityAudioSource()
    {
        audioSource.Play();
    }

    public void LoadAudio1()
    {
        LoadAudio(nativeClip1);
    }

    public void LoadAudio2()
    {
        LoadAudio(nativeClip2);
    }

    public void LoadAudioSA()
    {
        if (NotRealDevice()) return;

        //Loading via ScriptableAssets will have to ask Java to read out the file, then send that audio byte array to C.
        UnloadIfLoaded();
		latestLoadedAudioPointer = NativeAudio.Load("NativeAudioDemoSA.wav");
        Debug.Log("Loaded audio of length " + latestLoadedAudioPointer.Length + " from ScriptableAssets");
    }

    /// <summary>
    /// If you overwrite the pointer without unloading, you lose a way of unloading that audio permanently!
    /// </summary>
    private void UnloadIfLoaded()
    {
        if(latestLoadedAudioPointer != null)
        {
            latestLoadedAudioPointer.Unload();
            latestUsedNativeSource = default(NativeSource);
        }
    }

#if NATIVE_TOUCH_INTEGRATION
    /// <summary>
    /// On Android this is on a different thread, but because Native Audio does not care about thread it works beautifully! Fast!
    /// </summary>
    public static void NTCallback(NativeTouchData ntd)
    {
        if(ntd.WarmUpTouch)
        {
            Debug.Log("Warm up");
            return;
        }
        if(ntd.Phase == TouchPhase.Began)
        {
            staticPointer.Play();
        }
    }
    public static NativeAudioPointer staticPointer;
#endif

    public bool NotRealDevice()
    {
#if UNITY_EDITOR
        //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
        Debug.Log("Please try this in a real device!");
        return true;
#else
        return false;
#endif
    }

    public void StopLatestPlay()
    {
#if NATIVE_TOUCH_INTEGRATION
        Debug.Log("Native touch started!!");
        staticPointer = nativeAudioPointer; 
        NativeTouch.RegisterCallback(NTCallback);
        NativeTouch.Start(new NativeTouch.StartOption { disableUnityTouch = true });
        NativeTouch.WarmUp();
        return;
#endif
        if (NotRealDevice()) return;
        if (latestUsedNativeSource.IsValid)
        {
            latestUsedNativeSource.Stop();
        }
    }

    private void LoadAudio(AudioClip ac)
    {
        if (NotRealDevice()) return;

        //Loading via AudioClip will pull out and send the audio byte array to C side directly.
        UnloadIfLoaded();
		latestLoadedAudioPointer = NativeAudio.Load(ac);
        Debug.Log("Loaded audio of length "  + latestLoadedAudioPointer.Length);
    }

    public void PlayAudio1()
    {
        if (NotRealDevice()) return;
        latestUsedNativeSource = NativeAudio.GetNativeSourceAuto();
        latestUsedNativeSource.Play(latestLoadedAudioPointer);
    }

    public void PlayAudio2()
    {
        if (NotRealDevice()) return;
        var options = NativeSource.PlayOptions.defaultOptions;
        options.volume = 0.3f;
        options.pan = 1f;
        latestUsedNativeSource = NativeAudio.GetNativeSourceAuto();
        latestUsedNativeSource.Play(latestLoadedAudioPointer, options);
    }

    public void PlayAudio3()
    {
        if (NotRealDevice()) return;
        var options = NativeSource.PlayOptions.defaultOptions;
        options.volume = 0.5f;
        options.sourceLoop = true;
        latestUsedNativeSource = NativeAudio.GetNativeSourceAuto();
        latestUsedNativeSource.Play(latestLoadedAudioPointer, options);
    }

    NativeSource preparedSource;
    public void Prepare()
    {
        //get any native source and prepare on it.
        if (NotRealDevice()) return;
        preparedSource = NativeAudio.GetNativeSourceAuto();
        preparedSource.Prepare(latestLoadedAudioPointer);
    }

    public void PlayPrepared()
    {
        if (NotRealDevice()) return;
        if (preparedSource.IsValid)
        {
            preparedSource.PlayPrepared(); //Blind play!
        }
    }

    public void TrackPause()
    {
        if (latestUsedNativeSource.IsValid)
        {
            latestUsedNativeSource.Pause();
        }
    }

    /// <summary>
    /// It can fail to resume if an underlying native source has already been replaced with 
    /// other audio.
    /// </summary>
    public void TrackResume()
    {
        if (NotRealDevice()) return;
        if(latestUsedNativeSource.IsValid)
        {
            latestUsedNativeSource.Resume();
        }
    }

    float rememberedTime;

    /// <summary>
    /// An another strategy to do pause/resume. This one is resistant to
    /// a native source being replaced by other audio.
    /// </summary>
    public void RememberPause()
    {
        if (NotRealDevice()) return;
        if (latestUsedNativeSource.IsValid)
        {
            rememberedTime = latestUsedNativeSource.GetPlaybackTime();
            latestUsedNativeSource.Stop();
            Debug.Log("Pause and remembered time " + rememberedTime);
        }
    }

    /// <summary>
    /// An another strategy to do pause/resume. This one is resistant to
    /// a native source being replaced by other audio.
    /// </summary>
    public void RememberResume()
    {
        if (NotRealDevice()) return;
        if (latestLoadedAudioPointer != null)
        {
            var options = NativeSource.PlayOptions.defaultOptions;
            options.offsetSeconds = rememberedTime;
            latestUsedNativeSource = NativeAudio.GetNativeSourceAuto();
            latestUsedNativeSource.Play(latestLoadedAudioPointer, options);
            Debug.Log("Resume from time " + rememberedTime);
        }
    }

    public void Unload()
    {
        if (NotRealDevice()) return;
        latestLoadedAudioPointer.Unload();
    }

    public void GetPlaybackTime()
    {
        if (NotRealDevice()) return;
        if (latestUsedNativeSource.IsValid) 
        {
            playbackTimeText.text = latestUsedNativeSource.GetPlaybackTime().ToString();
        }
    }

}
