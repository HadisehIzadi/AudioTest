# StreamingAssets loading

This plugin also supports loading a `.wav` file from `StreamingAssets` folder with `NativeAudio.Load(stringPath, loadOption)` overload instead of using raw decoded data from `AudioClip`.

It was originally the only way before version 4.0.0. However unlike `AudioClip` that you can customize using Unity's importer, the supported format is very limited :

- Must be uncompressed PCM.
- File name must end in `.wav`.
- Bit depth must be 16-bit.
- Mono or stereo.
- The file parser at native side is very basic, may not handle "exotic" `.wav` file.

This method is now **not recommended** for regular use and is not maintained nor upgraded anymore, but in some situation it may be useful, such as downloading an uncompressed audio from online to the folder that represent `StreamingAssets` at runtime.