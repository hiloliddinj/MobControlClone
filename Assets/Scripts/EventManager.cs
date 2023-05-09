using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    public event Action GoLeft;
    public event Action GoRight;
    public event Action Tap;

    public event Action GunGenerate;

    public event Action KillAllCharacters;

    public event Action<int> BlueScoreIncrease;
    public event Action<int> YellowScoreIncrease;

    public event Action GoToNextInLevel;

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

    public void GunGenerateTrigger()
    {
        GunGenerate?.Invoke();
    }

    public void KillAllCharactersTrigger()
    {
        KillAllCharacters?.Invoke();
    }

    public void BlueScoreIncreaseTrigger(int amount)
    {
        BlueScoreIncrease?.Invoke(amount);
    }

    public void YellowScoreIncreaseTrigger(int amount)
    {
        YellowScoreIncrease?.Invoke(amount);
    }

    public void GoToNextInLevelTrigger()
    {
        GoToNextInLevel?.Invoke();
    }

}
