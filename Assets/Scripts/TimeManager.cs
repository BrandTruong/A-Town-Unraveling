using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    private float timePast;
    public int curInterval;

    public float intervalLength = 1000f;
    public event Action<int> IntervalPass;

    private bool isFrozen = false;
    public GameObject shade;
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }
        timePast = 0f;
        curInterval = 0;
    }

    /*static void Instantiate()
    {
            if (instance == null) instance = new;
            else
            {
                Destroy(this);
            }
    }*/

    // Start is called before the first frame update

    public void OnIntervalPass(int numInterval)
    {
        IntervalPass?.Invoke(numInterval);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFrozen)
        {
            return;
        }
        timePast += Time.fixedDeltaTime;
        if(timePast >= intervalLength)
        {
            timePast = timePast % intervalLength;
            curInterval++;
            OnIntervalPass(curInterval);
            if(curInterval == 24)
            {
                shade.SetActive(true);
            }
        }
    }

    public int getInterval()
    {
        return curInterval;
    }

    public void setFrozen(bool state)
    {
        isFrozen = state;
    }
    // 6:00AM interval 0
    // curinterval++
    // 6:30AM interval 1
    // 6 + intervalNum/2

    public float intervalToHour(int interval)
    {
        return 6 + interval / 2 + interval % 2;
    }

    public int hourToInterval(float hour)
    {
        return (int)((hour - 6)*2);
    }
}
