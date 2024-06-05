using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Tooltip("The amount of seconds it take for a min to pass in-game")]
    public float multiplier = 1;

    [Header("Skybox Materials")]
    [SerializeField] Material Dawn;

    [SerializeField] Material DawnToDay;
    [SerializeField] Material Day;
    [SerializeField] Material DayToSunset;
    TMP_Text GameTimeText;
    GameTimeUI GameTimeUI;
    [SerializeField] GameObject GameTimeUIPrefab;
    float hours, minutes;
    bool isMidDayChanged;
    MidDay midDay;
    [SerializeField] Material Midnight;
    [Header("Skybox Blend Materials")]
    [SerializeField] Material MidnightToDawn;

    [SerializeField] bool PauseTimer = false;
    float seconds;
    [Tooltip("the time taken (mins) to transition based on in-game time")]
    [SerializeField] float SkyboxDuration;

    [SerializeField] Material Sunset;
    [SerializeField] Material SunsetToMidnight;
    TimeOfDay timeOfDay;

    enum MidDay
    {
        AM,
        PM
    }

    enum TimeOfDay
    {
        Midnight,
        Dawn,
        Day,
        Sunset,
    }
    public bool getPauseTimer()
    {
        return PauseTimer;
    }

    public void SetPauseTimer(bool setTimerTo)
    {
        PauseTimer = setTimerTo;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        seconds = 0;
        hours  = 12;
        minutes = 0;
        midDay = MidDay.AM;
        isMidDayChanged = true;
        timeOfDay = TimeOfDay.Midnight;


    }

    IEnumerator LerpSkybox(TimeOfDay nextTimeOfDay, float duration)
    {
        Material blendMaterial = null;
        switch (nextTimeOfDay)
        {
            case TimeOfDay.Midnight:
                blendMaterial = SunsetToMidnight;
                break;
            case TimeOfDay.Dawn:
                blendMaterial = MidnightToDawn;
                break;
            case TimeOfDay.Day:
                blendMaterial = DawnToDay;
                break;
            case TimeOfDay.Sunset:
                blendMaterial = DayToSunset;
                break;
        }

        if (blendMaterial != null)
        {
            RenderSettings.skybox = blendMaterial;
            RenderSettings.skybox.SetFloat("_BlendCubemaps", 0);

            float elapsedTime = 0;
            float _duration = duration * 60;
            while (elapsedTime < _duration)
            {
                elapsedTime += Time.deltaTime * (60 * multiplier);
                RenderSettings.skybox.SetFloat("_BlendCubemaps", elapsedTime / _duration);
                Debug.Log(elapsedTime + "" + _duration);
                yield return null;
            }

            SetSkyBox(nextTimeOfDay);

        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = UIManager.Instance.canvas;
        var findGameTimeInScene = canvas.GetComponentsInChildren<GameTimeUI>();
        if (findGameTimeInScene.Length > 0)
        {
            GameTimeUI = findGameTimeInScene[0];
        }
        else
        {
            GameObject GameTimerGO = Instantiate(GameTimeUIPrefab);
            GameTimerGO.transform.parent = UIManager.Instance.canvas.transform;
            GameTimerGO.transform.localPosition = Vector3.zero;


            GameTimeUI = GameTimerGO.GetComponent<GameTimeUI>();
        }

        GameTimeText = GameTimeUI.gameTimerText;

    }

    void ResetTime()
    {
        hours = 0;
        minutes = 0;
        seconds = 0;
    }

    void SetSkyBox(TimeOfDay _timeOfDay)
    {

        switch (_timeOfDay)
        {
            case TimeOfDay.Midnight:
                timeOfDay = TimeOfDay.Midnight;
                RenderSettings.skybox = Midnight;
                break;
            case TimeOfDay.Dawn:
                timeOfDay = TimeOfDay.Dawn;
                RenderSettings.skybox = Dawn;
                break;
            case TimeOfDay.Day:
                timeOfDay = TimeOfDay.Day;
                RenderSettings.skybox = Day;
                break;
            case TimeOfDay.Sunset:
                timeOfDay = TimeOfDay.Sunset;
                RenderSettings.skybox = Sunset;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!getPauseTimer())
        {
            seconds += Time.deltaTime * (60 * multiplier);
            // 60 sec in game time = (multiplayer) sec irl time
            // 60 / (multiplayer) sec in-game time = 1 sec irl time
            UpdateTimeUI();
        }
        
    }

    void UpdateAMPM()
    {
        if (hours == 12 && !isMidDayChanged)
        {
            switch (midDay)
            {
                case MidDay.AM:
                    midDay = MidDay.PM;
                    break;

                case MidDay.PM:
                    midDay = MidDay.AM;
                    break;
            }

            isMidDayChanged = true;
        }
        else if (hours > 12)
        {
            hours = 1;
            isMidDayChanged = false;
            // a new day yipee
        }
    }

    void UpdateHours()
    {
        if (minutes >= 60)
        {
            hours += 1;
            minutes = 0;
            UpdateSkyBox();
        }
    }

    void UpdateMinutes()
    {
        if (seconds >= 60)
        {
            minutes += 1;
            seconds = 0;
        }
    }

    void UpdateSkyBox()
    {

        if (hours == 5 && minutes == 0 && midDay == MidDay.AM) // midnight to dawn
        {
            StartCoroutine(LerpSkybox(TimeOfDay.Dawn, SkyboxDuration));
        }
        else if (hours == 7 && minutes == 0 && midDay == MidDay.AM) // dawn to day
        {
            StartCoroutine(LerpSkybox(TimeOfDay.Day, SkyboxDuration));
        }
        else if (hours == 6 && minutes == 0 && midDay == MidDay.PM) // day to sunset
        {
            StartCoroutine(LerpSkybox(TimeOfDay.Sunset, SkyboxDuration));
        }
        else if (hours == 8 && minutes == 0 && midDay == MidDay.PM) // sunset to midnight
        {
            StartCoroutine(LerpSkybox(TimeOfDay.Midnight, SkyboxDuration));
        }


    }

    void UpdateTimeUI()
    {
        UpdateHours();
        UpdateMinutes();
        UpdateAMPM();
        UpdateSkyBox();
        GameTimeText.text = string.Format("{0}:{1} {2}", hours, minutes.ToString("00"), System.Enum.GetName(typeof(MidDay), midDay));

        Debug.Log(GameTimeUI.gameObject.name);
    }
}
