using System;
using System.Collections;
using UnityEngine;

namespace CosmosDefender
{
    public class CronoScheduler : MonoSingleton<CronoScheduler>
    {
        protected override bool dontDestroyOnLoad => true;

        public Coroutine ScheduleForTime(float time, Action callback)
        {
            return StartCoroutine(CoroutineForTime(time, callback));
        }

        public void ScheduleForTimeAndExecuteElapsed(float maxTime, float elapsedTime, Action callback)
        {
            StartCoroutine(CoroutineForTime(maxTime, elapsedTime, callback));
        }

        public void ScheduleForRepetitions(int repetitions, float delay, Action callback)
        {
            StartCoroutine(CoroutineForAmount(repetitions, delay, callback));
        }


        private IEnumerator CoroutineForTime(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback.Invoke();
        }

        private IEnumerator CoroutineForTime(float maxTime, float elapsedTime, Action callback)
        {
            float timer = Time.time + maxTime;
            while (Time.time < timer)
            {
                callback.Invoke();
                yield return new WaitForSeconds(elapsedTime);
            }
        }

        private IEnumerator CoroutineForRealtime(float time, Action callback)
        {
            yield return new WaitForSecondsRealtime(time);
            callback.Invoke();
        }

        private IEnumerator CoroutineForAmount(int repetitions, float delayBetweenActions, Action callback)
        {
            int counter = repetitions;
            while (counter > 0)
            {
                callback.Invoke();
                counter--;
                yield return new WaitForSeconds(delayBetweenActions);
            }
        }
    }
}