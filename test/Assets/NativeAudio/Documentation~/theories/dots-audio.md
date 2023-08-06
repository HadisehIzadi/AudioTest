# DOTS Audio

Unity's Data-Oriented Technology Stack is exciting! An audio will get its deserving upgrade too which is called [DOTS Audio](https://forum.unity.com/threads/dots-audio-discussion.651982/). Unity made this because of technological debt with FMOD was accumulating and would like to utilize its own internal custom solutions. Integrating DOTS with audio.

The question of our interest is could DOTS Audio solve the same problem as Native Audio or even beat and make Native Audio obsolete? Here are example features you can grok from that thread : 

- You are allowed more direct access to `AudioClip`'s content. The foundation engine code were added in 2019.1 if you take a look at [UnityCsReference](https://github.com/Unity-Technologies/UnityCsReference).
- Including on thread! There are new C# Jobs System type that could operate on audio and connecting as a graph ["DSP Graph"](https://www.youtube.com/watch?v=kDE-KxQBiYQ).
- Output directly with [HAL integration](https://forum.unity.com/threads/dots-audio-discussion.651982/page-3#post-5186321), it is like Unity is commanding the native side straight from `AudioClip` to hearing it.

Meaning that : 

- The direct interfacing to `AudioClip` has potential to help Native Audio, since we maybe able to skip the copy and just put a DSP Graph that performs the resampling explained in [Ways around latency](ways-around-latency.md) page completely at C# side and on thread too.
- The HAL integration has potential to kill the current Native Audio, as we even don't have to touch OpenAL/OpenSL ES or any "native sources" mess anymore and just go to the speaker? I am not sure how this would play out, but it is possible that in the future when these are more fleshed out we can have Native Audio that doesn't really have any native code, but performs like one.
- DSP Graph has potential to implement a "custom mixer". Native Audio is currently crippled because we don't mix and we get a low number of concurrency. If we want to mix then we fall back to Unity's slower latency. With DOTS Audio we can have a middle ground, maybe I can make a DSP Graph supporting low amount of channels to mix from, so we are still reasonably fast while being more flexible. This would help Native Audio make something like "near-Native Audio".

In short, "the Native Audio's way" is already very close to theoretical limit to play the fastest audio. By "theoretical" it means DOTS Audio is unlikely to surpass that, however DOTS Audio is capable of getting similar performance with much better potential and utilities. It will be exciting to add more useful functions to Native Audio while adding very little latency in return thanks to DOTS Audio.