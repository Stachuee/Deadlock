using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static bool markAsTutorial;
    [SerializeField] bool isTutorial;

    private void Start()
    {
        markAsTutorial = isTutorial;
    }
}
