osu!helper
===================


Stand-alone application that recommends osu! beatmaps, based on user's top plays and calculates expected PP gain using [Oppai](https://github.com/Francesco149/oppai).

**Recommendation Algorithm:**

1. Gets the user's top plays
2. For each top play, looks at others' scores on the same map that have similar performance (based on PP gained)
3. For each of the similar scores, visit player's who did them and look at their top plays. Take plays that are similar in PP gain to the original one.
4. Compose a list of plays that are similar by PP gained to the user's top plays. Remove plays that give less PP than the user's lowest top play. Remove plays on maps that the user already has a top play on.
5. Group the results by beatmap
6. For each group, get the median play, based on PP gained
7. Get beatmap data for each result
8. Return a list of recommendations

**User settings:**

- *(Required)* Username or UserID - self explanatory
- *(Required)* API Provider - the server you're playing on (currently either osu.ppy.sh or ripple.moe)
- *(Required if osu)* APIKey - get it [here](https://osu.ppy.sh/p/api)
- Prefer map download without video - if checked, osu!helper will only download beatmaps without video
- Don't suggest based on plays that are not at least S ranks - ignores lower ranked plays from suggestions (sorry, no way to determine FC atm)
- Difficulty preference - from easy to hard, what difficulty you prefer to see in recommended beatmaps
- Maximum recommendation count - up to how many results to return (less = faster)

**Dependencies:**

 - [Oppai](https://github.com/Francesco149/oppai) - performance point calculation
 - [GalaSoft.MVVMLight](http://www.mvvmlight.net/) - MVVM rapid development
 - [Json.NET](http://www.newtonsoft.com/json) - for deserialization of API responses
 - [MaterialDesignXAML](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit) - MaterialDesign UI
 - [Dragablz](https://github.com/ButchersBoy/Dragablz) - MaterialDesign tab control
 - [NLExtensions](https://www.nuget.org/packages/NLExtensions/) - my set of various extensions for rapid development
 - [NLWPFExtensions](https://www.nuget.org/packages/NLWPFExtensions) - my set of various WPF extensions for rapid development
 - [NLSettings](https://www.nuget.org/packages/NLSettings) - my settings manager

**Screenshots:**

![](http://www.tyrrrz.me/projects/images/osuhelper_1.jpg)
![](http://www.tyrrrz.me/projects/images/osuhelper_2.jpg)
![](http://www.tyrrrz.me/projects/images/osuhelper_3.jpg)
![](http://www.tyrrrz.me/projects/images/osuhelper_4.jpg)
