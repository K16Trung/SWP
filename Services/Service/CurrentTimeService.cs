using Services.Service.Interface;

namespace Services.Service
{
    public class CurrentTimeService : ICurrentTimeService
    {
        public DateTime GetCurrentTime() => DateTime.UtcNow;
    }
}
