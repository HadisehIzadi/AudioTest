# Unity's Mobile Audio Latency Problem And Solutions

Sirawat Pitaksarit/5argon - Exceed7 Experiments (6/4/2017)

> [!Video https://www.youtube.com/embed/HxhLOiR-3Qw]

## UPDATE 14/04/2018

This whole research was debunked by myself. After reading the whole thing, [please go to here for the continuation](https://github.com/5argon/UnityiOSNativeAudio). Mainly, going native on audio is one thing but there is ALSO an input delay! And the assumption that `AVAudioPlayer` is the best for iOS that I said here was wrong. Actually `OpenAL` is the best solution as far as I know. `AVAudioPlayer` also drop frame rate a little, but very apparent when rapidly play sounds.

Also, two Unity plugins that are for tackling this problem has been released : [Native Audio](http://exceed7.com/native-audio/) and [iOS Native Touch](http://exceed7.com/ios-native-touch/). Those webpages also serves as a knowledge base of all my researches including this one too. I won't update this page anymore but keep it as a diary. I may continue update those product page with more knowledges.

## Objective

I want to play an audio as a response to a touch screen tap on an iOS/Android game. Very simple, but many have underestimate the audio latency.

Currently, even the most basic sound playing in Unity ( Get touch down -> `audioSource.Play()`) results in a **long** latency on both iOS and Android.

The latency is not as horrible as you think, if this is just a response sound to a button tap it would be more than fine. However what if you want to make a musical apps like drumming, piano apps with Unity? This would be detrimental to the core experience. (It is known that a latency of less than 10ms is required for musicians, as is the case with real piano, guitar, etc.)

## 3 Classes Of Musical Apps

*   **Sequencer :** Application like digital audio workstation (DAW) on mobile phone or live performing musical apps like Looper, Launchpad falls into this category. The app is interactive, but the reference of what is the "correct" timing are all controllable. Imagine you start a drum loop. Each sound might have delay based of device, but all delays are equal, results in a perfect sequence albeit variable start time. When starting another loops, it is 100% possible for the software to compensate and match the beat that is currently playing. This class of application is immune to mobile audio latency.
*   **Instrument :** Apps like GarageBand (in live playing mode) is in this category. The sound have to respond when you touch the screen. A latency can impact the experience, but if you are rehearsing by yourself you might be able to ignore the latency since if you play perfectly, the output sound will all have equal latency and will be perfect with a bit of delay.
*   **Music Games :** There are many music games on mobile phone like Cytus, Deemo, Dynamix, VOEZ, Lanota, etc. If there is a sound feedback on hitting the note, this is the hardest class of the latency problem. Unlike Sequencer class, even though the song is predictable and the game know all the notes at all points in the song you cannot predict if the sound will play or not since it depends on player's performance. (Unless the sound is played regardless of hit or miss or bad judgement, then this class can be reduced to Sequencer class.) It is harder than Instrument class, since now we have backing track playing as a reference and also a visual indicator. If you hit on time according to the visuals or music, you will get "Perfect" judgement but the sound will be off the backing track. When this happen, even though you get Perfect already you will automatically adapt to hit earlier to make that respond sound match with the song, in which case you will not get the Perfect judgement anymore. In the Instrument class, if you are live jamming with others this might happen too but if you adapt to hit early you can get accurate sound and not be punished by the judgement like in games.

What I am making is a [music game](http://exceed7.com/mel-cadence). Even a little bit of latency will be very obvious. Since there is a beat in the song for reference, players will be able to tell right away that he/she is hearing 2 separate sound (the beat in the song and the response sound) even if the player scores a perfect.

## Unity's Default Latency

I have made the simplest project for confirming that Unity has this problem. You can also try it by getting the project from my GitHub [here.](https://github.com/5argon/UnityTapSound). It is just a check for touch in `Update` that triggers `audioSource.Play()`, and also changing background color to confirm when did the touch registers. I did not use uGUI's event system just in case you are skeptical of EventSystem's performance of catching events. (It's about the same, I have tested.)

Look at the clip on the top of this page to see the problem. Unity is on **Unity version 5.6.0f3**, the newest ones at the moment. Note that the screen turns to red very fast on iOS, highlighting superior touchscreen capability of Apple's hardware. But in both iOS and Android, the sound delays way too much for a simple project like this.

In the clip I have already used Project Settings > Audio > DSP Buffer Size > Best Latency.

On Android this is a [well-known problem](http://www.androidpolice.com/2015/11/13/android-audio-latency-in-depth-its-getting-better-especially-with-the-nexus-5x-and-6p/), but on iOS which was always praised by its rich contents of musical apps and the legendary latency of its CoreAudio to perform this bad, then it must be Unity's fault. Unity adds many audio features which I guess might adds up a lot of latency along the way.

## Native Audio

The basic solution should be trying to play an audio using the respective platform's fastest method. If we could write a plugin for Unity that can access those methods then we can bypass Unity's latency. First, we will confirm the performance of such native methods compared to Unity's unified `audioSource.Play()`.

On iOS, I will use `AVAudioPlayer`. There is also `AudioToolbox` and its `SystemSound`, but from researches the performance is similar.

On Android, some possible ways that plays audio the fastest is `SoundPool` and `AudioTrack`. There is also `MediaPlayer` but that is slower. Lastly, the most native way is OpenSL ES, which you have to write in C++ and also Java for interfacing. I have also include this in this test.

There is a catch on Android, it is possible to set `FLAG_LOW_LATENCY` ([https://developer.android.com/reference/android/media/AudioAttributes.html#FLAG\_LOW\_LATENCY](https://developer.android.com/reference/android/media/AudioAttributes.html#FLAG_LOW_LATENCY)) on Nougat. To make it effective it says the audio file needs to be at the same sample rate as the device's "native sample rate". Because of this, the tests will include both 44100Hz and 44800Hz to confirm this.

## Sargon

In the test we will measure **Sargon**, which is a self-made term that I will define for convenience of referring. It is a unit invented for subjectively measuring smartphone application's audio experience.

**Sargon** is a time interval from the peak of touchscreen's tap sound to the beginning of response sound from the application that an observer hear.

Of course this means it is dependent on a distance between sound source and an observer too, but in a very close distance a little bit of difference should not be a problem. The unit was for smartphone applications, the distance from the phone's speaker to player's ear is going to be almost the same and can be ignored considering sound's 343 m/s speed.

This Sargon unit is different from the usual latency test that it also accounts for input latency of an application + OS's touch screen driver + speaker driver. (Unity for example, adds a lot to input processing.) And it also include the time that a sound need to travel through the air too.

What's more is that it measure from the tap sound that your nail make to the touchscreen, as opposed to letting hardware to determine the starting point. I think this is a better starting point since application user's experience will depends on noticability of tap sound and the actual feedback sound.

When using a loopback cable method, the latency can be much lower than this but it is not a realistic use case. (Also when wearing headphone, the measured latency will be lower than the one that passed a speaker driver.)

## The Test

With terms defined, I will measure an **average Sargon** from phone's speaker to my computer nearby. The computer is used instead of my own ear to be able to calculate Sargon accurately. My computer and the position of my phone will remains almost at the same place throughout the test, because that will make all Sargons comparable to each other.

### Android's Test

#### Devices

*   Nexus 5 - Official Marshmallow (API 24)
*   Nexus 5 - Lineage custom ROM Nougat (API 25)

The audio feature of Nexus 5 is as follows :

*   LOW\_LATENCY == true, it means continuous output latency of 45 ms or less.
*   PRO == false, it means a continuous round-trip latency of 20 ms or less.
*   Native sample rate : 48000Hz

#### Tests

1.  Unity audioSource.Play()
2.  SoundPool
3.  AudioTrack
4.  AudioTrack + Low Latency Flag
5.  AudioTrack (Native Rate)
6.  AudioTrack + Low Latency Flag (Native Rate)
7.  OpenSL ES

### iOS's Test

#### Devices

*   iPod Touch Gen 5 (4-5 years old) - iOS 9
*   iPhone SE - iOS 10

#### Tests

1.  Unity's audioSource.Play()
2.  AVAudioPlayer

### Test Programs

*   Ableton Live - For recording tap and feedback sound on the computer and calculate Sargon. There is a 30-day trial if you want to try it out.
*   TapSound - A simple Unity program to call audioSource.Play() on touch. In this test it was compiled with Unity 5.6.0f3. [It's on my GitHub.](https://github.com/5argon/UnityTapSound)
*   SoundTest - An XCode project that uses AVAudioPlayer to play sound on tap. [It's on my GitHub.](https://github.com/5argon/iOSSoundTest)
*   PlayAudio - An Android Studio project that can play sounds in 5 different ways that uses SoundPool and AudioTrack. [It's on my GitHub.](https://github.com/5argon/AndroidPlayAudio)
*   NDK Native Audio - An example from Google that uses OpenSL ES to play sounds. [The original is here](https://github.com/googlesamples/android-ndk/tree/master/native-audio), however the one that I am using has been modified with my own sound file and with button triggers on touch down (instead of up).

All program plays a sound on touch **down**. I measure Sargon about 10 times from each tests and averaged them. The measurement is by my own eyes as where is the peak, but with an average this should leverage the problem.

## Results

  
![results](./Unity's Audio Latency Problem and Solutions_files/research-result.png)

[Here you can access that spreadsheet and view all measurement data.](https://docs.google.com/spreadsheets/d/1kSqkLM2C1NjxXg2oBcZzsVc9ooT4pZo2wSOY0Vt8C7k/edit?usp=sharing)

[You can also get the Ableton Live project file which contains all the audio recordings that I measured Sargon from. (~100MB)](http://exceed7.com/mobile-native-audio/LatencyTestProject.zip)

## Summary

*   Unity's audioSource.Play is so bad that even the new iPhone SE performs poorly. (Almost at the same level as a much older Nexus 5)
*   On Android, using `AudioTrack` is the way to go. It's not worth the trouble to go the way down to OpenSL ES as I see no improvement in latency.
*   Upgrading to Nougat, using low latency flag or not, using native rate or not, **does not matter** in a meaningful way. Using native rate seems to be a little bit better (3-4 ms better) but I am afraid that is just a coincidence.
*   On iOS, using `AVAudioPlayer` is also the way to go. It depends a lot on phone's capability as iPhone SE has an extremely low Sargon.
*   (Subjectively) A Sargon in this test that is around 100ms is **acceptable** to be used in musical applications.
*   With **native audio** playing on Android, it is able to reduce the latency to the level of iOS audio that is played **through Unity** with Best Latency settings. So that means...
*   If the latency on iOS (played with Unity) was acceptable for you before, it is now **possible** for Android to make a music application if the specs is around Nexus 5 or higher if you use AudioTrack for playing sounds.

So concluding this research, you should write a plugin that plays an audio with `AudioTrack` and `AVAudioPlayer` for Unity to tackle this problem. Probably you have to put your sounds in `/StreamingAssets` so that it will be copied verbatim to the final build.

## Update (19/8/2017)

*   The final scene in the clip I have made an error. The sound sounds matching with the music, but that is because I adapt to hit early so that the sound match. (listen to my nail's sound. It's earlier than music) So actually, Android Native is still not that good.
*   I tried FMOD for Unity on Nexus 5 and iPod Gen 5 to see if it helps with the latency or not. The result is NO. At best, the latency is equal to Unity's Best Latency settings on both devices. This is with `FMOD.System::setDSPBufferSize(64,4)`, a value so low that even a short sound effect starts cracking. The test was done with both Vorbis and FADPCM encoding, both with decompress playing method.
*   Haven't tried wwise, since their website does not looks as friendly as FMOD. But I assume the result might be similar.

## But wait! There's more...

If you don't want to implement what I have said on this entire page by yourself, I am probably making a Unity plugin for sale. Haha! (soon) At least only on iOS side, since Android's native latency is still not enough for me.

(Update 14/04/2018) They are out! [Native Audio](http://exceed7.com/native-audio/) and [iOS Native Touch](http://exceed7.com/ios-native-touch/)