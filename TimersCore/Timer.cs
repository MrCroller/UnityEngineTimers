using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngineTimers
{
    public sealed class Timer : IStop, IDisposable
    {
        #region Fields

        public UnityEvent OnEndTime { get; private set; }
        public UnityEvent<float> OnProgressTick { get; private set; }

        public bool IsRunning => _coroutine != null;

        private Coroutine _coroutine;

        #endregion

        #region Constructors

        public Timer()
        {
            OnEndTime = new UnityEvent();
            OnProgressTick = new UnityEvent<float>();
        }

        public void Dispose()
        {
            ClearListeners();

            OnEndTime = null;
            OnProgressTick = null;
        }

        #endregion

        #region Methods

        //// <summary>
        /// Starts the timer with scaled time.
        /// </summary>
        /// <param name="time">Duration of the timer in seconds.</param>
        /// <param name="endCallback">Method to be invoked at the end of the timer (optional).</param>
        /// <param name="progressCallback">Method to be invoked with progress updates (optional).</param>
        /// <returns>Returns the Timer instance for chaining.</returns>
        public IStop Start(float time = 1f, UnityAction endCallback = null, UnityAction<float> progressCallback = null)
        {
            return StartInternal(time, endCallback, progressCallback, false);
        }

        /// <summary>
        /// Starts the timer with unscaled time.
        /// </summary>
        /// <param name="time">Duration of the timer in seconds.</param>
        /// <param name="endCallback">Method to be invoked at the end of the timer (optional).</param>
        /// <param name="progressCallback">Method to be invoked with progress updates (optional).</param>
        /// <returns>Returns the Timer instance for chaining.</returns>
        public IStop StartUnscaled(float time = 1f, UnityAction endCallback = null, UnityAction<float> progressCallback = null)
        {
            return StartInternal(time, endCallback, progressCallback, true);
        }

        private IStop StartInternal(float time, UnityAction endCallback, UnityAction<float> progressCallback, bool unscaled)
        {
            if (IsRunning)
            {
                Stop();
            }

            if (endCallback != null) OnEndTime.AddListener(endCallback);
            if (progressCallback != null) OnProgressTick.AddListener(progressCallback);

            _coroutine = Coroutines.StartRoutine(unscaled ? UnscaledTicker(time) : Ticker(time));
            return this;
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                Coroutines.StopRoutine(_coroutine);
                ClearListeners();
                _coroutine = null;
            }
        }

        private void ClearListeners()
        {
            OnProgressTick?.RemoveAllListeners();
            OnEndTime?.RemoveAllListeners();
        }

        private IEnumerator Ticker(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                OnProgressTick?.Invoke(Mathf.Clamp01(elapsedTime / duration));
                yield return null;
            }

            OnEndTime?.Invoke();
            ClearListeners();
        }

        private IEnumerator UnscaledTicker(float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                OnProgressTick?.Invoke(Mathf.Clamp01(elapsedTime / duration));
                yield return null;
            }

            OnEndTime?.Invoke();
            ClearListeners();
        }

        #endregion
    }
}
