using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    Transform target;

    PlayerController playerController;
    Camera cam;

    [Range(0, .3f)][SerializeField] float damping;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 maxCameraTilt;

    private void Awake()
    {
        cam = transform.GetComponent<Camera>();
    }

    private void Start()
    {
        target = playerTransform;
        playerController = playerTransform.GetComponent<PlayerController>();
    }
    void Update()
    {
        transform.position = target.position + offset + new Vector3(playerController.currentAimDirection.x * maxCameraTilt.x, playerController.currentAimDirection.y * maxCameraTilt.y);
    }

    public void SetSplitScreenPosition(int index)
    {
        if(index == 0)
        {
            cam.rect = new Rect(0, 0, 1, 0.5f);
        }
        else
        {
            cam.rect = new Rect(0, 0.5f, 1, 0.5f);
        }
    }

    public void ChangeTarget(Transform target)
    {
        this.target = target;
    }

    public void ResetTarget()
    {
        target = playerTransform;
    }

    public Vector2 GetCenterOfCameraOnScreen()
    {
        return new Vector2(cam.rect.center.x * Screen.width, cam.rect.center.y * Screen.height);
    }

    public Vector2 ViewAngle()
    {
        Vector2 returnValue;
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector2 centerOfScreen = GetCenterOfCameraOnScreen();
        returnValue = (mouse - (Vector2)cam.WorldToScreenPoint(target.position)) / centerOfScreen.x;
        returnValue = returnValue.magnitude > 1 ? returnValue.normalized : returnValue;

        return returnValue;
    }
}
