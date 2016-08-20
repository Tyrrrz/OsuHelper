// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <PlayRank.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System.Diagnostics.CodeAnalysis;

namespace OsuHelper.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Not my fault they're all caps")]
    public enum PlayRank
    {
        D,
        C,
        B,
        A,
        S,
        SS,
        SH,
        XH
    }
}