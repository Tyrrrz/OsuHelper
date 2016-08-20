// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <Persistence.cs>
//  Created By: Alexey Golub
//  Date: 21/08/2016
// ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using NegativeLayer.Settings;
using NegativeLayer.WPFExtensions;
using OsuHelper.Models.Internal;

namespace OsuHelper
{
    public sealed class Persistence : SettingsManager
    {
        public static Persistence Default { get; } = new Persistence();

        private IEnumerable<BeatmapRecommendation> _lastRecommendations;
        private Rect _mainWindowRect;

        public IEnumerable<BeatmapRecommendation> LastRecommendations
        {
            get { return _lastRecommendations; }
            set { Set(ref _lastRecommendations, value); }
        }

        public Rect MainWindowRect
        {
            get { return _mainWindowRect; }
            set { Set(ref _mainWindowRect, value); }
        }

        public Persistence()
            : base(new SettingsManagerConfiguration("OsuHelper", "Persistence.dat"))
        {
            TryLoad();

            DispatcherHelper.UIDispatcher.InvokeSafe(() =>
            {
                Application.Current.Exit += (sender, args) => TrySave();
            });
        }
    }
}