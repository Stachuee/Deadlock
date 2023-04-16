using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    List<PlayerController> players = new List<PlayerController>();

    public void AddCameraToEffects(PlayerController player)
    {
        players.Add(player);
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


    [SerializeField] float duration;
    [SerializeField] float fadeIn;
    [SerializeField] float fadeOut;
    [SerializeField] float strength;
    [SerializeField] float bound;
    [SerializeField] float fallout;


    public void ScreenShake(float duration, float fadeIn, float fadeOut, float strength, float bound, float fallout, Vector2 source) // fallout reduces strength in % per meter
    {
        players.ForEach(player =>
        {
            var function = player.cameraController.CameraShake(duration, fadeIn, fadeOut, strength, bound, fallout, source);
            player.cameraController.StartCoroutine(function);
        });
    }
}
