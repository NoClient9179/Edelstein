using System;
using System.Timers;

namespace Edelstein.Core.Utils.Ticks
{
    public class TimerTicker : ITicker
    {
        private readonly Timer _timer;

        public TimerTicker(TimeSpan time, ITickBehavior behavior)
        {
            _timer = new Timer
            {
                Interval = time.TotalMilliseconds,
                AutoReset = true
            };
            _timer.Elapsed += async (sender, args) => await behavior.TryTick();
        }

        public void Start()
            => _timer.Start();

        public void Stop()
            => _timer.Stop();
    }
}