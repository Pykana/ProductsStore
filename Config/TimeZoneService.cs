using static BACKEND_STORE.Shared.Logs;

namespace BACKEND_STORE.Config
{
    public class TimeZoneService
    {
        private readonly string _timeZone;
        public TimeZoneService(IConfiguration configuration)
        {
            _timeZone = configuration["STORE_CONFIG_TIMEZONE"] ?? GetTimeZone();
        }
        public static string GetTimeZone()
        {
            return "UTC"; 
        }
        public TimeZoneInfo GetTimeZoneInfo()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(_timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                
                return TimeZoneInfo.Utc;
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Utc;
            }
        }
    }
}
