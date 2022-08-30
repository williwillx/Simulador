using UnityEngine;
using System.Collections;

public class FreeCamera : MonoBehaviour
{

    //private float
    //    DistanceCam1,
    //    DistanceCam = 2,
    //    rotX,
    //    rotY;

    //public float
    //    MouseSpeed = 4,
    //    MouseScrollSpeed = 2,
    //    MinDistance = 1,
    //    MaxDistance = 2;

    //public Transform target;
    //SimulationManager simulationManager;
    //Quaternion _savedRot;

    //private void Start()
    //{
    //    DistanceCam = 4;
    //    DistanceCam1 = 4.5f;
    //    rotX = 140;
    //    rotY = 30;

    //    simulationManager = FindObjectOfType<SimulationManager>();
    //}

    //void Update()
    //{
    //    if (target == null) return;
    //    bool isRotating = Input.GetKey(KeyCode.Mouse1);
    //    if (Input.GetKey(KeyCode.Mouse1))
    //    {
    //        rotX += Input.GetAxis("Mouse X") * MouseSpeed;
    //        rotY -= Input.GetAxis("Mouse Y") * MouseSpeed;
    //    }

    //    DistanceCam -= Input.GetAxis("Mouse ScrollWheel") * MouseScrollSpeed;
    //    DistanceCam = Mathf.Clamp(DistanceCam, MinDistance, MaxDistance);
    //    DistanceCam1 = Mathf.Lerp(DistanceCam1, DistanceCam, 10 * Time.deltaTime);

    //    if (!simulationManager)
    //    {
    //        simulationManager = FindObjectOfType<SimulationManager>();
    //    }

    //    transform.rotation = Quaternion.Euler(new Vector3(36, 40, 0));
    //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotY, rotX, 0), Time.deltaTime * 10);

    //    if (isRotating)
    //    {
    //        transform.position = target.position + transform.rotation * new Vector3(0, 0, -1);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotY, rotX, 0), Time.deltaTime * 10);
    //        _savedRot = transform.rotation;
    //    }
    //    else
    //    {
    //        Vector3 currentPos = transform.position;
    //        Vector3 destination = target.position + transform.rotation * new Vector3(0, 0, -1);
    //        Vector3 lerpPos = Vector3.Lerp(currentPos, destination, 10f * Time.deltaTime);
    //        transform.position = lerpPos;
    //        transform.rotation = _savedRot;
    //    }
    //}

    //public void RenableFreeCameraSystem()
    //{
    //    this.enabled = true;
    //}

    //public Vector3 GetDestination()
    //{
    //    return target.position + transform.rotation * new Vector3(0, 0, -DistanceCam1);
    //}
}