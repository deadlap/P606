using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
public class GameTimer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timer;
    [SerializeField] bool runTimer;
    [SerializeField] bool hasTriggeredObjectiveEvent;
    [SerializeField] UnityEngine.UI.Image image;
    public static event Action<bool> ToggleTimer;
    public static void OnToggleTimer(bool value) => ToggleTimer?.Invoke(value);
    void Start() {
        runTimer = false;
        timer = GameStats.INSTANCE.TimeLimit*60f;
        hasTriggeredObjectiveEvent = false;
    }

    void OnEnable() {
        ToggleTimer += ToggleRunTimer;
    }
    void OnDisable() {
        ToggleTimer -= ToggleRunTimer;
    }

    void ToggleRunTimer(bool toggle){
        runTimer = toggle;
    }
    void Update()
    {
        if (runTimer){
            timer -= Time.deltaTime;
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
        image.fillAmount = timer/GameStats.INSTANCE.TimeLimit;
        if (timerText != null)
            timerText.text = string.Format("Time left: {0:00}:{1:00}", minutes, seconds);
    }

    public void TimesUp(){

    }
}
