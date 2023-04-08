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
    NavNode targetNode;

    [SerializeField]
    GameObject navNode;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Queue<NavNode> GetPathTo(Vector2 from, Vector2 to)
    {
        Queue<NavNode> path = new Queue<NavNode>();

        NavNode currentNode = FindClosestWaypoint(from);
        NavNode toNode = FindClosestWaypoint(to);


        if (currentNode == null || toNode == null)
        {
            path.Enqueue(currentNode);
            return path;
        }

        SortedList<float, NavNode> openList = new SortedList<float, NavNode>();
        List<NavNode> closedList = new List<NavNode>();
        openList.Add(0, currentNode);

        currentNode.previous = null;
        currentNode.distance = 0f;

        while (openList.Count > 0)
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

    public Queue<NavNode> GetPathToScientist(Vector2 from)
    {
        Queue<NavNode> path = new Queue<NavNode>();
        NavNode currentNode = FindClosestWaypoint(from);
        if(currentNode == null) return path;

        path.Enqueue(currentNode);

        while (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            path.Enqueue(currentNode);
        }

        return path;
    }

    [SerializeField]
    LayerMask navNodeLayerMask;
    private NavNode FindClosestWaypoint(Vector2 target, bool toScientist = false)
    {
        NavNode closest = null;
        float closestDist = Mathf.Infinity;
        foreach (NavNode waypoint in nodes)
        {
            var dist = ((Vector2)waypoint.transform.position - target).magnitude + (toScientist ? waypoint.distanceToScientist : 0);
            if (dist < closestDist)
            {
                if(!Physics2D.Linecast(waypoint.transform.position, target, ~navNodeLayerMask))
                {
                    closest = waypoint;
                    closestDist = dist;
                }
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

    void AddWeigthToNodes()
    {
        List<NavNode> closedList = new List<NavNode>();
        List<NavNode> openList = new List<NavNode>();

        targetNode.distanceToScientist = 0;
        openList.Add(targetNode);

        while (openList.Count > 0)
        {
            targetNode = openList[0];
            openList.RemoveAt(0);
            closedList.Add(targetNode);

            float distance = targetNode.distanceToScientist;

            targetNode.GetConnectedNodes().ForEach(n =>
            {
                if(!(openList.Contains(n) || closedList.Contains(n)))
                {
                    n.distanceToScientist = distance + Vector2.Distance(targetNode.transform.position, n.transform.position);
                    n.nextNode = targetNode;
                    openList.Add(n);
                }
            });
            openList.Sort((first, second) => first.distanceToScientist.CompareTo(second.distanceToScientist));
        }
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
            if(r == targetRoom) targetNode = nodes[nodes.Count - 1];
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
        AddWeigthToNodes();
    }
}
