using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
public class Objectives : MonoBehaviour {

    public enum ObjectiveEnum{
        locateVictim,
        AccuseMurderer,
        UncoverMurderer,
    }

    [SerializeField] TextMeshProUGUI TitleTextElement;
    [SerializeField] TextMeshProUGUI ObjectiveTextElement;
    [SerializeField] Animator Animator;
    [SerializeField] List<string> ObjectiveText;
    [SerializeField] List<string> ObjectiveTitleText;
    ObjectiveEnum currentObjective;

    public static event Action<ObjectiveEnum> ChangeTextEvent;
    // public static event Action ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        currentObjective = 0;
        ChangeText();
    }
    void OnEnable() {
        ChangeTextEvent += UpdateText;
    }
    void OnDisable() {
        ChangeTextEvent -= UpdateText;
    }

    public void UpdateText(ObjectiveEnum objective) {
        currentObjective = objective;
        Animator.Play("ChangeObjective");
        Invoke("ChangeText", 1f);
    }
    void ChangeText(){
        ObjectiveTextElement.text = ObjectiveText[(int)currentObjective];
        TitleTextElement.text = ObjectiveTitleText[(int)currentObjective];
    }

}
