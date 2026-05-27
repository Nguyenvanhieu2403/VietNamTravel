using System;

namespace TravelVietnam.Application.Interfaces
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}
