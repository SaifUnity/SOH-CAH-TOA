using UnityEngine;

public class timeManager : MonoBehaviour
{

    public static timeManager Instance { get; private set; }


    public float timeScaleFactor = 0.05f;
    public float musicSlowFactor = 0.5f;
    public float backgroundBrightnessFactor = 0.5f;

    public float brightness = 1f;

    public float transitionSpeedEnter = 0.5f;
    public float transitionSpeedExit = 1f;

    public float timeDecreaseFactor = 0.5f;
    public float timeSlowDuration = 4f;
    public float timeSlowCap = 1f;

    public AudioSource gameMusic;

    public SpriteRenderer sohPrefab;
    public SpriteRenderer cahPrefab;
    public SpriteRenderer toaPrefab;

    private bool isTimeSlowed = false;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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


        if (isTimeSlowed == false)
        {
            brightness += (1f / transitionSpeedExit) * Time.unscaledDeltaTime;
            brightness = Mathf.Clamp(brightness,backgroundBrightnessFactor , 1f);
        }

        if (isTimeSlowed == true)
        {
            brightness -= (1f / transitionSpeedEnter) * Time.unscaledDeltaTime;
            brightness = Mathf.Clamp(brightness, backgroundBrightnessFactor, 1f);
        }
    }

    public void SlowTime()
    {
        isTimeSlowed = true;
        timeSlowCap = timeDecreaseFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.001f;
    }
}
