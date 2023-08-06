# Selecting native source

There are fixed number of native sources waiting at the native side to play. You can specify exactly which one you want on the `playOptions`, or use the default value or special `-1` value and let Native Audio just select one for you in a round-robin style. Native Audio will not spend time mixing, so these multiple sources is the only place you can layer up your sounds.

This page suggests some plans of explicitly specifying the native source to play.

## Exclusive channel

It is not uncommon for a rhythm game to have only few kinds of response sound and is still fun. [Taiko no Tatsujin](https://en.wikipedia.org/wiki/Taiko_no_Tatsujin) has only 2 kind of response sound for center hit and rim hit. Imagine using Native Audio to make it, it make sense to permanently assign one sound to native source `0` and another sound to native source `1`.

The next play of the same kind will cut off previous one however (it maybe desired to reduce audio clutters), you can try alternating 2 sources each instead. e.g. center hit `0 1 0 1 0 ...`, rim hit `2 3 2 3 2 ...`.

## Pause and loop

Specifying the source explicitly is important in the case that you have used pause or looping functionality. If new audio lands on the native source that was paused, then the state will disappear. Imagine like in Unity, you also cannot just play all audio into one `AudioSource` blindly. (Unless you use `PlayOneShot` which is magical)

## Audio de-cluttering

You can use native source limitation creatively. When playing a rapid gunshot sound you might want the new shot to cut off the old shot's release tail so that the sound is not overcrowded. In that case just use the same native source index over and over.

There are cases where cutoff is natural. For example drum kit's cymbal. A new cymbal hit cutting off the previous one sounds more like a real thing.

## A mix of exclusive channel and round-robin

If you are not using `PlayOptions` on `Play` to specify the source manually, each play will cycle through **all** sources in round-robin. In the case of allocated 7 sources, it would sounds strange if a snare roll that goes quickly more than 7 times ended up cutting off lingering cymbal, for example. 

So here is an example of mixed strategy that may produce better result :

- Cymbals : only `0`
- Snares : `1`, `2` round-robin
- Toms, rides, kicks, any others : `3`, `4`, `5`, `6` round-robin

Snares was given its exclusive round-robin range because it has a longer release envelope, the sound tail being cut off abruptly will be too obvious if only a single native source is allowed for it.

> [!TIP]
> You can implement your source selection logic in your custom class that implements `INativeSourceSelector`, then use the `NativeAudio.GetNativeSource(INativeSourceSelector)` overload.