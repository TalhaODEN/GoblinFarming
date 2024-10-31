using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float gameTime;
    public float gameSpeed;
    public static TimeManager timeManager;
    public event Action OnTimeChange;

    private bool isTimePaused = false;
    private float timeScale = 1f;

    private void Awake()
    {
        if (timeManager == null)
        {
            timeManager = this;
        }
    }

    private void Start()
    {
        gameTime = 12f;
        StartCoroutine(UpdateGameTime());
    }

    private IEnumerator UpdateGameTime()
    {
        while (true)
        {
            if (!isTimePaused)
            {
                gameTime += (Time.deltaTime * timeScale * gameSpeed / 60f);
                if (gameTime >= 24f)
                {
                    gameTime -= 24f;
                }
                OnTimeChange?.Invoke();
            }
            yield return null;
        }
    }

    public void PauseTime()
    {
        isTimePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        isTimePaused = false;
        Time.timeScale = 1f;
    }

    public void SetTimeScale(float newScale)
    {
        timeScale = newScale;
        if (!isTimePaused)
        {
            Time.timeScale = newScale;
        }
    }

    public int GetCurrentHour()
    {
        return Mathf.FloorToInt(gameTime);
    }

    public int GetCurrentMinute()
    {
        return Mathf.FloorToInt((gameTime % 1) * 60);
    }

    private void OnDestroy()
    {
        OnTimeChange -= HandleTimeChange;
    }

    private void HandleTimeChange()
    {
    }
}
