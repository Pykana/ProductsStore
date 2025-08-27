using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Config
{
    public class TimeZoneService
    {
        private readonly string _timeZone;

        public TimeZoneService()
        {
            _timeZone = Variables.STORE_CONFIG_TIMEZONE;
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
