using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderFeatureSecondaryCameraOutput : MonoBehaviour
{

    public void SetUp(Material glassesMat)
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 8);
        Debug.Assert(renderTexture.Create(), "Failed to create render asset");

        Camera cam = transform.GetComponent<Camera>();
        cam.targetTexture = renderTexture;
        glassesMat.SetTexture("_SecondaryTex", renderTexture);
    }
}
