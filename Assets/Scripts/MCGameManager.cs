using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MCGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _target1;
    [SerializeField] private GameObject _birthPoint;
    [SerializeField] private List<GameObject> _blueCharacterList;
    [SerializeField] private TextMeshProUGUI _blueScoreTMPro;
    [SerializeField] private TextMeshProUGUI _yellowScoreTMPro;

    private int _blueScore = 0;
    private int _yellowScore = 0;

    public int blueCharacterCount = 0;

    private void Start()
    {
        EventManager.current.GunGenerate += OnGunGenerateTriggered;
        EventManager.current.KillAllCharacters += OnKillAllCharactersTriggered;
        EventManager.current.BlueScoreIncrease += OnBlueScoreIncreased;
        EventManager.current.YellowScoreIncrease += OnYellowScoreIncreased;
        EventManager.current.GoToNextInLevel += OnGoToNextInLevelTriggered;
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
        //Debug.Log("MCGameManager, OnTapTriggered");
        foreach(GameObject blueCharacter in _blueCharacterList)
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
                break;
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
