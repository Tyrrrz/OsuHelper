namespace OsuHelper.Services
{
    public class RippleDataService : OsuDataService
    {
        protected override string GetApiRoot() => "https://ripple.moe/api/";

        public RippleDataService(IHttpService httpService)
            : base(httpService)
        {
        }
    }
}