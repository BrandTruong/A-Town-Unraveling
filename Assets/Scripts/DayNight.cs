using UnityEngine;
using UnityEngine.UI;

public class DayNight : MonoBehaviour
{
    public int curInterval;
    public Text dayNight;

    // Update is called once per frame
    void Update()
    {
        GameObject timeOfDay = GameObject.Find("TimeManager");
        TimeManager interval = timeOfDay.GetComponent<TimeManager>();
        curInterval = interval.getInterval();

        if (curInterval >= 0 && curInterval <= 23)
        {
            dayNight.text = "Daytime";
        }
        else
        {
            dayNight.text = "Nighttime";
        }
    }
}
