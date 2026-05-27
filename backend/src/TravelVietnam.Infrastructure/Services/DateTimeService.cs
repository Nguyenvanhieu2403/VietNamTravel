using System;
using TravelVietnam.Application.Interfaces;

namespace TravelVietnam.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
