using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : _EnemyBase
{
    [SerializeField]
    Transform target;

    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * (target.position.x - transform.position.x > 0 ? 1 : -1);
    }
}
