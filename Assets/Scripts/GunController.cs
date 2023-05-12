using System;
using DG.Tweening;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("GUN PARTS ----")]
    [SerializeField] private GameObject _gunHead;
    [SerializeField] private GameObject _gunCarrier;

    [Header("CAMERA ----")]
    [SerializeField] private Camera _mainCamera;

    private Vector3 _startPos = new Vector3(10.89f, 1.013f, 1.98f);
    private Vector3 _circleRed1 = new Vector3(10.89f, 1.013f, 24.71f);
    private Vector3 _shootPoz2 = new Vector3(14.91f, 1.013f, 28.73f);

    private bool _shooting = false;

    private SkinnedMeshRenderer _gunSkinnedMeshRenderer;

    private float _gunAnimeCounter1 = 0.0f, _gunAnimeCounter2 = 0.0f;
    private int _gunAnimeState = 0; // 0 - GoBack, 1 - Shoot, 2 - Go To Init State
    private int _gunMult = 1000;

    private int _gunLeftRightMult = 7;

    private bool _contrallable = false;
    private bool _canGoLeft = true;
    private bool _canGoRight = true;

    private int _platformDegree = 0;

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
        Vector3 camerafOffset = transform.position - _mainCamera.transform.position;

        DOVirtual.DelayedCall(0.5f, () =>
        {
            transform.DOMove(_startPos, 2);
            _gunCarrier.transform.DORotate(new Vector3(0, -90, 0), 1.0f);
            _gunCarrier.transform.DORotate(new Vector3(0, 0, 0), 1.0f)
            .SetDelay(1.0f).OnComplete(() =>
            {
                _contrallable = true;
            });

            _mainCamera.transform.DOMoveZ((_startPos - camerafOffset).z, 2);
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
                    if (_contrallable) 
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
            double value = _gunLeftRightMult * Time.deltaTime;
            Vector3 moveDirection = new Vector3(
                    transform.position.x - (float)value,
                    transform.position.y,
                    transform.position.z
                    );

            if (_platformDegree == 45)
            {
                value *= Math.Sqrt(2);
                moveDirection = new Vector3(
                    transform.position.x - (float)value,
                    transform.position.y,
                    transform.position.z + (float)value
                    );
            } 
            transform.position = Vector3.Lerp(
                transform.position, moveDirection, 03f);
        }
            
    }

    private void OnGoRightTriggered()
    {
        if (_contrallable && _canGoRight)
        {
            double value = _gunLeftRightMult * Time.deltaTime;
            Vector3 moveDirection = new Vector3(
                    transform.position.x + (float)value,
                    transform.position.y,
                    transform.position.z
                    );

            if (_platformDegree == 45)
            {
                value *= Math.Sqrt(2);
                moveDirection = new Vector3(
                    transform.position.x + (float)value,
                    transform.position.y,
                    transform.position.z - (float)value
                    );
            }

            transform.position = Vector3.Lerp(
                transform.position, moveDirection, 03f);
        }     
    }

    private void ControlGun(bool isLeft)
    {
        int dir = 1;
        if (isLeft) dir = -1;

        if (_contrallable && _canGoLeft)
        {
            double value = _gunLeftRightMult * Time.deltaTime;
            Vector3 moveDirection = new Vector3(
                    transform.position.x - (float)value * dir,
                    transform.position.y,
                    transform.position.z
                    );

            if (_platformDegree == 45)
            {
                value *= Math.Sqrt(2);
                moveDirection = new Vector3(
                    transform.position.x - (float)value * dir,
                    transform.position.y,
                    transform.position.z + (float)value * dir
                    );
            }
            transform.position = Vector3.Lerp(
                transform.position, moveDirection, 03f);
        }
    }

    private void OnTapTriggered()
    {
        if (_contrallable) 
            _shooting = true;
    }

    private void OnGoToNextInLevelTriggered()
    {
        _contrallable = false;
        DOVirtual.DelayedCall(0.5f, () => {
            if (MCGameManager.level1Steps == 1)
            {
                MoveStreightAndLeft();
            } else if (MCGameManager.level1Steps == 2)
            {
                //MoveStart different!
            }
        });
        
        
    }

    private void MoveStreightAndLeft()
    {
        Vector3 camerafOffset = transform.position - _mainCamera.transform.position;

        transform.DOMove(_startPos, 1).OnComplete(() => {
            //Go Streihgt
            _mainCamera.transform.DOMoveZ((_circleRed1 - camerafOffset).z, 5);
            _gunCarrier.transform.DORotate(new Vector3(0, -90, 0), 1.0f);
            transform.DOMove(_circleRed1, 5).OnComplete(() => {
                //Rotate 45 degrees
                DOVirtual.DelayedCall(1.0f, () => {
                    _mainCamera.transform.DOMoveZ((_circleRed1).z, 0.5f);
                    _mainCamera.transform.DORotate(new Vector3(45, 45, 0), 0.5f);
                });
                transform.DORotate(new Vector3(0, 45, 0), 0.5f).OnComplete(() => {
                    //GO To new destinoation
                    transform.DOMove(_shootPoz2, 2);
                    _gunCarrier.transform.DORotate(new Vector3(0, -90 - 45, 0), 1.0f)
                    .SetDelay(1.0f).OnComplete(() => {
                        _platformDegree = 45;
                        _contrallable = true;
                    });
                });
            });
        });
    }
}
