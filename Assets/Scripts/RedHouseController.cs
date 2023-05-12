using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedHouseController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textOnHead;
    [SerializeField] private ParticleSystem _smallRedPS;
    [SerializeField] private ParticleSystem _firePS;

    private int _amountOnRedHouse = 0;

    private bool _dead = false;

    private void Start()
    {
        _amountOnRedHouse = int.Parse(gameObject.name);
        _textOnHead.text = _amountOnRedHouse.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagConst.blueCharacter))
        {
            collision.gameObject.GetComponent<BlueCharacterController>().Die();
            _amountOnRedHouse--;
            _textOnHead.text = _amountOnRedHouse.ToString();
            _smallRedPS.Play();

            if (_amountOnRedHouse <= 0)
            {
                _firePS.Play();
                gameObject.SetActive(false);
                EventManager.current.KillAllCharactersTrigger();
                EventManager.current.BlueScoreIncreaseTrigger(1);
                EventManager.current.YellowScoreIncreaseTrigger(200);
                if (!_dead)
                {
                    _dead = true;
                    EventManager.current.GoToNextInLevelTrigger();
                }
                
            }
            else
            {
                _smallRedPS.Play();
                EventManager.current.BlueScoreIncreaseTrigger(1);
            }
        }
        else if (collision.gameObject.CompareTag(TagConst.yellowCharacter))
        {
            collision.gameObject.GetComponent<YellowCharacterController>().Die();
            if (_amountOnRedHouse >= 2)
                _amountOnRedHouse -= 2;
            else
                _amountOnRedHouse --;
            _textOnHead.text = _amountOnRedHouse.ToString();
            _smallRedPS.Play();

            if (_amountOnRedHouse <= 0)
            {
                _firePS.Play();
                gameObject.SetActive(false);
                EventManager.current.KillAllCharactersTrigger();
                EventManager.current.BlueScoreIncreaseTrigger(2);
                EventManager.current.YellowScoreIncreaseTrigger(200);
                if (!_dead)
                {
                    _dead = true;
                    EventManager.current.GoToNextInLevelTrigger();
                }
            }
            else
            {
                _smallRedPS.Play();
                EventManager.current.BlueScoreIncreaseTrigger(2);
            }
        }
    }
}
