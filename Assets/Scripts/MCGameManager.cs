using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCGameManager : MonoBehaviour
{
    [SerializeField] private GameObject _target1;
    [SerializeField] private GameObject _birthPoint;
    [SerializeField] private List<GameObject> _blueCharacterList;

    private void Start()
    {
        EventManager.current.Tap += OnTapTriggered;
    }

    private void OnDisable()
    {
        EventManager.current.Tap -= OnTapTriggered;
    }

    private void Update()
    {
        
    }

    private void OnTapTriggered()
    {
        //Debug.Log("MCGameManager, OnTapTriggered");
        foreach(GameObject blueCharacter in _blueCharacterList)
        {
            if (!blueCharacter.activeInHierarchy)
            {
                blueCharacter.transform.position = _birthPoint.transform.position;
                blueCharacter.GetComponent<BlueCharacterController>().target = _target1;
                blueCharacter.SetActive(true);
                break;
            }
        }
    }
}
