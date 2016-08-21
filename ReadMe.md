osu!helper
===================


Stand-alone application that (currently only) recommends osu! beatmaps, based on user's top plays.

**Algorithm:**
1) Gets the user's top plays
2) For each top play, looks at others' scores on the same map that have similar performance (based on PP gained)
3) For each of the similar scores, visit player's who did them and look at their top plays. Take plays that are similar in PP gain to the original one.
4) Compose a list of plays that are similar by PP gained to the user's top plays. Remove plays that give less PP than the user's lowest top play. Remove plays on maps that the user already has a top play on.
5) Group the results by beatmap
6) For each group, get the median play, based on PP gained
7) Get beatmap data for each result
8) Return a list of recommendations

**User settings:**
*(Required)* Username or UserID - self explanatory
*(Required)* APIKey - get it [here](https://osu.ppy.sh/p/api)
How many of your top plays to scan? - (Min 1, Max 100, Default 20) - Set this to larger values to receive suggestions that give less PP, but are easier or set this to smaller values to get hard maps that give a lot of PP. Larger values also increase the scan time.
How many of other's top plays to scan? - (Min 1, Max 100, Default 5) - Higher value means more suggestions, but it takes longer to parse them all.
How many similar plays to one of your play? - (Min 1, Max 100, Default 5) - Same as above.

**Screenshots:**

![](http://www.tyrrrz.me/projects/images/osuhelper_1.jpg)
