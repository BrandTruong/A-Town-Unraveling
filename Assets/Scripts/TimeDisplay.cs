using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    public int curInterval;
    public Text timeText;
    int hour;
    string amPm;
    // Update is called once per frame
    void Update()
    {
        // starts a 0 means !:00 0,1 is 6      2,3 is 7 
        GameObject timeOfDay = GameObject.Find("TimeManager");
        TimeManager interval = timeOfDay.GetComponent<TimeManager>();
        curInterval = interval.getInterval();
        hour = 6 + curInterval / 2;
        if (hour > 12)
        {
            hour -= 12;
            if (hour > 12)
            {
                hour -= 12;
            }
        }
        if ((0 <= curInterval && curInterval <= 11) || curInterval >= 36)
        {
            amPm = "AM";
        }
        else
        {
            amPm = "PM";
        }
        if (curInterval % 2 == 0 || curInterval == 0)
        {
            timeText.text = (hour + ":00 " + amPm).ToString();
        }
        else if (curInterval % 2 == 1)
        {
            timeText.text = (hour + ":30 " + amPm).ToString();
        }



    }
}
