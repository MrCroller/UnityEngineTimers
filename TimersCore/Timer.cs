using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace UnityEngineTimers
{
    public sealed class Timer : IStop
    {

        #region Fields

        public UnityEvent OnEndTime;
        /// <summary>
        /// Return a floating point number from 0.0f to 1.0f
        /// </summary>
        public UnityEvent<float> OnProgressTick;

        /// <summary>
        /// Is the timer running?
        /// </summary>
        public bool IsRunning { get => _coroutine != null; }

        private Coroutine _coroutine = null;

        #endregion


        #region CodeLifeCycles

        public Timer()
        {
            OnEndTime = new UnityEvent();
            OnProgressTick = new UnityEvent<float>();
        }

        #endregion


        #region Methods

        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="method">Method on the end of the timer to be run</param>
        /// <param name="time">Timer running time</param>
        public IStop Start(UnityAction method, float time, bool unscale = false)
        {
            if (IsRunning)
            {
                Stop();
            }

            OnEndTime.AddListener(method);
            _coroutine = Coroutines.StartRoutine(unscale ? UnscaleTicker(time) : Ticker(time));
            return this;
        }

        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="method">Method on the end of the timer to be run</param>
        /// <param name="timeTickMethod">Method returning progress values from 0.0f to 1.0f</param>
        /// <param name="time">Timer running time</param>
        public IStop Start(UnityAction method, UnityAction<float> timeTickMethod, float time, bool unscale = false)
        {
            if (IsRunning)
            {
                Stop();
            }

            OnEndTime.AddListener(method);
            OnProgressTick.AddListener(timeTickMethod);
            _coroutine = Coroutines.StartRoutine(unscale ? UnscaleTicker(time) : Ticker(time));
            return this;
        }

        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="timeTickMethod">Method returning progress values from 0.0f to 1.0f</param>
        /// <param name="time">Timer running time</param>
        public IStop Start(UnityAction<float> timeTickMethod, float time, bool unscale = false)
        {
            if (IsRunning)
            {
                Stop();
            }

            OnProgressTick.AddListener(timeTickMethod);
            _coroutine = Coroutines.StartRoutine(unscale ? UnscaleTicker(time) : Ticker(time));
            return this;
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                Coroutines.StopRoutine(_coroutine);
                OnProgressTick?.RemoveAllListeners();
                OnEndTime?.RemoveAllListeners();
                _coroutine = null;
            }
        }

        private IEnumerator Ticker(float maxTime)
        {
            float time = maxTime;
            do
            {
                time -= Time.deltaTime;
                OnProgressTick?.Invoke(Mathf.Lerp(0.0f, 1.0f, (maxTime - time) / maxTime));
                yield return null;
            } while (time > 0.0f);

            OnEndTime?.Invoke();

            OnProgressTick?.RemoveAllListeners();
            OnEndTime?.RemoveAllListeners();
            _coroutine = null;
        }

        private IEnumerator UnscaleTicker(float maxTime)
        {
            float time = maxTime;
            do
            {
                time -= Time.unscaledDeltaTime;
                OnProgressTick?.Invoke(Mathf.Lerp(0.0f, 1.0f, (maxTime - time) / maxTime));
                yield return null;
            } while (time > 0.0f);

            OnEndTime?.Invoke();

            OnProgressTick?.RemoveAllListeners();
            OnEndTime?.RemoveAllListeners();
            _coroutine = null;
        }

        #endregion

    }
}