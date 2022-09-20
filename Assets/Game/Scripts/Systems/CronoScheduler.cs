using System;
using System.Collections;
using UnityEngine;

namespace CosmosDefender
{
    public class CronoScheduler : MonoSingleton<CronoScheduler>
    {
        protected override bool dontDestroyOnLoad => true;

        public void ScheduleForTime(float time, Action callback)
        {
            StartCoroutine(CoroutineForTime(time, callback));
        }

        public void ScheduleForTimeAndExecuteElapsed(float maxTime, float elapsedTime, Action callback)
        {
            StartCoroutine(CoroutineForTime(maxTime, elapsedTime, callback));
        }

        private IEnumerator CoroutineForTime(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback.Invoke();
        }

        private IEnumerator CoroutineForTime(float maxTime, float elapsedTime, Action callback)
        {
            float timer = 0;
            while (timer < maxTime)
            {
                yield return new WaitForSeconds(elapsedTime);
                callback.Invoke();
            }
        }
    }
}