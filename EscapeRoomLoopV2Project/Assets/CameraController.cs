using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float minOrthographicSize = 5;
    public float maxOrthographicSize = 25;
    public float mouseZoomSensitivity = 1f;
    public float mouseMoveSensitivity = 1f;
    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;
    public Vector3 cameraAngle;

    Transform rig;

    Camera myCamera;
    private void Start()
    {
        myCamera = GetComponent<Camera>();
        rig = transform.parent;
    }

    private void Update()
    {
        myCamera.orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * mouseZoomSensitivity;
        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, minOrthographicSize, maxOrthographicSize);

        if (Input.GetMouseButton(2))
        {
            float mouseX = -Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            rig.transform.position += new Vector3(mouseX, mouseY, 0) * mouseMoveSensitivity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rig.transform.position += rig.transform.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rig.transform.position += -rig.transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rig.transform.position += -rig.transform.up * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rig.transform.position += rig.transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 rot = rig.transform.rotation.eulerAngles;
            rot.z += rotationSpeed * Time.deltaTime;
            rig.transform.rotation = Quaternion.Euler(rot);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 rot = rig.transform.rotation.eulerAngles;
            rot.z -= rotationSpeed * Time.deltaTime;
            rig.transform.rotation = Quaternion.Euler(rot);
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            Vector3 rot = myCamera.transform.rotation.eulerAngles;

            if (rot.x == 0 && rot.y == 0 && rot.z == 0)
                rot = cameraAngle;
            else
                rot = Vector3.zero;

            myCamera.transform.rotation = Quaternion.Euler(rot);
        }
    }

    public void SetMaxOrthographicSize(float Size)
    {
        maxOrthographicSize = Size;

        if (myCamera == null)
            myCamera = Camera.main;
        myCamera.orthographicSize = maxOrthographicSize;
    }
}
