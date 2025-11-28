using System.Collections.Concurrent;

namespace WatchMatchApi.Services
{
    public class DelayedActionScheduler(TimeSpan delay)
    {
        private readonly TimeSpan _delay = delay;
        private readonly ConcurrentDictionary<string, Timer> _timers = new();

        public TimeSpan DefaultDelay => _delay;
        public event Action<string>? ActionFired;

        public void Schedule(string key, Action action)
        {
            Schedule(key, action, _delay);
        }

        public void Schedule(string key, Action action, TimeSpan delay)
        {
            Cancel(key);

            var timer = new Timer(_ =>
            {
                try
                {
                    ActionFired?.Invoke(key);
                    action.Invoke();
                }
                finally
                {
                    Cancel(key);
                }
            }, null, delay, Timeout.InfiniteTimeSpan);

            _timers[key] = timer;
        }

        public void Cancel(string key)
        {
            if (_timers.TryRemove(key, out var timer))
            {
                timer?.Dispose();
            }
        }
    }
}
