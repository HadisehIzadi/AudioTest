using System.Collections;
using System.Collections.Generic;
using E7.Native;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NativeAudioDemoStressTest : MonoBehaviour
{
    public AudioClip clip;
    public Text debugText;
    public AudioSource unitySource;
    public Graphic colorSwitch;

    private NativeAudioPointer pointer;
    private const int idleCycle = 100;
    private const int playsPerFrame = 5;

    private IEnumerator routine;

    public void StartTest()
    {
        StartTestCommon(playsPerFrame,1,1);
    }

    public void StartTestSlower()
    {
        StartTestCommon(1, 0, 3);
    }

    public void StartTestCommon(int playsPerFrame, int idleCycle, int waitFrame)
    {
        NativeAudio.Initialize();
        debugText.text = NativeAudio.GetDeviceAudioInformation().ToString();
        pointer = NativeAudio.Load(clip);

        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StressTestRoutine(playsPerFrame, idleCycle, waitFrame);
        StartCoroutine(routine);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("NativeAudioDemo-CustomIndex");
    }

    public void StartTestUnity()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StressTestUnityRoutine();
        StartCoroutine(routine);
    }

    /// <summary>
    /// Because Native Audio is not frame dependent, it is possible to delay the play in-frame by doing empty for loop.
    /// This is not possible with Unity audio as it would just collect all plays issued to be played at the same time at the end of frame.
    /// This stress test plays <paramref name="playsPerFrame"> times in one frame with <paramref name="idleCycle"> inbetween each, then wait <paramref name="waitFrame"> frame.
    /// </summary>
    IEnumerator StressTestRoutine(int playsPerFrame, int idleCycle, int waitFrame)
    {
        while (true)
        {
            colorSwitch.color = colorSwitch.color == Color.red ? Color.white : Color.red;
            for (int p = 0; p < playsPerFrame; p++)
            {
                NativeAudio.GetNativeSourceAuto().Play(pointer);
                for (int i = 0; i < idleCycle; i++) { }
            }
            for (int i = 0; i < waitFrame; i++)
            {
                yield return null;
            }
        }
    }

    IEnumerator StressTestUnityRoutine()
    {
        while (true)
        {
            unitySource.PlayOneShot(clip);
            yield return null;
        }
    }

    public void StopTest()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }

    public void ShowAndroidDebugDialog()
    {
#if UNITY_ANDROID
        var androidNativeAudio = new AndroidJavaClass("com.Exceed7.NativeAudio.NativeAudio");
        androidNativeAudio.CallStatic("DebugDialog");
#endif
    }
}
