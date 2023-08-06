# Things to keep in mind

### Concurrency limit

This one got people the most when they expect Native Audio to be easy and "just works" as a drop-in `AudioSource.PlayOneShot()` replacement, that I have to mention it everywhere in this site. Remember that a new play on any native sources **completely cut off the previous play** because we do not mix audio.

Refer to explained [Ways around latency](../theories/ways-around-latency.md) how much native sources you could have, which equals how many concurrent audio you could have.

### Do not play music

Native Audio is for small but latency critical sound. There is no care about memory footprint at all. The loaded audio will be uncompressed, if used with music this will be massive! (easily 30+ MB of RAM per song)

Anyways there are not many reasons to **start** the music fast. Maybe you were looking for precision instead of latency (e.g. in a sequencer app), in that case you should use [`PlayScheduled`](https://docs.unity3d.com/ScriptReference/AudioSource.PlayScheduled.html) to play at precise future instead of using Native Audio that plays at fastest present.

### Native Audio skips Unity entirely

-   It ignores Unity's mixer. If you used to mute your game with it the sound from Native Audio will still plays.
-   Even with `Project Settings > Audio > Disable Unity Audio` you can still hear sounds from Native Audio. (obviously)
-   The audio memory and CPU usages will not show up in Unity's Profiler.
-   The memory loaded in Native Audio is not garbage collected. If you lose a `NativeAudioPointer` in managed C# side it does not mean the garbage collector can free up audio memory. his pointer just remembers which audio player instance at native side belongs to which sound. By the way that pointer has an optional `.Unload()` method. (Do not unload while you know this sound is playing, and there is currently no programmatic way to check this.)

### Native Audio is frame-independent

Unity's audio system wait and collects all the play command and summarize/process them at the end of frame, finally playing sound at the end.

Native Audio on the other hand does not have any concept of frame once it is called and left the C# realm. That means unlike `audioSource.Play()` **the line of code position which you call `nativeAudioPointer.Play()` now affects the latency as well**. The micro-optimization you can do is if you can call it earlier in a frame, you could squeeze even more latency reduction.

Another subtle difference and might be a problem in your game : For example if you have a full-screen attack that destroy multiple enemies at the same time. You used to loop through each enemy and play the bomb sound once per enemy with `audioSource.Play()`. All bomb sounds will play exactly at the same time due to Unity summarize commands at the end of the frame I explained earlier. With Native Audio however, it will play right there when you are looping through each enemy. If the loop is a costly function then you might actually hear the sounds being played sequentially fast instead of exactly at the same time.

### Native Audio is main thread-independent, but not thread safe

Also unlike Unity's API it does not have to be called from the main thread. If you have a calculation script or something NOT in the main thread, (like using Unity [C# Jobs system](https://docs.unity3d.com/Manual/JobSystem.html), or C#'s [Task Parallel Library](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming)) if that calculation is to decide to play or not, you can ask Native Audio to play right there without synching to the main thread first. (On the other hand if you use `audioSource` in non-main thread it will mostly error out.)

If you are already using [Data-Oriented Tech Stack](https://unity.com/dots), you may play Native Audio from jobs scheduled from your systems.

On Android Mono build, it works perfectly with [Native Touch](http://exceed7.com/native-touch) where the returned faster touch is on a different thread. You don't need a sync point in the main thread to play audio from that touch. The faster touch time gained is now directly translated to better perceived audio latency. On iOS the touch is already on the main thread, there is no problem playing either Native Audio or normal `AudioSource`.

Not thread safe means that even if you can play audio from any thread, you should avoid 2 different threads using `nativeAudioPointer.Play()` at the **same time**. This is because Native Audio avoid using mutexes, which can cause thread preemption and glitches or even segmentation fault. (The same reason as [why AAudio is not thread safe](https://developer.android.com/ndk/guides/audio/aaudio/aaudio#thread-safety).)

### No need to warm up the circuit

From [this official documentation](https://developer.android.com/ndk/guides/audio/audio-latency#warmup-latency), it is possible to "warm up the audio circuit". It may already be a sorts of common sense to you, the first play of audio while the circuit is cold will have more latency. So you may think of countering it by playing some "dummy" audio before the real one to "warm it up".

But with Native Audio there's no need to do this since I made Native Audio to [always playing silence audio](https://gametorrahod.com/androids-native-audio-primer-for-unity-developers-65acf66dd124#44e3) when it is not busy.

This is originally not intended for warming up, it is a hack to "keep active" the audio tracks in the native side so that a costly `AudioPolicyManager` on some phone causing frame rate drop does not rapidly triggered when playing sounds for a lot of times. (I suspect Unity is also doing this with its own native source requested from Android) Always warmed up being a nice side effect of this and it improves latency. So no need to pre-play your audio with volume 0 or anything.