using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum Mode { game, help}

public class UiControl : MonoBehaviour
{
    [SerializeField] Slider zoomSlider;
    [SerializeField] GameObject helpPanel;
    [SerializeField] GameObject gamePanel;

    public static UiControl instance;

    Mode currentMode = Mode.game;


    private void Awake()
    {
        instance = this;
    }
    public void ToggleHelpPanel()
    {
        if(currentMode== Mode.game)
        {
            currentMode = Mode.help;
            InputerHandler.instance.SetInputState(false);

            gamePanel.SetActive(false);
            helpPanel.SetActive(true); 
        }
        else if(currentMode == Mode.help)
        {
            currentMode = Mode.game;
            InputerHandler.instance.SetInputState(true);

            gamePanel.SetActive(true);
            helpPanel.SetActive(false);
        }
    }
    public void SetSliderValue(float value)
    {
        zoomSlider.value = value;   
    }
    public void AddSliderOnChange(UnityAction<float> eventCall)
    {
        zoomSlider.onValueChanged.AddListener(eventCall);
    }
}
