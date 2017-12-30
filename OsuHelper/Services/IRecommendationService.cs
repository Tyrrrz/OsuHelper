using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OsuHelper.Models;

namespace OsuHelper.Services
{
    public interface IRecommendationService
    {
        Task<IReadOnlyList<Recommendation>> GetRecommendationsAsync(IProgress<double> progressHandler);
    }
}