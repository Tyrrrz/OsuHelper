// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <APIServiceConfiguration.cs>
//  Created By: Alexey Golub
//  Date: 22/08/2016
// ------------------------------------------------------------------ 

namespace OsuHelper.Models.Internal
{
    public class APIServiceConfiguration
    {
        public APIProvider APIProvider { get; }
        public string APIKey { get; }

        public APIServiceConfiguration(APIProvider apiProvider, string apiKey)
        {
            APIProvider = apiProvider;
            APIKey = apiKey;
        }
    }
}