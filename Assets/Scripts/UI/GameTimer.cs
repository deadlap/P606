using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
public class GameTimer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timer;
    [SerializeField] bool runTimer;
    public static event Action<bool> ToggleTimer;
    void Start() {
        runTimer = false;
        timer = GameStats.INSTANCE.TimeLimit;
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
            if (timer <= 0)
                TimesUp();
        }
    }

    void UpdateDisplay(){
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        if (timerText != null)
            timerText.text = string.Format("Time left: {0:00}:{1:00}", minutes, seconds);
    }

    public void TimesUp(){

    }
}
