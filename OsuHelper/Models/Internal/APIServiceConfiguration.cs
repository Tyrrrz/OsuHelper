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

            // Clear API key when it's not required (security reasons)
            if (apiProvider != APIProvider.Osu)
                APIKey = string.Empty;
        }
    }
}