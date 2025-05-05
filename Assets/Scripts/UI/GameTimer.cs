using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
public class GameTimer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timer;
    [SerializeField] float timerDisc;
    [SerializeField] bool runTimer;
    [SerializeField] bool hasTriggeredObjectiveEvent;
    [SerializeField] UnityEngine.UI.Image image;
    public static GameTimer INSTANCE { get; private set; }
    public static event Action<bool> ToggleTimer;
    public static void OnToggleTimer(bool value) => ToggleTimer?.Invoke(value);
    void Start() {
        runTimer = false;
        timer = GameStats.INSTANCE.TimeLimit*60f;
        timerDisc = 0;
        hasTriggeredObjectiveEvent = false;
        INSTANCE = this;
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
            timerDisc += Time.deltaTime;
            UpdateDisplay();
            if (timer <= GameStats.INSTANCE.EventTriggerTime*60f && !hasTriggeredObjectiveEvent) {
                hasTriggeredObjectiveEvent = true;
                Objectives.OnChangeTextEvent(Objectives.ObjectiveEnum.AccuseMurderer);
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
}
