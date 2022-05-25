using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public interface ITimer
    {
        bool IsCompleted { get; }
        bool IsRunning { get; }
        float CurrentTime { get; }
    
        void StartTimer();
        void Stop();
        void SetTime(float time);
    }
}

