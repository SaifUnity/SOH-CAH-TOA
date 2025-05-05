using UnityEngine;

public class timeManager : MonoBehaviour
{
    public float timeScaleFactor = 0.05f;
    public float musicSlowFactor = 0.5f;

    public float transitionSpeedEnter = 0.5f;
    public float transitionSpeedExit = 1f;

    public float timeDecreaseFactor = 0.5f;
    public float timeSlowDuration = 4f;
    public float timeSlowCap = 1f;

    public AudioSource gameMusic;

    private bool isTimeSlowed = false;
    void Update()
    {

        timeSlowCap += (1f / timeSlowDuration) * Time.unscaledDeltaTime;
        timeSlowCap = Mathf.Clamp(timeSlowCap, 0f, 1f);

        if (timeSlowCap == 1f)
        {
            isTimeSlowed = false;
        }

        if (isTimeSlowed == false)
        {
            Time.timeScale += (1f / transitionSpeedExit) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
        if (isTimeSlowed == true)
        {
            Time.timeScale -= (1f / transitionSpeedEnter) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, timeScaleFactor, 1f);
        }

        if (Time.timeScale == 1f)
        {
            isTimeSlowed = false;
        }

        if (isTimeSlowed == false)
        {
            gameMusic.pitch += (1f / transitionSpeedExit) * Time.unscaledDeltaTime;
            gameMusic.pitch = Mathf.Clamp(gameMusic.pitch, 0.1f, 1f);
        }

        if (isTimeSlowed == true)
        {
            gameMusic.pitch -= (1f / transitionSpeedEnter) * Time.unscaledDeltaTime;
            gameMusic.pitch = Mathf.Clamp(gameMusic.pitch, musicSlowFactor, 1f);
        }
        
    }

    public void SlowTime()
    {
        isTimeSlowed = true;
        timeSlowCap = timeDecreaseFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.001f;
    }
}
