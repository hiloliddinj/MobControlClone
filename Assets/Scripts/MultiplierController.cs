using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConst.blueCharacter))
        {
            BlueCharacterController bCC = other.GetComponent<BlueCharacterController>();

            if (bCC.isGunGenerate && !bCC.collidedToMultiplier)
            {
                bCC.Collided();
                int amount = int.Parse(gameObject.name);
                Debug.Log("MultiplierController, amount: " + amount);
                MCGameManager mCGameManager = GameObject.FindGameObjectWithTag(TagConst.gameManager).GetComponent<MCGameManager>();
                mCGameManager.OnMultiplierGenerate(gameObject.transform.position, amount);
            }
        }
    }
}
