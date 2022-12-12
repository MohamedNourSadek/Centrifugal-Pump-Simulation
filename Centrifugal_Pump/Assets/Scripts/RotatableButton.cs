using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


[System.Serializable]
public class RotatableButton
{
    [SerializeField] GameObject buttonObj;
    [SerializeField] Vector3 offRotation;
    [SerializeField] Vector3 onRotation;
    [SerializeField] float rotationSpeed = 1f;

    public void Rotate(bool state)
    {
        ServicesProvider.instance.StopAllCoroutines();
        ServicesProvider.instance.StartCoroutine(RotateButtonIenum(state ? onRotation : offRotation));
    }
    public void SetRotation(float value)
    {
        //value must be 0 - 1

        buttonObj.transform.rotation = Quaternion.Lerp(
                Quaternion.Euler(offRotation),
                Quaternion.Euler(onRotation),
                value);
    }
    IEnumerator RotateButtonIenum(Vector3 finalRotation)
    {
        while ((finalRotation - buttonObj.transform.rotation.eulerAngles).magnitude >= 1f)
        {
            buttonObj.transform.rotation = Quaternion.Lerp(
                            buttonObj.transform.rotation,
                            Quaternion.Euler(finalRotation),
                            Time.fixedDeltaTime * rotationSpeed);

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
}
