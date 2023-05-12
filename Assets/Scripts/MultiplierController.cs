using UnityEngine;

public class MultiplierController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConst.blueCharacter))
        {
            BlueCharacterController bCC = other.GetComponent<BlueCharacterController>();

            if (bCC.isGunGenerate && !bCC.collidedToMultiplier)
            {
                bCC.Collided();
                int amount = int.Parse(gameObject.name);
                //Debug.Log("MultiplierController, amount: " + amount);
                MCGameManager mCGameManager = GameObject.FindGameObjectWithTag(TagConst.gameManager).GetComponent<MCGameManager>();
                mCGameManager.OnMultiplierGenerate(gameObject.transform.position, amount, true);
            }
        }
        else if (other.CompareTag(TagConst.yellowCharacter))
        {
            YellowCharacterController yCC = other.GetComponent<YellowCharacterController>();

            if (yCC.isGunGenerate && !yCC.collidedToMultiplier)
            {
                yCC.Collided();
                int amount = int.Parse(gameObject.name);
                //Debug.Log("MultiplierController, amount: " + amount);
                MCGameManager mCGameManager = GameObject.FindGameObjectWithTag(TagConst.gameManager).GetComponent<MCGameManager>();
                mCGameManager.OnMultiplierGenerate(gameObject.transform.position, amount, false);
            }
        }
    }
}
