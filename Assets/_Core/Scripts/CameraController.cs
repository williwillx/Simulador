using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{
    private bool canControlCamera;
    float minDistance = 0.25f;
    float maxDistance = 4;
    Transform lookAtObj;

    [Header("")]
    [SerializeField] float cameraSens = .5f;
    [SerializeField] float cameraDistance;

    [Header("Standard Camera Config")]
    [SerializeField] Transform initialCameraPos;
    [SerializeField] Transform initialCameraLookAt;
    [SerializeField] float initialMinDistance;
    [SerializeField] float initialMaxDistance;

    [Header("Camera movement limits")]
    [SerializeField] LayerMask layerMaskLimitator;
    [SerializeField] float rayDistance;
    [SerializeField] bool clampYRotation;

    private void Start()
    {
        SetLayerMaskConfig();
        SetFloorLayerMask();
    }

    private void SetLayerMaskConfig()
    {
        if(layerMaskLimitator != LayerMask.GetMask("Chao"))
        {
            Debug.LogError("Garanta de alterar a variável layerMaskLimitator do CameraController para a câmera" +
                "funcionar corretamente");
        }

        if(rayDistance == 0)
        {
            rayDistance = 0.2f;
        }
    }

    private void SetFloorLayerMask()
    {
        GameObject floor = GameObject.Find("chao");
        if (floor)
        {
            floor.layer = LayerMask.NameToLayer("Chao");
            BoxCollider collider = floor.AddComponent<BoxCollider>();
            collider.center = Vector3.zero;
        } else
        {
            Debug.LogError("Não foi encontrado o chão da cena. Garanta que o nome do objeto seja \"chao\"");
        }
    }

    private void Update()
    {
        ManualCameraControl();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        clampYRotation = Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, layerMaskLimitator);
    }

    public void InitCamera()
    {
        minDistance = initialMinDistance;
        maxDistance = initialMaxDistance;

        SetCameraControl(false);
        lookAtObj = initialCameraLookAt == null ? transform : initialCameraLookAt;

        Sequence seq = DOTween.Sequence();

        seq.Insert(0, transform.DOMove(initialCameraPos.position, 0f));
        seq.Insert(0, transform.DOLookAt(initialCameraLookAt.position, .0f));

        seq.OnComplete(() =>
        {
            SetCameraControl(true);
            ManualCameraControl();
        });

        Application.targetFrameRate = 60;
    }

    public void MoveCameraTo(Transform _pos, Transform _lookAt, float _minDistance, float _maxDistance,
        float _timeToCameraMove = 1f, float _timeToCameraLookAt = .5f)
    {
        bool posEqualtoLookAt = _pos == _lookAt;
        float distanceFromPosToLook = Vector3.Distance(_pos.position, _lookAt.position);

        if(_pos == _lookAt)
        {
            _minDistance = Mathf.Clamp(_minDistance, 0.03f, float.PositiveInfinity);
        }

        minDistance = _minDistance;
        maxDistance = _maxDistance;

        if (maxDistance < distanceFromPosToLook)
        {
            maxDistance = distanceFromPosToLook;
        }

        SetCameraControl(false);

        lookAtObj = _lookAt == null ? transform : _lookAt;

        Vector3 movementDirection = _lookAt.position - _pos.position;
        cameraDistance = minDistance;
        Vector3 finalPos = _pos.position;

        if (posEqualtoLookAt)
        {
            movementDirection = _pos.position - transform.position;
            finalPos = _pos.position - (movementDirection.normalized * maxDistance);
            cameraDistance = maxDistance;
        }

        Sequence seq = DOTween.Sequence();

        if (posEqualtoLookAt)
        {
            seq.Insert(0, transform.DOMove(finalPos, _timeToCameraMove));
            seq.Insert(0, transform.DOLookAt(_lookAt.position, _timeToCameraLookAt));
        }
        else
        {
            seq.Insert(0, transform.DOMove(finalPos, _timeToCameraMove));
            seq.Insert(1, transform.DOLookAt(_lookAt.position, _timeToCameraLookAt));
        }

        seq.OnComplete(() =>
        {
            SetCameraControl(true);
        });
    }

    public void SetCameraControl(bool _value)
    {
        canControlCamera = _value;
    }

    void ManualCameraControl()
    {
        if (!canControlCamera) return;

        float x = 0;
        float y = 0;

        float mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (clampYRotation)
        {
            mouseScrollValue = Mathf.Clamp(mouseScrollValue, 0, float.PositiveInfinity);
            print("Too close from the floor");
        }

        cameraDistance -= mouseScrollValue;
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);

        if (Input.GetMouseButton(1))
        {
            x = Input.GetAxis("Mouse X") * cameraSens * cameraDistance;
            y = Input.GetAxis("Mouse Y") * cameraSens * cameraDistance;

            // Avoids upper rotation glitch
            if(transform.eulerAngles.x > 85 && transform.eulerAngles.x < 91)
            {
                y = Mathf.Clamp(y, 0, 1);
            }

            // Avoids entering the floor
            if (clampYRotation)
            {
                y = Mathf.Clamp(y, -1, 0);
            }
        }

        Vector3 currentPos = transform.position;
        Vector3 destination = lookAtObj.position + transform.rotation * new Vector3(-x, -y, -cameraDistance);
        Vector3 lerpPos = Vector3.Lerp(currentPos, destination, 10f * Time.deltaTime);
        transform.position = lerpPos;

        transform.LookAt(lookAtObj);
    }
}