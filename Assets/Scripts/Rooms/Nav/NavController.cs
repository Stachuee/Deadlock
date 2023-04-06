using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavController : MonoBehaviour
{
    public static NavController instance;


    public List<NavNode> nodes = new List<NavNode>();
    
    [SerializeField]
    GameObject navNode;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void CreateNavMesh()
    {

    }



    [ContextMenu("Create navmesh from premade map")]
    void createNavMeshPremade()
    {
        nodes.Clear();
        NavNode[] allNodes = FindObjectsOfType<NavNode>();
        for (int i = 0; i < allNodes.Length; i++)
        {
            DestroyImmediate(allNodes[i].gameObject);
        }

        List<Rooms> allRooms = FindObjectsOfType<Rooms>().ToList();
        List<DoorMarker> doorMarkers = FindObjectsOfType<DoorMarker>().ToList();

        //Add node to each room
        allRooms.ForEach(r =>
        {
            nodes.Add(Instantiate(navNode, r.Position, Quaternion.identity, r.transform).GetComponent<NavNode>());
        });

        // match rooms with overlaping door markers
        nodes.ForEach(node =>
        {
            List<DoorMarker> markers = node.GetComponentInParent<Rooms>().GetComponentsInChildren<DoorMarker>().ToList();
            markers.ForEach(marker =>
            {
                doorMarkers.ForEach(doorMarker =>
                {
                    if(doorMarker != marker && Vector2.Distance(doorMarker.transform.position, marker.transform.position) < 0.2f)
                    {
                        node.AddConnectedNode(doorMarker.GetComponentInParent<Rooms>().GetComponentInChildren<NavNode>());
                    }
                });
            });
        });

    }
}
