using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MCGameManager : MonoBehaviour
{
    [Header("-> TARGETS ")]
    [SerializeField] private GameObject _target1;
    [SerializeField] private GameObject _birthPoint;

    [Header("-> CHARACTERS")]
    [SerializeField] private List<GameObject> _blueCharacterList;
    [SerializeField] private List<GameObject> _yellowCharacterList;

    [Header("-> UI")]
    [SerializeField] private TextMeshProUGUI _blueScoreTMPro;
    [SerializeField] private TextMeshProUGUI _yellowScoreTMPro;

    [Header("-> GUN")]
    [SerializeField] private Slider _gunBlueIndicatorSlider;

    private int _blueScore = 0;
    private int _yellowScore = 0;

    public int blueCharacterCount = 0;
    public int yellowCharacterCount = 0;

    private int _gunGeneratedBlueCharacterCount = 0;

    private void Start()
    {
        EventManager.current.GunGenerate += OnGunGenerateTriggered;
        EventManager.current.KillAllCharacters += OnKillAllCharactersTriggered;
        EventManager.current.BlueScoreIncrease += OnBlueScoreIncreased;
        EventManager.current.YellowScoreIncrease += OnYellowScoreIncreased;
        EventManager.current.GoToNextInLevel += OnGoToNextInLevelTriggered;

        _gunBlueIndicatorSlider.value = 0;
        _gunBlueIndicatorSlider.maxValue = 25;
    }

    private void OnDisable()
    {
        EventManager.current.GunGenerate -= OnGunGenerateTriggered;
        EventManager.current.KillAllCharacters -= OnKillAllCharactersTriggered;
        EventManager.current.BlueScoreIncrease += OnBlueScoreIncreased;
        EventManager.current.YellowScoreIncrease += OnYellowScoreIncreased;
        EventManager.current.GoToNextInLevel -= OnGoToNextInLevelTriggered;
    }

    private void OnGunGenerateTriggered()
    {
        if (_gunGeneratedBlueCharacterCount < 25)
        {
            foreach (GameObject blueCharacter in _blueCharacterList)
            {
                if (!blueCharacter.activeInHierarchy)
                {
                    blueCharacter.transform.position = _birthPoint.transform.position;

                    BlueCharacterController bCController = blueCharacter.GetComponent<BlueCharacterController>();
                    bCController.target = _target1;
                    bCController.isGunGenerate = true;
                    blueCharacter.SetActive(true);
                    bCController.MoveAfterGunGenerate();
                    blueCharacterCount++;
                    _gunGeneratedBlueCharacterCount++;
                    _gunBlueIndicatorSlider.value = _gunGeneratedBlueCharacterCount;
                    break;
                }
            }
        } else
        {
            foreach (GameObject yellowCharacter in _yellowCharacterList)
            {
                if (!yellowCharacter.activeInHierarchy)
                {
                    yellowCharacter.transform.position = _birthPoint.transform.position;

                    YellowCharacterController yCController = yellowCharacter.GetComponent<YellowCharacterController>();
                    yCController.target = _target1;
                    yCController.isGunGenerate = true;
                    yellowCharacter.SetActive(true);
                    yCController.MoveAfterGunGenerate();
                    yellowCharacterCount++;
                    _gunGeneratedBlueCharacterCount = 0;
                    _gunBlueIndicatorSlider.value = _gunGeneratedBlueCharacterCount;
                    break;
                }
            }

        }
        
    }

    public void OnMultiplierGenerate(Vector3 birthPoint, int amount)
    {
        for(int i = 0; i < amount - 1; i++)
        {
            foreach (GameObject blueCharacter in _blueCharacterList)
            {
                if (!blueCharacter.activeInHierarchy)
                {
                    blueCharacter.transform.position = birthPoint;

                    BlueCharacterController bCController = blueCharacter.GetComponent<BlueCharacterController>();
                    bCController.target = _target1;
                    blueCharacter.SetActive(true);
                    bCController.MoveAfterGunGenerate();
                    blueCharacterCount++;
                    break;
                }
            }
        }
    }

    private void OnKillAllCharactersTriggered()
    {
        foreach (GameObject blueCharacter in _blueCharacterList)
        {
            if (blueCharacter.activeInHierarchy)
            {
                BlueCharacterController bCController = blueCharacter.GetComponent<BlueCharacterController>();
                bCController.Die();
            }
        }
    }

    private void OnBlueScoreIncreased(int amount)
    {
        _blueScore += amount;
        _blueScoreTMPro.text = _blueScore.ToString();
    }

    private void OnYellowScoreIncreased(int amount)
    {
        _yellowScore += amount;
        _yellowScoreTMPro.text = _yellowScore.ToString();
    }

    private void OnGoToNextInLevelTriggered()
    {
        //Do Something;
    }

}
