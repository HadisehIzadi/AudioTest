using System.Collections;
using E7.Native;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NativeAudioDemoCustomIndex : MonoBehaviour
{
    public AudioClip test1;
    public AudioClip test2;
    public AudioClip loop;

    public NativeAudioPointer test1Pointer;
    public NativeAudioPointer test2Pointer;
    public NativeAudioPointer loopPointer;

    public IEnumerator Start()
    {
        if (NativeAudio.OnSupportedPlatform == false) yield break;
        NativeAudio.Dispose();
        var option = NativeAudio.InitializationOptions.defaultOptions;

        //In this demo native source index 0 is for loop
        //Index 1,2 is for small sound that should sounds like it is able to play over itself. (limited concurrency)
        //Index 3 is for cymbal-like sound that cuts over itself, mimic the real cymbal.
        option.androidAudioTrackCount = 4;

        //The dispose above followed by this initialize could cause problem on some phones.
        //You will get non-fast track if you previously holding too many fast ones, because
        //it need more time to make the fast track available again after release. The easiest
        //way to reproduce is to press Next Scene until it came back to this scene again.

        //So! This little wait will help that.

        yield return new WaitForSeconds(0.5f);

        NativeAudio.Initialize(option);

        test1Pointer = NativeAudio.Load(test1);
        test2Pointer = NativeAudio.Load(test2);
        loopPointer = NativeAudio.Load(loop);
    }

    private NativeSource loopSource;
    public void StartLoop()
    {
        if (NativeAudio.Initialized == false) return;
        //The loop is fixed to use index 0, other sound will also try to avoid using this index.
        var playOptions = NativeSource.PlayOptions.defaultOptions;
        playOptions.sourceLoop = true;
        loopSource = NativeAudio.GetNativeSource(0);
        loopSource.Play(loopPointer, playOptions);
    }

    public void StopLoop()
    {
        if (NativeAudio.Initialized == false) return;
        if (loopSource.IsValid)
        {
            loopSource.Stop();
        }
    }

    private class OneTwoAlternate : INativeSourceSelector
    {
        bool first = false;
        public int NextNativeSourceIndex()
        {
            first = !first;
            return first ? 1 : 2;
        }
    }

    OneTwoAlternate oneTwoAlternateSelector = new OneTwoAlternate();
    public void OneShot1()
    {
        if (NativeAudio.Initialized == false) return;
        //This play uses index 1 2 1 2 1 2 ... alternately so that it do not disturb the loop at index 0.
        //While allowing 1 more concurrency over itself. For short sound, only this is good enough.
        var sourceOneOrTwo = NativeAudio.GetNativeSourceAuto(oneTwoAlternateSelector);
        sourceOneOrTwo.Play(test1Pointer);
    }

    public void OneShot2()
    {
        if(NativeAudio.Initialized == false) return;
        //This play uses exclusively the 3rd source. Resulting in this specific sound cutting over itself.
        //It is not entirely a bad thing, for "cleaner mix" maybe it works better this way.

        //Not to mention that some sound naturally exhibit this behaviour, like cymbal crash where when you hit it
        //again it would have to stop the current vibration first.

        //Chaining like this is OK, but you should cache it if possible.
        NativeAudio.GetNativeSource(3).Play(test2Pointer);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("NativeAudioDemo-TestPlayer");
    }

}
