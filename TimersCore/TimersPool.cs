using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngineTimers
{
    public sealed class TimersPool : IDisposable
    {
        #region Fields

        private static TimersPool _instance;
        private List<Timer> _timers;
        private const int TIMERS_PRE_GENERATE = 6;

        #endregion

        #region Properties

        public Timer Timer
        {
            get
            {
                // Find an inactive timer
                foreach (var timer in _timers)
                {
                    if (!timer.IsRunning)
                    {
                        return timer;
                    }
                }

                // All timers are busy; create a new one
                var newTimer = new Timer();
                _timers.Add(newTimer);
                return newTimer;
            }
        }

        #endregion

        #region ClassLifeCycle

        private TimersPool()
        {
            _timers = new List<Timer>(TIMERS_PRE_GENERATE);
            for (int i = 0; i < TIMERS_PRE_GENERATE; i++)
            {
                _timers.Add(new Timer());
            }
        }

        public static TimersPool GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TimersPool();
            }
            return _instance;
        }

        #endregion

        #region Methods

        public IStop StartTimer(float time, UnityAction endCallback) => Timer.Start(time, endCallback);
        public IStop StartTimer(float time, UnityAction<float> progressCallback) => Timer.Start(time, progressCallback: progressCallback);
        public IStop StartTimer(float time, UnityAction endCallback, UnityAction<float> progressCallback) => Timer.Start(time, endCallback, progressCallback);

        public void Dispose()
        {
            // Stop all timers and clear the pool
            foreach (var timer in _timers)
            {
                timer.Stop();
            }
            _timers.Clear();
        }

        #endregion
    }
}
