using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public event Action GoLeft;
    public event Action GoRight;
    public event Action Tap;

    private void Awake()
    {
        if (current == null)
            current = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void GoLeftTrigger()
    {
        GoLeft?.Invoke();
    }

    public void GoRightTrigger()
    {
        GoRight?.Invoke();
    }

    public void TapTrigger()
    {
        Tap?.Invoke();
    }
}
