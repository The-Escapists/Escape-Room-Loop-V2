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

    Transform rig;

    Camera myCamera;
    private void Start()
    {
        myCamera = GetComponent<Camera>();
        rig = transform.parent;
    }

    private void Update()
    {
        if (!MapCreator.instance.IsMouseOverUI())
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
