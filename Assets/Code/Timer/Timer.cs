using System;
using Code.Utils;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Code.Timer
{
    [Serializable]
    public class FloatEvent : UnityEvent<float> { }
    
    public class Timer: MonoBehaviour
    {
        [SerializeField] private float duration;
        private float _finalDuration;
        public FloatRange randomRange;
        private TimerIcon _timerIcon;
        private float _timeSpent;
        
        [SerializeField] private UnityEvent onStart;
        [SerializeField] private FloatEvent onFinish;   
        [SerializeField] private FloatEvent onTick;
        
        private bool _isActive;
        
        void FixedUpdate()
        {
            if (!_isActive) return;

            _timeSpent += Time.deltaTime;
            
            float progress = Mathf.Clamp01(_timeSpent / _finalDuration);
            onTick?.Invoke(progress);
            
            if (_timeSpent >= _finalDuration)
            {
                _isActive = false;
                _timeSpent = 0f;
                onFinish?.Invoke(_finalDuration);
            }
        }

        public void AddDuration(float duration)
        {
            _finalDuration += duration;
        }

        public bool StartTimer()
        {
            if (duration <= 0)
            {
                return false;
            }
            _finalDuration = duration + Random.Range(randomRange.min, randomRange.max);
            _isActive = true;
            onStart?.Invoke();
            return true;
        }
        
        public void Stop()
        {
            _isActive = false;
        }
        
        // public bool SubscribeFinish(Action<float> callback)
        // {
        //     if (callback == null) return false;
        //     _subscriptionsFinish += callback;
        //     return true;
        // }
        
        // public bool SubscribeTick(Action<float> callback)
        // {
        //     if (callback == null) return false;
        //     _subscriptionsTick += callback;
        //     return true;
        // }
    }

    public class TimerIcon : MonoBehaviour
    {
        public SpriteMask mask;
        float _timeLeft;

        public void UpdateTimer(float timeSpent, float timeMax)
        {
            _timeLeft = timeMax - timeSpent;
            _timeLeft = Mathf.Max(_timeLeft, 0f);

            float t = 1f - (_timeLeft / timeMax);

            // 0 deg = full, 360 deg = empty 
            float angle = t * 360f;
            mask.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}