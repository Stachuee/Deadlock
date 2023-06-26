using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowScript : MonoBehaviour, IControllSubscriberMove, IControllSubscriberAim
{
    Vector3 cameraShake;
    Transform camHolder;

    [SerializeField]
    Transform playerTransform;

    Transform target;

    PlayerController playerController;
    public Camera cam;

    [Range(0, .3f)][SerializeField] float damping;
    [SerializeField] Vector3 playerOffset;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] Vector2 maxCameraTilt;

    [SerializeField] Material glassesMat;

    [SerializeField] float camDumping;
    Vector3 dampVel = Vector3.zero;


    bool onPlayer;

    public bool useMove;

    Vector2 move;
    Vector2 aim;

    private void Awake()
    {
        cam = transform.GetComponent<Camera>();
        camHolder = transform.parent;
        onPlayer = true;
    }

    private void Start()
    {
        target = playerTransform;
        playerController = playerTransform.GetComponent<PlayerController>();
        playerController.AddAimSubscriber(this);
        playerController.AddMoveSubscriber(this);
    }

    void LateUpdate()
    {
        //camHolder.position = target.position + offset + new Vector3(playerController.currentAimDirection.x * maxCameraTilt.x, playerController.currentAimDirection.y * maxCameraTilt.y);
        Vector3 desiredPos;

        if(useMove)
        {
            desiredPos = target.position + (onPlayer ? playerOffset : cameraOffset) + new Vector3(move.x * maxCameraTilt.x, move.y * maxCameraTilt.y);
        }
        else
        {
            desiredPos = target.position + (onPlayer ? playerOffset : cameraOffset)  + new Vector3(aim.x * maxCameraTilt.x, aim.y * maxCameraTilt.y);
        }

        if(Vector2.Distance(transform.position, (Vector2)desiredPos) < 0.2f) camHolder.position = desiredPos;
        else camHolder.position = Vector3.SmoothDamp(camHolder.position, desiredPos, ref dampVel, camDumping);
        
    }

    public void SetSplitScreenPosition(int index, bool glassesMode)
    {
        if(index == 0)
        {
            if(glassesMode)
            {

            }
            else
            {
                cam.rect = new Rect(0, 0, 1, 0.5f);
                cam.orthographicSize = cam.orthographicSize / 2;
            }
            
        }
        else
        {
            if (glassesMode)
            {
                cam.transform.tag = "SecondaryCamera";
                cam.gameObject.AddComponent<RenderFeatureSecondaryCameraOutput>().SetUp(glassesMat);
            }
            else
            {
                cam.rect = new Rect(0, 0.5f, 1, 0.5f);
                cam.orthographicSize = cam.orthographicSize / 2;
            }
        }
    }

    public void ChangeTarget(Transform target)
    {
        this.target = target;
        onPlayer = false;
    }

    public void ResetTarget()
    {
        target = playerTransform;
    }

    public Vector2 GetCenterOfCameraOnScreen()
    {
        return new Vector2(cam.rect.center.x * Screen.width, cam.rect.center.y * Screen.height);
    }

    public Vector2 MouseWorldPoint()
    {
        return cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public Vector2 ViewAngle()
    {
        Vector2 returnValue;
        if (target == null) return Vector2.zero;
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

    public void ForwardCommandMove(Vector2 controll, Vector2 controllSmooth)
    {
        move = controllSmooth;
    }

    public void ForwardCommandAim(Vector2 controll, Vector2 controllSmooth)
    {
        aim = controllSmooth;
    }
}
