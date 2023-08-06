# Research history

Here are my researches leading up to the development of Native Audio. The earlier ones may contain misunderstanding or failures.

### (06/04/2017) "UnityTapSound"

This plugin was developed when I noticed that both my Android AND iOS game has a considerably worse **audio latency** than other music apps such as Garage Band installed on the **same device**.

I have confirmed by creating [a basic Unity app](https://github.com/5argon/UnityTapSound) which just play an audio on touch down vs. a basic [Xcode iOS app](https://github.com/5argon/iOSSoundTest)/[Android Studio app](https://github.com/5argon/AndroidPlayAudio) which also plays an audio on touch down. You can clone the project on GitHub to confirm this by yourself.

### (06/04/2017) "Unity's Mobile Audio Latency Problem And Solutions"

> [!Video https://www.youtube.com/embed/HxhLOiR-3Qw]

While containing many false assumptions I have get my feet (fingers) wet with many native solutions and arrives that `AudioTrack` (Android) and `AVAudioPlayer` (iOS) are better than the rest. Watch [the research video](https://www.youtube.com/watch?v=HxhLOiR-3Qw) or [read the first experiment's note here](../mobile-native-audio/research.md). The plugin is still in development at this point.

### (31/10/2017) "Unity iOS/Android Native Audio, Native Touch, and latency"

> [!Video https://www.youtube.com/embed/6Wot7lzZR5o]

This time the detailed write-up was [hosted on GitHub with repro project](https://github.com/5argon/UnityiOSNativeAudio), I have found that **touch/input latency** also plays a role in the perceived audio latency. By going native not just audio but also on touch, we can get up to 1 frame earlier (can be 2 on Android) of touch response and that translate to FREE -16ms audio latency as long as that audio plays on touch response. (That's a lot when you "feel" it as a part of interaction)

A 1-take video which is my final proof that the plugin will have any benefit. At this point the first version of Native Audio is out along with "[Native Touch](http://exceed7.com/native-touch)" to solve the aforementioned input latency.

Though, in that video I still insisted that `AVAudioPlayer` is the best for iOS. That is wrong.

### (12/04/2018) "Unity's Mobile Audio Latency Problem And Solutions - iOS Build & OpenAL"

> [!Video https://www.youtube.com/embed/Riws7Ais3bo]

On iOS `OpenAL` does not introduce frame rate drop when playing plus we get an even better latency. The plugin was updated to use `OpenAL` on iOS.

### (28/06/2018) "The Android OpenSL ES migration"

The research took place entirely in a [Discord channel](https://discordapp.com/invite/8gthuWA) I have recently started. A user searching for audio solution is not satisfied with other solution but at the same time using Native Audio introduce a strange frame skip on Android. I have no choice but to abandon AudioTrack and blindly go for even more native OpenSL ES and luckily it solves the problem, also luckily for me I found many more faster path than the old Java way. It leads to the massive [Android Native Audio Primer for Unity Developers](https://gametorrahod.com/android-native-audio-primer-for-unity-developers/) article. You can read the epic discovery I and that user made together there.

### (17/08/2018) "Faster input is possible on Android"

[I have discovered a way](https://github.com/5argon/UnityiOSNativeAudio#update-17082018--the-state-of-android-2) to get faster touch on Android and indirectly related when used with Native Audio you will get up to 10-33ms free "perceived" lantency reduction. Previously named "iOS Native Touch" it will be renamed to just "Native Touch" with Android support. You are free to use Native Audio in combination with that one if you want even more.

### (02/11/2018) "Unity’s Android audio latency improvement in 2019.1.0"

[Unity had just opened the door to alpha build. In this article](https://gametorrahod.com/unitys-android-audio-latency-improvement-in-2019-1-0-ebcffc31a947), I talk about a recent exciting news in 2019.1 alpha about audio latency improvement. You will learn about a portion of problem that Native Audio solves pre-2019 which Unity 2019 now solves as well, along with reasons to still use Native Audio to sqeeze more latency reduction.