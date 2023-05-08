using UnityEngine;

public class UserInputControl : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //LEFT
            if (Input.GetAxis(InputConst.mouseX) < 0)
            {
                EventManager.current.GoLeftTrigger();
            }

            //RIGHT
            if (Input.GetAxis(InputConst.mouseX) > 0)
            {
                EventManager.current.GoRightTrigger();
            }
        }
            

        //Touch - Tap
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    EventManager.current.TapTrigger();
                }
            }
        }

        // Mouse Left Click
        if (Input.GetMouseButtonDown(0))
        {
            EventManager.current.TapTrigger();
        }
    }
}
