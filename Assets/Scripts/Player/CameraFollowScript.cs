using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    Transform target;

    PlayerController playerController;

    [Range(0, .3f)][SerializeField] float damping;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 maxCameraTilt;


    private void Start()
    {
        target = playerTransform;
        playerController = playerTransform.GetComponent<PlayerController>();
    }
    void Update()
    {
        transform.position = target.position + offset + new Vector3(playerController.currentAimDirection.x * maxCameraTilt.x, playerController.currentAimDirection.y * maxCameraTilt.y);
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
