using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class BlueCharacterController : MonoBehaviour
{
    public Vector3 target;
    public bool isGunGenerate = false;
    public bool collidedToMultiplier = false;

    private NavMeshAgent _navMeshAgent;

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
                _navMeshAgent.SetDestination(target);
            });

        } else
        {
            _navMeshAgent.SetDestination(target);
        }
    }

    public void Collided()
    {
        collidedToMultiplier = true;
        DOVirtual.DelayedCall(1.0f, () =>
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
