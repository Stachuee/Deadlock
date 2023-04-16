using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowScript : MonoBehaviour
{
    Vector3 cameraShake;
    Transform camHolder;

    [SerializeField]
    Transform playerTransform;

    Transform target;

    PlayerController playerController;
    public Camera cam;

    [Range(0, .3f)][SerializeField] float damping;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 maxCameraTilt;

    private void Awake()
    {
        cam = transform.GetComponent<Camera>();
        camHolder = transform.parent;
    }

    private void Start()
    {
        target = playerTransform;
        playerController = playerTransform.GetComponent<PlayerController>();
    }
    void Update()
    {
        camHolder.position = target.position + offset + new Vector3(playerController.currentAimDirection.x * maxCameraTilt.x, playerController.currentAimDirection.y * maxCameraTilt.y);
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

    public IEnumerator CameraShake(float duration, float fadeIn, float fadeOut, float strength, float bound, float fallout, Vector2 source)
    {
        float timeElapsed = 0;
        cameraShake = Vector3.zero;
        while (duration > timeElapsed)
        {
            float strengthMultipilier = Mathf.Clamp01(1 - (Vector2.Distance(source, target.position) * fallout));
            if (Vector2.Distance(transform.localPosition, cameraShake) < 0.1f) cameraShake = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * bound * strengthMultipilier;
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, cameraShake, Mathf.Lerp(0, 1, timeElapsed / fadeIn) * Mathf.Lerp(1, 0, (timeElapsed - duration + fadeOut) / fadeOut) * strength * strengthMultipilier * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
