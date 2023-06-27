using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static bool markAsTutorial;
    public static TutorialController instance;

    [SerializeField] bool isTutorial;
    [SerializeField] Transform soliderAnim;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        markAsTutorial = isTutorial;
        if (isTutorial)
        {
            StartCoroutine("DelayTutorial");
        }
    }

    IEnumerator DelayTutorial()
    {
        yield return new WaitForSeconds(0);
        StartTutorial();
    }

    public void StartTutorial()
    {
        PacingController.pacingController.StartTutorial();
        PlayerController.solider.LockInCutscene(true);
        DialogueManager.instance.StartTutorial();
        PlayerController.solider.cameraController.ChangeTarget(soliderAnim, true);
    }
}
