using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavController : MonoBehaviour
{
    public static NavController instance;


    public List<NavNode> nodes = new List<NavNode>();
    
    [SerializeField]
    Rooms targetRoom;

    [SerializeField]
    GameObject navNode;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Queue<NavNode> GetPahtToScientist(Vector2 from)
    {
        return GetPathTo(from, targetRoom.transform.position);
    }

    public Queue<NavNode> GetPathTo(Vector2 from, Vector2 to)
    {
        Queue<NavNode> path = new Queue<NavNode> ();

        NavNode currentNode = FindClosestWaypoint(from);
        NavNode toNode = FindClosestWaypoint(to);


        if(currentNode == null || toNode == null)
        {
            path.Enqueue (currentNode);
            return path;
        }

        SortedList<float, NavNode> openList = new SortedList<float, NavNode>();
        List<NavNode> closedList = new List<NavNode>();
        openList.Add(0, currentNode);

        currentNode.previous = null;
        currentNode.distance = 0f;

        while(openList.Count > 0)
        {
            currentNode = openList.Values[0];
            openList.RemoveAt(0);

            float distance = currentNode.distance;
            closedList.Add(currentNode);

            if (toNode == currentNode)
            {
                while (currentNode.previous != null)
                {
                    path.Enqueue(currentNode);
                    currentNode = currentNode.previous;
                }
                path.Enqueue(currentNode);
            }
            else
            {
                currentNode.GetConnectedNodes().ForEach(node =>
                {
                    if (!(closedList.Contains(node) || openList.ContainsValue(node)))
                    {
                        node.previous = currentNode;
                        node.distance = currentNode.distance + Vector2.Distance(node.transform.position, currentNode.transform.position);
                        float distanceToTarget = node.distance + Vector2.Distance(node.transform.position, toNode.transform.position);
                        openList.Add(distanceToTarget, node);
                    }
                });
            }
        }
        path = new Queue<NavNode>(path.Reverse());
        return path;
    }

    private NavNode FindClosestWaypoint(Vector2 target)
    {
        NavNode closest = null;
        float closestDist = Mathf.Infinity;
        foreach (NavNode waypoint in nodes)
        {
            var dist = ((Vector2)waypoint.transform.position - target).magnitude;
            if (dist < closestDist)
            {
                closest = waypoint;
                closestDist = dist;
            }
        }
        if (closest != null)
        {
            return closest;
        }
        return null;
    }



    public void CreateNavMesh()
    {

    }

    struct DoorNav
    {
        public StairsScript script;
        public NavNode node;
    };

    [ContextMenu("Create navmesh from premade map")]
    void createNavMeshPremade()
    {
        List<Rooms> allRooms = FindObjectsOfType<Rooms>().ToList();
        targetRoom = allRooms.Find(x => x.startingRoom == true);
        if(targetRoom == null)
        {
            Debug.LogError("Create room marked as starting");
            return;
        }

        nodes.Clear();
        NavNode[] allNodes = FindObjectsOfType<NavNode>();
        for (int i = 0; i < allNodes.Length; i++)
        {
            DestroyImmediate(allNodes[i].gameObject);
        }

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


        List<DoorNav> stairsNodes = new List<DoorNav>();


       allRooms.ForEach(room =>
       {
           List<StairsScript> stairs = room.transform.GetComponentsInChildren<StairsScript>().ToList();

           NavNode mainNode = room.GetComponentInChildren<NavNode>();

           stairs.ForEach(stairs =>
           {
               NavNode temp = Instantiate(navNode, stairs.transform.position, Quaternion.identity, room.transform).GetComponent<NavNode>();
               temp.navNodeType = NavNode.NavNodeType.Stairs;
               stairs.node = temp;
               stairsNodes.Add(new DoorNav {node = temp, script = stairs });
               temp.AddConnectedNode(mainNode);

               List<NavNode> nodesToConnect = mainNode.GetConnectedNodes();
               temp.AddConnectedNode(nodesToConnect);
               nodesToConnect.ForEach(nodeToConnect =>
               {
                   nodeToConnect.AddConnectedNode(temp);
               });
           });

           stairs.ForEach(stairs => 
           {
               mainNode.AddConnectedNode(stairs.node);
           });
       });

        stairsNodes.ForEach(stairsNode =>
        {
            stairsNode.node.AddConnectedNode(stairsNode.script.GetConnected().node);
        });

    }
}
