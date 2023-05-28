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


    public enum ScreenShakeStrength {Weak, Medium, Strong }
    public enum ScreenShakeRange {Small, Medium, Large, Global }

    public void ScreenShake(float duration, ScreenShakeRange range, ScreenShakeStrength strength, Vector2 source)
    {
        float fadeIn, fadeOut, shakeStrength = 0, bound = 0, fallout = 0;
        fadeIn = duration * 0.2f;
        fadeOut = duration * 0.2f;

        switch (strength)
        {
            case ScreenShakeStrength.Weak:
                shakeStrength = 10f;
                bound = 0.1f;
                break;
            case ScreenShakeStrength.Medium:
                shakeStrength = 20f;
                bound = 0.2f;
                break;
            case ScreenShakeStrength.Strong:
                shakeStrength = 30f;
                bound = 0.3f;
                break;
        }

        switch (range)
        {
            case ScreenShakeRange.Small:
                fallout = 0.1f;
                break;
            case ScreenShakeRange.Medium:
                fallout = 0.05f;
                break;
            case ScreenShakeRange.Large:
                fallout = 0.025f;
                break;
            case ScreenShakeRange.Global:
                fallout = 0f;
                break;
        }

        ScreenShake(duration, fadeIn, fadeOut, shakeStrength, bound, fallout, source);

    }

    public void ScreenShake(float duration, float fadeIn, float fadeOut, float strength, float bound, float fallout, Vector2 source) // fallout reduces strength in % per meter
    {
        players.ForEach(player =>
        {
            var function = player.cameraController.CameraShake(duration, fadeIn, fadeOut, strength, bound, fallout, source);
            player.cameraController.StartCoroutine(function);
        });
    }
}
