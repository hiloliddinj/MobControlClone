using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BlueCharacterController : MonoBehaviour
{
    public Vector3 target;
    public bool isGunGenerate = false;
    public bool collidedToMultiplier = false;

    private NavMeshAgent _navMeshAgent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConst.redCharacter))
        {
            Die();
            other.gameObject.SetActive(false);
            EventManager.current.BlueScoreIncreaseTrigger(1);
        }
    }

    public void MoveAfterGunGenerate()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (isGunGenerate)
        {
            Vector3 jumpVector = new Vector3(
                transform.position.x,
                transform.position.y - 0.6f,
                transform.position.z + 2);

            if (MCGameManager.level1Steps > 0)
            {
                jumpVector = new Vector3(
                transform.position.x + 1.414f,
                transform.position.y - 0.6f,
                transform.position.z + 1.414f);
            }

            transform.DOMove(jumpVector, 0.5f);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (gameObject.activeInHierarchy)
                    _navMeshAgent.SetDestination(target);
            });

        } else
        {
            _navMeshAgent.SetDestination(target);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                isGunGenerate = true;
            });
        }
    }

    public void Collided()
    {
        collidedToMultiplier = true;
        DOVirtual.DelayedCall(0.5f, () =>
        {
            collidedToMultiplier = false;
        });
    }

    public void Die()
    {
        isGunGenerate = false;
        collidedToMultiplier = false;
        gameObject.SetActive(false);
    }
}
