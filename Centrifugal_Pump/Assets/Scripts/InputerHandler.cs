using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;


public delegate void MoveInput(Vector2 vector);
public delegate void ZoomInput(float direction);
public delegate void MousePressDown(string objName);
public delegate void MousePress(string objName, int direction);

public class InputerHandler : MonoBehaviour
{
    [SerializeField] float mouseWheelSensitivity = 1f;

    public event MoveInput OnMoveInput;
    public event ZoomInput OnZoomInput;
    public event MousePressDown OnMousePressDown;
    public event MousePress OnMousePress;

    public static InputerHandler instance;

    Collider cacheObj;
    public bool active = true;

    public void SetInputState(bool state)
    {
        active = state;
    }
    public bool GetActive()
    {
        return active;
    }


    void Awake()
    {
        instance = this;
    }
    void FixedUpdate()
    {
        if (active)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector2 direction;

                direction.x = Input.GetAxis("Horizontal");
                direction.y = Input.GetAxis("Vertical");

                OnMoveInput.Invoke(direction);
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                OnZoomInput.Invoke(mouseWheelSensitivity * Input.mouseScrollDelta.y);
            }
        }
    }
    void Update()
    {
        if (active)
        {
            RaycastHit hit = CastFromMouse();

            if (hit.point.magnitude != 0)
            {
                if (hit.collider.tag == "Button")
                {
                    if (hit.collider != cacheObj)
                    {
                        if (cacheObj != null)
                        {
                            cacheObj.GetComponent<Highlight>().SetHighlight(false);
                        }

                        cacheObj = hit.collider;
                        cacheObj.GetComponent<Highlight>().SetHighlight(true);
                    }
                }
                else
                {
                    if (cacheObj != null)
                    {
                        cacheObj.GetComponent<Highlight>().SetHighlight(false);
                        cacheObj = null;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (cacheObj != null)
                {
                    OnMousePressDown?.Invoke(cacheObj.name);
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (cacheObj != null)
                {
                    OnMousePress?.Invoke(cacheObj.name, 1);
                }
            }

            if (Input.GetMouseButton(1))
            {
                if (cacheObj != null)
                {
                    OnMousePress?.Invoke(cacheObj.name, -1);
                }
            }
        }

    }


    RaycastHit CastFromMouse()
    {
        RaycastHit hit;

        Vector2 mouse2d = Input.mousePosition;
        Vector3 mousePosition = new(mouse2d.x, mouse2d.y, 2f);

        Ray ray = Camera.main.ScreenPointToRay(mousePosition, Camera.MonoOrStereoscopicEye.Mono);

        Physics.Raycast(ray, out hit);

        return hit;
    }

}
