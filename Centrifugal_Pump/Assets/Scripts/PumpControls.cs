using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PumpControls : MonoBehaviour
{
    [Header("References")]

    [Header("Buttons")]
    [SerializeField] RotatableButton powerButton;
    [SerializeField] RotatableButton pumpButton;
    [SerializeField] RotatableButton speedButton;
    [SerializeField] RotatableButton leftGauge;
    [SerializeField] RotatableButton rightGauge;
    [SerializeField] RotatableButton waterControlValve;
    [SerializeField] RotatableButton slidingValve;

    [Header("Pump Power Screens UI")]
    [SerializeField] TextMeshProUGUI pumpPowerText;
    [SerializeField] TextMeshProUGUI pumpSpeedText;
    [SerializeField] GameObject screenOnObject;

    [Header("Other")]
    [SerializeField] GameObject pumpImpeller;
    [SerializeField] GameObject pipeWater;
    [SerializeField] GameObject meterWater;
    [SerializeField] List<GameObject> impellerWater;

    [Header("Audio")]
    [SerializeField] float volume;

    [Header("Values")]
    [SerializeField] float pumpSpeedControlSpeed = 1f;
    [SerializeField] float waterControlValveSpeed = 1f;
    [SerializeField] float pumpSpeedControlPressureConstant = 1f;
    [SerializeField] float waterControlValvePrssureConstnat = 1f;
    [SerializeField] float pumpImpellerSpeedConstant = 1f;
    [SerializeField] float meterSpeed;
    [SerializeField] float impellerWaterSpeed = 1f;
    [SerializeField] Vector3 pumpImpellerRotationAxes;
    [SerializeField] Vector3 pipeWaterOff;
    [SerializeField] Vector3 pipeWaterOn;


    AudioSource engineAudio;

    bool powerState;
    bool pumpState;
    bool sliderValveState;
    float pumpSpeedValue;
    float controlWaterValve;
    float liquidMeterValue; 



    private void Awake()
    {
        engineAudio = gameObject.GetComponent<AudioSource>();
        InputerHandler.instance.OnMousePressDown += OnActionRequest;
        InputerHandler.instance.OnMousePress += OnContinousAction;
    }
    private void FixedUpdate()
    {
        //pump audio
        engineAudio.volume = volume * pumpSpeedValue;

        //Pump speed
        //Reduce automatically if not used
        if (!powerState || !pumpState)
            pumpSpeedValue = Mathf.Clamp01(pumpSpeedValue - Time.fixedDeltaTime);
        pumpSpeedText.text = ((int)(pumpSpeedValue * 1410)).ToString();

        //impeller
        pumpImpeller.transform.Rotate(pumpImpellerRotationAxes, pumpSpeedValue * pumpImpellerSpeedConstant);

        //impeller water
        int impellerWaterState = pumpSpeedValue > 0 ? 1 : -1;
        float waterIncrement = impellerWaterState *  impellerWaterSpeed * Time.fixedDeltaTime;
        foreach (GameObject water in impellerWater)
        {
            water.transform.localScale = Vector3.one * Mathf.Clamp01((water.transform.localScale.x + waterIncrement));
        }

        //water pipe
        pipeWater.transform.localScale = Vector3.Lerp(pipeWaterOff, pipeWaterOn, pumpSpeedValue * controlWaterValve);

        //Gauges
        leftGauge.SetRotation((pumpSpeedControlPressureConstant*pumpSpeedValue) + (pumpSpeedControlPressureConstant * controlWaterValve));
        rightGauge.SetRotation(pumpSpeedValue - (waterControlValvePrssureConstnat * controlWaterValve));

        //Meter
        float increment = 0f;


        if(sliderValveState)
        {
            if(powerState && pumpState)
            {
                increment = (meterSpeed * controlWaterValve * pumpSpeedValue * Time.fixedDeltaTime);
            }
            else
            {
                increment = 0f;
            }
        }
        else
        {
            increment = (-meterSpeed * Time.fixedDeltaTime);
        }

        liquidMeterValue = Mathf.Clamp01(liquidMeterValue + increment);
        meterWater.transform.localScale = new Vector3(meterWater.transform.localScale.x, meterWater.transform.localScale.y, liquidMeterValue);
    }
    void OnActionRequest(string actionName)
    {
        if(actionName == "powerButton")
            SetPowerState(!powerState);
        else if (actionName == "pumpButton")
            SetPumpState(!pumpState);
        else if (actionName == "sliderButton")
            SetSliderValve(!sliderValveState);
    }
    void OnContinousAction(string actionName, int direction)
    {
        if (actionName == "pumpSpeedButton")
            ChangePumpSpeed(direction * (pumpSpeedControlSpeed * Time.fixedDeltaTime));
        else if (actionName == "waterValveButton")
            ChangeWaterControlSpeed(direction * (waterControlValveSpeed * Time.fixedDeltaTime));
    }


    //Pump Functionality
    void SetPowerState(bool state)
    {
        powerState = state;

        //Switch
        powerButton.Rotate(state);

        //Screen
        screenOnObject.SetActive(state);

        //Pump Values
        if (state)
        {
            pumpSpeedText.text = "0";
            pumpPowerText.text = "220 V";
        }
        else
        {
            pumpSpeedText.text = "";
            pumpPowerText.text = "";
        }
    }
    void SetPumpState(bool state)
    {
        pumpState = state;
        pumpButton.Rotate(state);
    }
    void ChangeWaterControlSpeed(float delta)
    {
        waterControlValve.SetRotation(controlWaterValve);
        controlWaterValve = Mathf.Clamp01(controlWaterValve + delta);
    }
    void ChangePumpSpeed(float delta)
    {
        if (pumpState && powerState)
        {
            pumpSpeedValue = Mathf.Clamp01(pumpSpeedValue + delta);
            speedButton.SetRotation(pumpSpeedValue);
        }
    }
    void SetSliderValve(bool state)
    {
        sliderValveState = state;
        slidingValve.Rotate(state);
    }
}
