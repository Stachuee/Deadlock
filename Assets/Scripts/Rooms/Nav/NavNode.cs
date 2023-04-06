using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    [SerializeField]
    NavNodeType navNodeType;

    [SerializeField]
    List<NavNode> connectedNodes = new List<NavNode>();

    enum NavNodeType { Horizontal, Stairs}

    private void OnDrawGizmos()
    { 
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .2f);
        
        Gizmos.color = Color.red;
        connectedNodes.ForEach(node =>
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        });
    }

    public void AddConnectedNode(NavNode node)
    {
        connectedNodes.Add(node);
    }
}
