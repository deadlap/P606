using UnityEngine;
using TMPro;
using System;
public class GameTimer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timer;
    [SerializeField] float timerDisc;
    [SerializeField] bool runTimer;
    [SerializeField] bool hasTriggeredObjectiveEvent;
    [SerializeField] UnityEngine.UI.Image image;
    
    AudioSource audioSource;
    Animator animator;

    public static GameTimer INSTANCE { get; private set; }
    public static event Action<bool> ToggleTimer;
    public static void OnToggleTimer(bool value) => ToggleTimer?.Invoke(value);

    bool timerTick0, timerTick1, timerTick2, timerTick3, timerTick4, timerTick5;
    void Start() {
        runTimer = false;
        timer = GameStats.INSTANCE.TimeLimit*60f;
        timerDisc = 0;
        hasTriggeredObjectiveEvent = false;
        INSTANCE = this;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void OnEnable() {
        ToggleTimer += ToggleRunTimer;
        Ending.EndGameEvent += StopTimer;
    }
    void OnDisable() {
        ToggleTimer -= ToggleRunTimer;
        Ending.EndGameEvent -= StopTimer;
    }

    void ToggleRunTimer(bool toggle){
        runTimer = toggle;
    }
    void StopTimer(){
        runTimer = false;
    }
    void Update()
    {
        if (runTimer){
            timer -= Time.deltaTime;
            timerDisc = GameStats.INSTANCE.TimeLimit * 60 - timer;
            UpdateDisplay();
            if (timer <= GameStats.INSTANCE.EventTriggerTime*60f && !hasTriggeredObjectiveEvent) {
                hasTriggeredObjectiveEvent = true;
                Objectives.OnChangeTextEvent(Objectives.ObjectiveEnum.AccuseMurderer);
            }
            if (timer <= GameStats.INSTANCE.TimeLimit*60 && !timerTick0)
            {
                animator.Play("TimerTick");
                timerTick0 = true;
            }
            if (timer <= 600 && !timerTick1)
            {
                animator.Play("TimerTick");
                timerTick1 = true;
            }
            if (timer <= 300 && !timerTick2)
            {
                animator.Play("TimerTick");
                timerTick2 = true;
            }
            if (timer <= 180 && !timerTick3)
            {
                animator.Play("TimerTick");
                timerTick3 = true;
            }
            if (timer <= 120 && !timerTick4)
            {
                animator.Play("TimerTick");
                timerTick4 = true;
            }
            if (timer <= 60 && !timerTick5)
            {
                animator.Play("TimerTick");
                timerTick5 = true;
            }
            if (timer <= 0)
                TimesUp();
        }
    }

    void UpdateDisplay(){
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        image.fillAmount = Mathf.Clamp01(timerDisc/(GameStats.INSTANCE.TimeLimit*60f));
        if (timerText != null)
            timerText.text = string.Format("Time left: {0:00}:{1:00}", minutes, seconds);
    }

    public void TimesUp(){
        Ending.OnEndGameEvent();

    }
    public bool IsTimeUp() {
        return timer <= 0;
    }
    public float GetTimeLeft() {
        return timer;
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}
