using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] Vector2 horizontalMovementConstrains;
    [SerializeField] Vector2 verticalMovementConstrains;
    [SerializeField] float zoomConstant = 1f;

    [SerializeField] float delay;
    [SerializeField] float speed;

    //Private data
    Vector3 originalPosition;
    Vector3 finalPosition;
    float zoomFinalValue;

    void Awake()
    {
        InputerHandler.instance.OnMoveInput += OnMoveInput;
        InputerHandler.instance.OnZoomInput += OnDeltaZoom;
        UiControl.instance.AddSliderOnChange(OnZoom);

        originalPosition= transform.position;
        finalPosition = originalPosition;
    }
    void FixedUpdate()
    {
        //Update position but delayed (it gives a nice smooth look)

        this.transform.position = Vector3.Lerp(this.transform.position, finalPosition, Time.fixedDeltaTime / delay);
    }


    //Event handlers
    void OnZoom(float finalValue)
    {
        zoomFinalValue = finalValue;
        finalPosition.z = zoomConstant * (originalPosition.z + finalValue);
    }
    void OnDeltaZoom(float deltaValue)
    {
        zoomFinalValue = Mathf.Clamp01(zoomFinalValue + deltaValue);
        UiControl.instance.SetSliderValue(zoomFinalValue);
    }
    void OnMoveInput(Vector2 directions)
    {
        if(CanMove(originalPosition.x, transform.position.x,directions.x,horizontalMovementConstrains))
        {
            finalPosition += new Vector3(directions.x * speed, 0f, 0f);
        }

        if(CanMove(originalPosition.y, transform.position.y, directions.y, verticalMovementConstrains))
        {
            finalPosition += new Vector3(0f, directions.y * speed,0f);
        }
    }


    //Internal Algorithms
    bool CanMove(float original, float current, float direction, Vector2 relativeLimits)
    {
        //Limit.y is the positive limit
        //Limit.x is the negative limit

        Vector2 limits = new Vector2(original + relativeLimits.x, original + relativeLimits.y);


        bool outSidePositive = (current > limits.y);
        bool outSideNegative = (current < limits.x);
        bool positiveDirection = direction > 0;

        if(!outSidePositive && !outSideNegative)
        {
            return true;
        }
        else if(outSidePositive && positiveDirection)
        {
            return false;
        }
        else if(outSideNegative && !positiveDirection)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
