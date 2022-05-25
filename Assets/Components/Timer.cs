using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Components
{
    public class Timer : MonoBehaviour, ITimer
    {
        private float _currentTime;
        
        public bool IsCompleted
        {
            get
            {
                return (CurrentTime <= 0);
            }
        }
        public bool IsRunning { get; private set; }
    
        public float CurrentTime
        {
            get { return _currentTime; }
            private set
            {
                _currentTime = Mathf.Max(value, 0);
            }
    
        }
        
        public void StartTimer()
        {
            IsRunning = true;
        }
    
        public void Stop()
        {
            IsRunning = false;
        }
    
        public void SetTime(float time)
        {
            if (!IsRunning)
            {
                CurrentTime = time;
            }
        }
    
        private void Awake()
        {
            Stop();
        }
    
        private void Update()
        {
            if (IsRunning)
            {
                CurrentTime -= Time.deltaTime;
                if (IsCompleted)
                {
                    Stop();
                }
            }
        }
    }

}
