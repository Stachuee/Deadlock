using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNade : NadeBase
{
    protected override void Explode(bool onContact = false, Vector2? normals = null)
    {
        base.Explode(onContact);
        Destroy(Instantiate(explosionVFX, (Vector2)transform.position + (Vector2)normals * 0.5f, Quaternion.identity), 1);
    }
}
