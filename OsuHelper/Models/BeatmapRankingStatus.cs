// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <BeatmapRankingStatus.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

namespace OsuHelper.Models
{
    public enum BeatmapRankingStatus
    {
        Graveyard = -2,
        WorkInProgress = -1,
        Pending = 0,
        Ranked = 1,
        Approved = 2,
        Qualified = 3
    }
}