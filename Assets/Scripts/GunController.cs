using DG.Tweening;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("GUN PARTS ----")]
    [SerializeField] private GameObject _gunHead;
    [SerializeField] private GameObject _gunCarrier;
    [SerializeField] private Vector3 _startPos;

    [Header("CAMERA ----")]
    [SerializeField] private Camera _mainCamera;

    private bool _shooting = false;

    private SkinnedMeshRenderer _gunSkinnedMeshRenderer;

    private float _gunAnimeCounter1 = 0.0f, _gunAnimeCounter2 = 0.0f;
    private int _gunAnimeState = 0; // 0 - GoBack, 1 - Shoot, 2 - Go To Init State
    private int _gunMult = 1000;

    private int _gunLeftRightMult = 7;

    private bool _contrallable = false;
    private bool _canGoLeft = true;
    private bool _canGoRight = true;


    private void Start()
    {
        EventManager.current.GoLeft += OnGoLeftTriggered;
        EventManager.current.GoRight += OnGoRightTriggered;
        EventManager.current.Tap += OnTapTriggered;
        EventManager.current.GoToNextInLevel += OnGoToNextInLevelTriggered;

        _gunSkinnedMeshRenderer = _gunHead.GetComponent<SkinnedMeshRenderer>();
        MoveStart();
    }

    private void OnDisable()
    {
        EventManager.current.GoLeft -= OnGoLeftTriggered;
        EventManager.current.GoRight -= OnGoRightTriggered;
        EventManager.current.Tap -= OnTapTriggered;
        EventManager.current.GoToNextInLevel -= OnGoToNextInLevelTriggered;
    }

    private void Update()
    {
        GunShootAnimate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConst.wallLeft))
        {
            _canGoLeft = false;
        }
        else if (other.CompareTag(TagConst.wallRight))
        {
            _canGoRight = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagConst.wallLeft))
        {
            _canGoLeft = true;
        }
        else if (other.CompareTag(TagConst.wallRight))
        {
            _canGoRight = true;
        }
    }


    private void MoveStart()
    {
        Vector3 cameraOffset = transform.position - _mainCamera.transform.position;

        DOVirtual.DelayedCall(0.5f, () => {
            transform.DOMove(_startPos, 2);
            _gunCarrier.transform.DORotate(new Vector3(0, -90, 0), 1.0f);
            _gunCarrier.transform.DORotate(new Vector3(0, 0, 0), 1.0f)
            .SetDelay(1.0f).OnComplete(() => {
                _contrallable = true;
            });

            _mainCamera.transform.DOMoveZ((_startPos - cameraOffset).z, 2);
        });
    }

    private void GunShootAnimate()
    {
        if (_shooting)
        {
            if (_gunAnimeState == 0 && _gunAnimeCounter1 < 100)
            {
                _gunAnimeCounter1 += _gunMult * Time.deltaTime;
                if (_gunAnimeCounter1 > 100) _gunAnimeCounter1 = 100;

                _gunSkinnedMeshRenderer.SetBlendShapeWeight(0, _gunAnimeCounter1);
                _gunSkinnedMeshRenderer.SetBlendShapeWeight(3, _gunAnimeCounter1);

                if (_gunAnimeCounter1 >= 100)
                    _gunAnimeState = 1;

            }
            else if (_gunAnimeState == 1 && _gunAnimeCounter2 < 100 && _gunAnimeCounter1 > 0)
            {
                _gunAnimeCounter1 -= _gunMult * Time.deltaTime;
                if (_gunAnimeCounter1 < 0) _gunAnimeCounter1 = 0;

                _gunAnimeCounter2 += _gunMult * Time.deltaTime;
                if (_gunAnimeCounter2 > 100) _gunAnimeCounter2 = 100;

                _gunSkinnedMeshRenderer.SetBlendShapeWeight(0, _gunAnimeCounter1);
                _gunSkinnedMeshRenderer.SetBlendShapeWeight(1, _gunAnimeCounter2);
                _gunSkinnedMeshRenderer.SetBlendShapeWeight(2, _gunAnimeCounter2);

                if (_gunAnimeCounter2 >= 100)
                {
                    _gunAnimeState = 2;
                    /// Generate character!
                    EventManager.current.GunGenerateTrigger();
                }
            }
            else if (_gunAnimeState == 2 && _gunAnimeCounter2 > 0)
            {
                _gunAnimeCounter2 -= _gunMult * Time.deltaTime;
                if (_gunAnimeCounter2 < 0) _gunAnimeCounter2 = 0;

                _gunSkinnedMeshRenderer.SetBlendShapeWeight(1, _gunAnimeCounter2);
                _gunSkinnedMeshRenderer.SetBlendShapeWeight(2, _gunAnimeCounter2);
                _gunSkinnedMeshRenderer.SetBlendShapeWeight(3, _gunAnimeCounter2);

                if (_gunAnimeCounter2 <= 0)
                {
                    _gunAnimeState = 0;
                    /// Finished shooting
                    _shooting = false;
                }
            }
        } 
    }

    private void OnGoLeftTriggered()
    {
        if (_contrallable && _canGoLeft)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(
                    transform.position.x - _gunLeftRightMult * Time.deltaTime,
                    transform.position.y,
                    transform.position.z
                    ), 03f
                );
        }
            
    }

    private void OnGoRightTriggered()
    {
        if (_contrallable && _canGoRight)
        {
            transform.position = Vector3.Lerp(
                 transform.position,
                 new Vector3(
                     transform.position.x + _gunLeftRightMult * Time.deltaTime,
                     transform.position.y,
                     transform.position.z
                     ), 03f
                 );
        }
            
    }

    private void OnTapTriggered()
    {
        //Debug.Log("GunController, OnTapTriggered");
        if (_contrallable) 
            _shooting = true;
    }

    private void OnGoToNextInLevelTriggered()
    {
        _contrallable = false;
    }
}
