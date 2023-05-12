using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MCGameManager : MonoBehaviour
{
    [Header("-> TARGETS ")]
    [SerializeField] private Vector3 _target1;
    [SerializeField] private Vector3 _target2;
    [SerializeField] private GameObject _birthPoint;

    [Header("-> CHARACTERS")]
    [SerializeField] private List<GameObject> _blueCharacterList;
    [SerializeField] private List<GameObject> _yellowCharacterList;
    [SerializeField] private List<GameObject> _redCharacterList;

    [Header("-> UI")]
    [SerializeField] private TextMeshProUGUI _blueScoreTMPro;
    [SerializeField] private TextMeshProUGUI _yellowScoreTMPro;

    [Header("-> GUN")]
    [SerializeField] private Slider _gunBlueIndicatorSlider;

    private int _blueScore = 0;
    private int _yellowScore = 0;

    private int _gunGeneratedBlueCharacterCount = 0;

    private Vector3 _targetOfNavmeshes;

    private int blueCharacterCount = 0;
    private int yellowCharacterCount = 0;

    public static int level1Steps = 0;

    public static bool _trigger1Triggered = false;

    private void Start()
    {
        EventManager.current.GunGenerate += OnGunGenerateTriggered;
        EventManager.current.KillAllCharacters += OnKillAllCharactersTriggered;
        EventManager.current.BlueScoreIncrease += OnBlueScoreIncreased;
        EventManager.current.YellowScoreIncrease += OnYellowScoreIncreased;
        EventManager.current.GoToNextInLevel += OnGoToNextInLevelTriggered;

        _targetOfNavmeshes = _target1;

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
                    bCController.target = _targetOfNavmeshes;
                    bCController.isGunGenerate = true;
                    if (level1Steps == 0)
                        blueCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else
                    {
                        blueCharacter.transform.rotation = Quaternion.Euler(0, 45, 0);
                        if (level1Steps == 1 && !_trigger1Triggered)
                        {
                            _trigger1Triggered = true;
                            GenerateReds();
                            DOVirtual.DelayedCall(6.0f, () => {
                                GenerateReds();
                            });
                        }    
                    }

                        
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
                    yCController.target = _targetOfNavmeshes;
                    yCController.isGunGenerate = true;
                    if (level1Steps == 0)
                        yellowCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else
                        yellowCharacter.transform.rotation = Quaternion.Euler(0, 45, 0);
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

    public void OnMultiplierGenerate(Vector3 birthPoint, int amount, bool isBlueCharacter)
    {
        for(int i = 0; i < amount - 1; i++)
        {
            if (isBlueCharacter)
            {
                foreach (GameObject blueCharacter in _blueCharacterList)
                {
                    if (!blueCharacter.activeInHierarchy)
                    {
                        blueCharacter.transform.position = birthPoint;

                        BlueCharacterController bCController = blueCharacter.GetComponent<BlueCharacterController>();
                        bCController.target = _targetOfNavmeshes;
                        if (level1Steps == 0)
                            blueCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else
                            blueCharacter.transform.rotation = Quaternion.Euler(0, 45, 0);
                        blueCharacter.SetActive(true);
                        bCController.MoveAfterGunGenerate();
                        blueCharacterCount++;
                        break;
                    }
                }
            }
            else
            {
                foreach (GameObject yellowCharacter in _yellowCharacterList)
                {
                    if (!yellowCharacter.activeInHierarchy)
                    {
                        yellowCharacter.transform.position = birthPoint;

                        YellowCharacterController yCController = yellowCharacter.GetComponent<YellowCharacterController>();
                        yCController.target = _targetOfNavmeshes;
                        if (level1Steps == 0)
                            yellowCharacter.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else
                            yellowCharacter.transform.rotation = Quaternion.Euler(0, 45, 0);
                        yellowCharacter.SetActive(true);
                        yCController.MoveAfterGunGenerate();
                        blueCharacterCount++;
                        break;
                    }
                }
            }
            
        }
    }

    private void OnKillAllCharactersTriggered()
    {
        //Kill All Blues
        foreach (GameObject blueCharacter in _blueCharacterList)
        {
            if (blueCharacter.activeInHierarchy)
            {
                BlueCharacterController bCController = blueCharacter.GetComponent<BlueCharacterController>();
                bCController.Die();
            }
        }

        //Kill All Yellows
        foreach (GameObject yellowCharacter in _yellowCharacterList)
        {
            if (yellowCharacter.activeInHierarchy)
            {
                YellowCharacterController yCController = yellowCharacter.GetComponent<YellowCharacterController>();
                yCController.Die();
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
        blueCharacterCount = 0;
        yellowCharacterCount = 0;
        _targetOfNavmeshes = _target2;
        level1Steps++;
    }

    private void GenerateReds()
    {
        DOVirtual.DelayedCall(3.0f, () =>
        {
            int count = 16;
            while (count > 0)
            {
                DOVirtual.DelayedCall(0.3f, () => {
                    int counter = 4;
                    foreach (GameObject redChar in _redCharacterList)
                    {
                        if (!redChar.activeInHierarchy)
                        {
                            redChar.transform.position = _target2;
                            if (level1Steps == 0)
                                redChar.transform.rotation = Quaternion.Euler(0, 180, 0);
                            else
                                redChar.transform.rotation = Quaternion.Euler(0, 180 + 45, 0);
                            redChar.SetActive(true);
                            counter--;
                            if (counter == 0) break;
                        }
                    }
                });
                count -= 4;
            } 
        });
    }
}
