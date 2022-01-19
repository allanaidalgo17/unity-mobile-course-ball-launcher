using UnityEngine;
using UnityEngine.InputSystem;

public class BallManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            return;
        }

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
    }
}
