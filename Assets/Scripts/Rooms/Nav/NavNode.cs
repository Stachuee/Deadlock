using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavNode : MonoBehaviour
{
    [SerializeField]
    public NavNodeType navNodeType;
    [SerializeField]
    public NavNode connectedStairs;

    [SerializeField]
    List<NavNode> connectedNodes = new List<NavNode>();
    
    [HideInInspector]
    public NavNode previous;
    [HideInInspector]
    public float distance;

    public NavNode nextNode;
    public float distanceToScientist;
    public float obstaclesWeigths;

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
        connectedNodes = connectedNodes.Distinct().ToList();
    }
    public void AddConnectedNode(List<NavNode> node)
    {
        connectedNodes.AddRange(node);
        connectedNodes = connectedNodes.Distinct().ToList();
    }
    public void RemoveConnectedNode(NavNode node)
    {
        connectedNodes.Remove(node);
    }

    public List<NavNode> GetConnectedNodes()
    {
        return connectedNodes;
    }

}
