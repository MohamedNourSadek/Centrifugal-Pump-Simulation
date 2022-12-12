using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] GameObject highlightObj;
    public void SetHighlight(bool _state)
    {
        highlightObj.SetActive(_state);
    }
}
