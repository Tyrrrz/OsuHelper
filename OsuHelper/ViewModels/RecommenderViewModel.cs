// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <RecommenderViewModel.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using GalaSoft.MvvmLight;
using OsuHelper.Services;

namespace OsuHelper.ViewModels
{
    public class RecommenderViewModel : ViewModelBase
    {
        private readonly APIService _apiService;

        public RecommenderViewModel(APIService apiService)
        {
            _apiService = apiService;
        }
    }
}