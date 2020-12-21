using System;

namespace SfMdfSpeed
{
    public record SpeedResult
    {
        public string Title { get; }
        
        public long Size { get; }
        public double Time { get; }

        public SpeedResult(string title, long size, double time) => (Title, Size, Time) = (title, size, time);
    }
}
