using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    [SerializeField]
    public NavNodeType navNodeType;

    [SerializeField]
    List<NavNode> connectedNodes = new List<NavNode>();

    public NavNode previous;
    public float distance;

    public enum NavNodeType { Horizontal, Stairs}

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
    public void AddConnectedNode(List<NavNode> node)
    {
        connectedNodes.AddRange(node);
        connectedNodes = connectedNodes.Distinct().ToList();
    }

    public List<NavNode> GetConnectedNodes()
    {
        return connectedNodes;
    }
}
