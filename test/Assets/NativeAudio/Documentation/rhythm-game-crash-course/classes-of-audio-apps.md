# 4 classes of audio applications

## Music player

iTunes is a musical app. But do it need low latency play? No! No one cares if the song starts immediately on pressing play or not. The goodness is in the song and you are listening to it! In this case audio fidelity is the most important, and audio latency is not that of the concern. It is not an **interactive** audio application.

Also to prevent audio cracking because of buffer underrun they usually do so by increasing buffer size, which in turn increase latency. But music player do not care about latency, so that is generally a free quality.

## Sequencer

Application like digital audio workstation (DAW) on mobile phone or live performing musical apps like Looper, Launchpad falls into this category. The app **is interactive**, but the reference of what is the "correct" timing are all controllable. Imagine you start a drum loop. Each sound might have delay based on device, but all delays are equal, results in a perfect sequence albeit variable start time. When starting another loops, it is 100% possible for the software to compensate and match the beat that is currently playing. This class of application is immune to mobile audio latency.

## Instrument

Apps like GarageBand (in live playing mode) is in this category. The sound have to respond when you touch the screen. A latency can impact experience, but if you are rehearsing by yourself you might be able to ignore the latency since if you play perfectly, the output sound will all have equal latency and will be perfect with a bit of delay.

Also remember that **all** instruments in the world do have their own latency. Imagine a piano which the sound must travel through its chamber, or an effected guitar with rather long chain, or a vocalist which heard their own voice immediately "in the head". How could all of them managed to play together in sync at the concert? With practice you will get used to the latency naturally! So this class is not as bad as it sounds with a bit of latency.

## Music games

There are many music games on mobile phone made by Unity like Cytus, Deemo, Dynamix, VOEZ, Lanota, Arcaea, etc. If there is a sound feedback on hitting the note, this is the hardest class of the latency problem. Unlike Sequencer class, even though the song is predictable and the game know all the notes at all points in the song you cannot predict if the sound will play or not since it depends on player's performance. (Unless the sound is played regardless of hit or miss or bad judgement, then this class can be reduced to Sequencer class.)

It is harder than Instrument class, since now we have backing track playing as a reference and also a visual indicator. If you hit on time according to the visuals or music, you will get "Perfect" judgement but the sound will be off the backing track. When this happen, even though you get Perfect already you will automatically adapt to hit earlier to make that respond sound match with the song, in which case you will not get the Perfect judgement anymore. In the Instrument class, if you are live jamming with others this might happen too but if you adapt to hit early you can get accurate sound and not be punished by the judgement like in games.

Even a little bit of latency will be very obvious in a music game. Since there is a beat in the song for reference, players will be able to tell right away that he/she is hearing 2 separate sound (the beat in the song and the response sound) even if the player scores a perfect.

Also you can think music games works like instrument. Game with respond sound like Beatmania IIDX, Groove Coaster, or DJMAX Respect for example if you listen to the "button sound" of good players that tapped to the music, the tap sound is obviously **before** the audio, but they could consistently do that throughout the song that the response sound comes out perfectly in sync and get perfect scores. These games in a way, works like a real instrument. **You live with the latency** and get good.

On the contrary a game like Dance Dance Revolution has no response sound, and is properly calibrated so that you can step **exactly** to the music and get Marvelous judgement. If you go listen to footstep of good players, you can hear that it matches perfectly with the audio. In effect that means a game like this had already accounted for the time audio traveled from loudspeaker to your ear in the judgement calibration!