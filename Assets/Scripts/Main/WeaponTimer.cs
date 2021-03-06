using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WeaponTimer : MonoBehaviour
{
    // Timer objects
    List<Timer> timers;
    // Timer removal queue
    List<int> removalPending;

    private int idCounter;

    /// <summary>
    /// Timer entity class
    /// </summary>
    class Timer
    {
        public int id;
        public bool isActive;

        public float rate;
        public int ticks;
        public int ticksElapsed;
        public float last;
        public Action callBack;
		public WeaponTimer timer;

        public Timer(int id_, float rate_, int ticks_, Action callback_,WeaponTimer wt)
        {
            id = id_;
            rate = rate_ < 0 ? 0 : rate_;
            ticks = ticks_ < 0 ? 0 : ticks_;
            callBack = callback_;
            last = 0;
            ticksElapsed = 0;
            isActive = true;
			timer = wt;
        }

        public void Tick()
        {
            last += Time.deltaTime;

            if (isActive && last >= rate)
            {
                last = 0;
                ticksElapsed++;
                callBack.Invoke();

                if (ticks > 0 && ticks == ticksElapsed)
                {
                    isActive = false;
                    timer.RemoveTimer(id);
                }
            }
        }
    }

    /// <summary>
    /// Awake
    /// </summary>
    void Awake()
    {
        timers = new List<Timer>();
        removalPending = new List<int>();
    }

    /// <summary>
    /// Creates new timer
    /// </summary>
    /// <param name="rate">Tick rate</param>
    /// <param name="callBack">Callback method</param>
    /// <returns>Time GUID</returns>
    public int AddTimer(float rate, Action callBack)
    {
        return AddTimer(rate, 0, callBack);
    }

    /// <summary>
    /// Creates new timer
    /// </summary>
    /// <param name="rate">Tick rate</param>
    /// <param name="ticks">Number of ticks before timer removal</param>
    /// <param name="callBack">Callback method</param>
    /// <returns>Timer GUID</returns>
    public int AddTimer(float rate, int ticks, Action callBack)
    {
        Timer newTimer = new Timer(++idCounter, rate, ticks, callBack, this);
        timers.Add(newTimer);
        return newTimer.id;
    }

    /// <summary>
    /// Removes timer
    /// </summary>
    /// <param name="timerId">Timer GUID</param>
    public void RemoveTimer(int timerId) { removalPending.Add(timerId); }

    /// <summary>
    /// Timer removal queue handler
    /// </summary>
    void Remove()
    {
        if (removalPending.Count > 0)
        {
            foreach (int id in removalPending)
                for (int i = 0; i < timers.Count; i++)
                    if (timers[i].id == id)
                    {
                        timers.RemoveAt(i);
                        break;
                    }

            removalPending.Clear();
        }
    }

    /// <summary>
    /// Updates timers
    /// </summary>
    void Tick()
    {
		if (timers != null) {
			for (int i = 0; i < timers.Count; i++)
				timers [i].Tick ();
		}
    }

    // Update is called once per frame
    void Update()
    {
        Remove();
        Tick();
    }
}