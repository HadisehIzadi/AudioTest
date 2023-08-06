# Game design and notecharting

This is completely unrelated from really audio latency related or Native Audio. But I guess for the sake of completeness of this section..

First and foremost this should come from your experience as a music gamer. Actually play the game and try to get good, play a lot of popular games that were done right and learn why they are fun. It ties to the game design as well so it may or may not apply to your game.

## Design affects pattern readability

Pay attention to game design because it affects how fun the pattern could be played.

- Why Superbeat Xonic is not as fun as maimai? Did you notice that fading out the note from inside out is much more distracting than maimai's "popup while not moving yet"?
- What would happen if maimai requires you to drag instantly instead of a beat later? 
- Why you could see the "horizon" in SDVX/Arcaea/Museca but not in Chunithm where it uses the UI as the horizon instead and used higher perspective view? What would happen if the game draw infinitely inside the screen instead of a much shallower sharp cut?
- What would happen if Lanota removed that center overlay and make it comes out from nothing?
- Why players uses Sudden intentionally in IIDX or DDR to have easier time in some sections?
- Why the "shutter" note in Jubeat is the easiest to read?
- Why Idolmaster has a slightly curved up note that benefits the reading while NeonFM looks messy?
- Why DDR feels snappy, harsh, and accurate while Pump It Up feels lenient and forgiving?
- What did you feel from seeing the note bomb effect of DDR compared to Pump It Up?
- Why the unique "W" lane in Museca that is reflecting the controller perfectly, is doing more harm than good in reading that they have to make it straight later?
- What Cytus, a Technika derivative, do to alleviate the read-ahead problem when you no longer have the incoming half to prepare?
- What would happen if a note tail in Project Diva is removed and it is just the symbol? Why the note tail disappeared suddenly on hit instead of a more graceful presentation?
- What is the benefit of bomb effect that zooms through the opposite direction of a note instead of staying still where you hit the note, an innovation by Chunithm's air note that Wacca eventually copied?
- In Jubeat, the note is coming from the inside so it is impossible to place jackhammers without being awkward to read, while vertical/perspective scrolling game is perfectly fine to do jackhammers. What would you do to Jubeat's design if you want to make jackhammer pattern viable? On the other hand, have you considered "banning" some patterns in your own game because it cannot work well enough?

Here are my tips. Most of the reading problems came from how you can relate one note to the other, and remaining problems are how notes enter the screen (suddenly? fading? scaling? how early?). It is difficult to tell which notes come at the same time in Jubeat more than vertical scrolling game, and easier still if they happen to be adjacent in cell to each other. However, what's even more difficult is in multidirection scrolling games like maimai or Superbeat Xonic where the opposing side may forms a double note. And there are more notes on the screen at a given time for you to "figure out" than Jubeat.

You may use visual aid like yellow color in maimai for doubles. Or color coded beats like DDR, Tone Sphere where you can read the beat far ahead, relaxing the brain from having to relate note positions. Some games employed both doubles indicator and color coded beat.

You can also use pattern design to help in note reading. Like you could gradually prepare player from closely relatable double notes, and take it from there, instead of starting from hard to read patterns. (See a song Mythos in maimai, for example. A close notes that leads to wide spin is fine. But it is not fine to start from wide doubles unless it is early in the song with less notes.) maimai is circular so they could do this, while Superbeat Xonic could not follow since there is no "bridge" that connect each sides. If you feel that your game is in a similar situation, maybe you should redesign it first before trying to find a pattern or add a new visual cues, which may not work out in the end. Maybe it is impossible by design. Prototype a lot, and harshly. (Demolish things that doesn't work without mercy) Don't go to the next phase until you can create a fun patterns from your design.

You maybe thinking of some excuses like : "Dammit, why is this not working like *that less-innovative game*? If we could get tons of OP composers like them...". A developer being in competitive spirit is always nice for music games community, but it maybe the case that you are not seeing innovations in those games.

You can't simply think that because most games scroll top to bottom, therefore you make it more innovative by doing perspective scroll. If that game is perspective scroll with note sprite, then you make more innovative by making the note a real 3D object. You must rethink did you introduces any other problems that didn't exist in that "less innovative" design and solve it accordingly.

## Long notes and doubles

Something that apply to generally all games, is when to use double/multiple notes at the same time.

Traditionally games are keysounded so doubles only appears when there are 2 sounds overlapping. That may be technically correct but not necessarily fun, because too many polyrhythms may confuse players. Nowadays doubles are used more expressively to highlight things. Cymbals are very commonly a double despite the drummer uses only one hand to hit one cymbal, for example. A piano chord may naturally be charted as 3-note chord, if your game allow. Polyrhythm works better when you have a 4-to-the-floor kick drum going on with something else like a lead synth that is relatively longnote-ish.

Beware of long note syndrome where you ended up charting everything as long notes. "But this is a vocal, and that is a long-ey synth! It's a blasphemy to chart those to anything *but* long notes!" The same apply as doubles/chords syndrome where you think each sound you hear is quite heavy and therefore you think they are justified to be an epic doubles. In the end, the chart ended up a chordsmashing mess. It dilute the effect of long note or doubles and make the chart bland. When you falls into this, the chart is said to went "rainbow" because you decorate everything according to the rule too correctly. And that is not necessarily a good thing.

It is fine to try "dry" charts and let the player "feel it" instead of cluttering the chart. You don't have to be overly worry if the player don't know which note is for which sound. Especially for non-keysounded game, use expressiveness to counter this. See DDR, there are many silent or made-up beats that works. Or Chunithm/Arcaea, where there is a kind of note that require movement but not a button press, you will see seemingly "random" sliding movement that goes to nothing, but they works when you play it. See the song Bird Sprite in Chunithm for an example of expressive, subjectivemania charting.

## Lane control

Which lane would you place the note? You have to be mindful about note density and hand movement. Other than obviously try to balance them out, even if the end product is balanced, not all lanes are equals.

A lane which is at your ring or pinky finger requires more effort to hit than index finger. There is a different weight in each lane. (So "balanced" is not necessary all lanes received the same amount of notes.)

A keyboard game like O2Jam has an awkward pattern called a bracket. If I label all the keys as 123 4 567, then this pattern : \[13\]\[2\]\[13\]\[2\]... is especially hard. Try alternating between first : hit index-ring at the same time, then second : hit only middle finger, then repeat over and over fast, you will know what I mean. In a game like IIDX, staircases are much harder than in O2Jam, because in O2Jam your fingers are already stationed linearly for them. In Pop'n Music, brackets are easier since you use the whole hand instead of fingers, and you can see-saw your wrist, or even use the other hand for help if available.

Beware of accidental patterns formed from your lane decision, like accidental mini-jacks (2 notes on the same lane one after another). For example you place a double/chord for cymbal, then the next note has higher chance to be in the same lane as one of the previous notes. You could do this intentionally for impact, or avoid this to increase flow. If a staircase runs into a cymbal you may be tempted to place a double in a way that it does not form an accidental mini-jack, but this may instead create a pseudo bracket. If there is no way out, you may skip that double instead or change it to a single long note. Get creative and don't be locked in your own rule in your head.

A game which requires physical movement like DDR, 2 jumps from UL to DR is a bit more demanding than 2 jumps from LR to UD, despite both cases no notes are overlapping with the previous one and absolutely no accidental patterns formed. The reason being UL to DR feels more like squatting where you may lose balance to the front or back, but LR to UD has perfectly countering momentum that keeps you steady. Jubeat is one game that didn't use body movement but unexpectedly rely on movements because you cannot station your hand to prepare for all patterns. For example, just the seemingly simple 2 double notes \[3+8\] corner pattern actually require you to twist the arm a bit from the rest position, because your hand can't pivot fast to that posture. Games like Chunithm or SDVX, where even in very difficult song an experienced player could be seen relatively stable and mashing the controller, you can cover most input except for patterns that locks your hand and force the other hand to crossover.

## Pitch relevancy and intuitive patterns

They are patterns that are mostly "agreed" to work. In real world, higher frequency sounds tends to go to the right or up. If your game has a "right" or "up", try to make that go together with the music. For example, if a piano arp that goes up is in the song, you may layout the note as from left to right as opposed to right to left. You may break this rule for pattern variation, but generally pitch relevancy is baked into human's brain already.

About intuitive patterns, if something "repetitive sounding" is coming naturally jackhammer pattern is a direct representation, but also a 2 note trills works as well. Something that sounds like a staircase could be replaced by slightly alternating back and forth staircase hybrid, that is still going from left to right "overall".

## Pattern variations

Test it out well. You will begin learning available variation that are kinda the same meaning wise. For example in DDR, if you previously used Left-Down-Right crossover (if your left foot starts on left, you are required to cross that foot to R button if you want to always alternating foot.) then when that sound comes again, it's mirror Right-Down-Left is the variation you could use, but not Left-Up-Down where there is no crossover and the weight of pattern is different. The player will fail to associate the variant with a prior pattern. Or in O2Jam, if you previously used a straight staircase 1234567 then later you may use something like 1425364, it will fell kinda the same, but with different notes.

But also sometimes repeating the same pattern over and over is more fun than changing the pattern often, so the player could get a feel what they are hitting.

In a genre like Techno for example, being repetitive could represent the song more than varying them because repetitiveness is what defining the genre. In IIDX, there is a term "L1 kick" where songs put kick drum consistently on lane 1, at least at the song's beginning. It helps the player getting into the song as opposed to it being all over the place for the sake of variation. In DDR a song Rising Fire Hawk Challenge chart, if you focus on the down arrow lane you see it meaningfully coming back to the lane most of the time kick drum strikes, it goes very well with a Jumpstyle song like this and feel like the kick is coming out from that pad despite being a non-keysounded game.

Music is in a way meaningful repetition with variation that are related with music theories, you can do the same with your notechart. The same reason music do not change its kick drum sound every measure, you can do the same with your notechart.

## Decorations

Recently there are more decoration elements added to the charts. It make the chart more distinct and visually interesting, but it may affect gameplay more or less as well.

### BPM changes

Classically BPM changes could be considered decorations, when the song goes to a break section then the BPM halves accordingly, making the chart looks more "breathtaking". This affects difficulty and tecnicality of the chart as well. Be aware of "slowdown syndrome" where you are always tempted to halves BPM on almost every song because you notice some kind of break in every song. It's a part of composition technique that before chorus, the tension before it should be low enough for the chorus to be powerful. That doesn't mean there should be a slowdown on every song.

### Note speed changes

Some games modify the note speed like they change your speed mod, Pump It Up extensively use this. Well, you can make bouncing charts and it is one of the most common thinking of this feature. But think for yourself if that is excessive or not.

In Taiko no Tatsujin, the "timeline" appears to tie to each notes as oppposed to all notes being run on the same timeline. This can produce bizarre effect like a later coming note appears to "race" faster and catching up or even get ahead of notes that came earlier.

### Environmental changes

Games like SDVX, Ongeki, Arcaea, or Lanota can move lanes and viewing perspective mid-song. Making it possible to vary play experience without touching notes. It is good to give the note charter more tools to play with more than available lanes.

Be careful when this affects user experience. In touch screen if this moves the touch zone it will be more difficult. In a game like Neon FM where you have a controller that stay still, excessive visual can still be too much and tiring instead of flowing. Do think from time to time that when one enjoy a music (not music game, just listening to music) the happiness do come from just gently bopping head, being in a flow that guides him to the end. You don't have to make the notechart like death trap stage in Japanese game show.

Groove Coaster have its experience based mainly on environmental changes, because there is a unique stage made for each song.

### Note arts

Withing the charts, you can use the note objects for decoration as well. These came from the fact that sometimes you don't have to play them or they are lenient enough that wacky shapes didn't affect gameplay too much. You may think about adding them to your game design that it simultaneously create a solid gameplay and interesting note chart. (Do not sacrifice solid gameplay.)

- Long note is the most common target (victim) to create note arts. In O2Jam, sometimes the tail length of long note doesn't represent the lingering sound at all. They are just long for the sake of their look! I personally call some stupid pattern in that game [a "Khene" pattern](https://en.wikipedia.org/wiki/Khene) because it looks like that Thai instrument. Reflec Beat also implemented a special on-off long note in its latest installment that is capable of creating note arts.
- "Rapid notes" that is more lenient in timing or positioning has been gaining popularity. Deemo has golden notes that you can just drag around, so they often make interesting shapes out of them while not affecting difficulty. VOEZ has particle notes that do the same thing. Dynamix's fader red note too.
- A game with fluid lanes (looks non-discrete) like Voez or Arcaea have more liberty to create art, much like latte art where instead of just pouring milk you can pour it anywhere in XY coordinate on the cup's surface. It will not affect the taste much, but more visually pleasing.
- A game with "warning indicator" that aids readability like SDVX (knob warning), Arcaea (air note incoming indicator), Ravon (grid note incoming indicator) can use those for artistic purpose.
- In BMS, charter sometimes use the custom measure bar feature just to make it more artistic when you see a lot more bars coming in grabbing your attention, you are not playing them.
- Games with note colors like Stepmania, DDR, or Tonesphere also could intentionally "colorize" the note. There are one period in Stepmania history where charters shift the note very little bit (64th, 192th off) just to make a note green colored without using mini-long note.
- Games with "do not hit" notes like DDR (shock arrow), Chunithm (damage notes), Ongeki (bullets), In The Groove/Stepmania (mines), can use those for artistic purpose. They are not there just for the player to avoid, they should also represent some "sound" in the song that you would rather not want player to play. They maybe damaging if hit, but you can use them where it is unlikely to hit them at all, remember that the player still see them and may have more fun if something in the song goes with it.
- It is not uncommon to express something "literally" with notes.
    - Anytime the song says "up" or "raise your hand" you feel the urge put a flick note that literally goes up.
    - Whenever the song says "guruguru" or "spin" the game must try to convey their version of "spin" just so the player notice and chuckled a bit.
    - When the song says "one two" you literally put one and two notes regardless of any other note theories.
    - Games with fluid notes often attempts to "draw image" with notes to flex that they could.
    - In VOEZ version of Freedom Dive the particle notes actually shaped like downwards arrow.
    - In the song ORCA in DDR A20 the chart turns yellow because the theme of DDR A20 is "golden".
    - On any games that licensed Conflict must find some ways to shoot a beam at the chorus.
    - On any games that licensed Brain Power must try to draw the song's lyric or make the screen goes crazy at the chorus.