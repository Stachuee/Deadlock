using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsiBuffer : ActiveEnemy
{
    [SerializeField] float range;
    [SerializeField] LayerMask toBuff;
    protected override void Start()
    {
        base.Start();
        StartCoroutine("PsiBuff");
    }
    IEnumerator PsiBuff()
    {
        while(true)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, toBuff);

            for(int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform != transform) hits[i].GetComponent<_EnemyBase>().PsiBoost(1);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
