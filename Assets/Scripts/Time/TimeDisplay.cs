using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI hourText;
    public TextMeshProUGUI minuteText;
    private int lastDisplayedMinute = 0;
    private void Start()
    {
        TimeManager timeManager = FindObjectOfType<TimeManager>();
        if (timeManager != null)
        {
            timeManager.OnTimeChange += UpdateTimeDisplay;
            UpdateTimeDisplay();
        }
        else
        {
            Debug.LogError("TimeManager not found in the scene!");
        }
    }

    private void UpdateTimeDisplay()
    {
        TimeManager timeManager = TimeManager.timeManager;
        if (timeManager != null)
        {
            int hours = timeManager.GetCurrentHour();
            int minutes = timeManager.GetCurrentMinute();

            if (minutes % 10 == 0 && minutes != lastDisplayedMinute)
            {
                hourText.text = $"{hours:D2}";
                minuteText.text = $"{minutes:D2}";
                lastDisplayedMinute = minutes; 
            }
        }
    }

    private void OnDestroy()
    {
        TimeManager.timeManager.OnTimeChange -= UpdateTimeDisplay;
    }
}
