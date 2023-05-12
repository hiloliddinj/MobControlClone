using UnityEngine;
using DG.Tweening;

public class MultiplierController : MonoBehaviour
{

    [SerializeField] Vector3 _leftPosition;
    [SerializeField] Vector3 _rightPosition;
    [SerializeField] bool _isMoving = false;
    [SerializeField] bool _isStartsFromLeft = false;

    private bool _alreadyMooving = false;

    private void FixedUpdate()
    {
        if (_isMoving && !_alreadyMooving)
        {
            _alreadyMooving = true;
            if (_isStartsFromLeft)
            {
                transform.DOMove(_rightPosition, 4.0f).OnComplete(() => {
                    transform.DOMove(_leftPosition, 4.0f).OnComplete(() => {
                        _alreadyMooving = false;
                    });
                });
            }
            else
            {
                transform.DOMove(_leftPosition, 4.0f).OnComplete(() => {
                    transform.DOMove(_rightPosition, 4.0f).OnComplete(() => {
                        _alreadyMooving = false;
                    });
                });
            }

            
        }

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
