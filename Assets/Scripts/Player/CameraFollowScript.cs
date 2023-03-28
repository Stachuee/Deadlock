using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
