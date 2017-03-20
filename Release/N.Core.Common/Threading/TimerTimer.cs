using System;
using System.Threading;
namespace N.Core.Common.Threading
{
    internal class TimerTimer
    {
        private Timer _timer;
        private Action _action;

        public TimerTimer(Action action, object state, TimeSpan dueTime, TimeSpan period)
        {
            _action = action;
            _timer = new Timer(PCLTimerCallback, state, dueTime, period);
        }

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            return _timer.Change(dueTime, period);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        internal void PCLTimerCallback(object state)
        {
            _action.Invoke();
        }
    }
}
