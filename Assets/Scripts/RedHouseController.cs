using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RedHouseController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textOnHead;
    [SerializeField] private ParticleSystem _smallRedPS;
    [SerializeField] private ParticleSystem _firePS;

    private int amountOnRedHouse = 0;

    private void Start()
    {
        amountOnRedHouse = int.Parse(gameObject.name);
        _textOnHead.text = amountOnRedHouse.ToString();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(TagConst.blueCharacter))
    //    {
    //        other.gameObject.GetComponent<BlueCharacterController>().Die();
    //        amountOnRedHouse--;
    //        _textOnHead.text = amountOnRedHouse.ToString();
    //        _smallRedPS.Play();
    //        _rigidBody.AddForce(0, 1.0f, 0, ForceMode.Impulse);

    //        if (amountOnRedHouse <= 0)
    //        {
    //            _firePS.Play();
    //            gameObject.SetActive(false);
    //            EventManager.current.KillAllCharactersTrigger();
    //        } else
    //        {
    //            _smallRedPS.Play();
    //        }
    //    }

    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagConst.blueCharacter))
        {
            collision.gameObject.GetComponent<BlueCharacterController>().Die();
            amountOnRedHouse--;
            _textOnHead.text = amountOnRedHouse.ToString();
            _smallRedPS.Play();

            if (amountOnRedHouse <= 0)
            {
                _firePS.Play();
                gameObject.SetActive(false);
                EventManager.current.KillAllCharactersTrigger();
                EventManager.current.BlueScoreIncreaseTrigger(1);
                EventManager.current.YellowScoreIncreaseTrigger(200);
                EventManager.current.GoToNextInLevelTrigger();
            }
            else
            {
                _smallRedPS.Play();
                EventManager.current.BlueScoreIncreaseTrigger(1);
            }
        }
    }
}
