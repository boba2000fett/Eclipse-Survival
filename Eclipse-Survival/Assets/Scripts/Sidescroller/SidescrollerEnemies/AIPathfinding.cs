using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathfinding
{
    static public List<Node> GenerateNodesForLevel()
    {
        // Initialize all nodes
        GameObject[] groundArray = GameObject.FindGameObjectsWithTag("Ground");
        Node[] groundNodes = new Node[groundArray.Length];
        for (int i = 0; i < groundArray.Length; i++)
        {
            groundNodes[i] = new Node(i, (Vector2)groundArray[i].transform.position + new Vector2(0f, 0.5f), true);
        }

        GameObject[] climbableArray = GameObject.FindGameObjectsWithTag("Climbable");
        Node[] climbableNodes = new Node[climbableArray.Length];
        for (int i = 0; i < climbableArray.Length; i++)
        {
            climbableNodes[i] = new Node(groundArray.Length + i, climbableArray[i].transform.position, false);
        }

        // Generate Edges
        for (int i = 0; i < groundNodes.Length; i++)
        {
            // Search for nearby ground
            Dictionary<int, float> groundNodeDirs = new Dictionary<int, float>();
            for (int c = 0; c < groundNodes.Length; c++)
            {
                if (i != c)
                {
                    Vector2 pos1 = groundNodes[i].position;
                    Vector2 pos2 = groundNodes[c].position;

                    if (Vector2.Distance(pos1, pos2) < Constants.AI_CONNECTION_DISTANCE)
                    {
                        // Angles are based as such
                        // Left is +/-180, Left is 0, down is 90, up is -90


                        // Check if it's directly above or below, cannot go through ground
                        float angle = Vector2.SignedAngle(pos2 - pos1, Vector2.right);
                        if ((angle < -145 || angle > 145) || (angle > -45 && angle < 45))
                        {
                            groundNodeDirs.Add(groundNodes[c].number, angle);
                        }
                    }
                }
            }
            // Only take one ground piece per 30 degree arc
            List<int> groundToRemove = new List<int>();
            foreach (KeyValuePair<int,float> kvStart in groundNodeDirs)
            {
                bool closest = true;
                foreach (KeyValuePair<int, float> kvEnd in groundNodeDirs)
                {
                    if (kvStart.Key != kvEnd.Key && !groundToRemove.Contains(kvStart.Key) && !groundToRemove.Contains(kvEnd.Key))
                    {
                        float angle1 = (kvStart.Value + 180f) % 360;
                        float angle2 = (kvEnd.Value + 180f) % 360;

                        float angleDiff1 = Mathf.Abs(angle1 - angle2);

                        angle1 = (kvStart.Value + 30f) % 360;
                        angle2 = (kvEnd.Value + 30f) % 360;

                        float angleDiff2 = Mathf.Abs(angle1 - angle2);

                        if (angleDiff1 < 15f || angleDiff2 < 15f)
                        {
                            Vector2 pos1 = groundNodes[i].position;
                            Vector2 node1 = groundNodes[kvStart.Key].position;
                            Vector2 node2 = groundNodes[kvEnd.Key].position;
                            float distDiff = Vector2.Distance(pos1, node1) - Vector2.Distance(pos1, node2);
                            if (distDiff > 0)
                            {
                                closest = false;
                                groundToRemove.Add(kvStart.Key);
                            } 
                            else groundToRemove.Add(kvEnd.Key);
                        }   
                    }
                    else if (groundToRemove.Contains(kvStart.Key)) closest = false;
                }
                if (closest)
                {
                    if (!groundNodes[i].connections.Contains(groundNodes[kvStart.Key])) groundNodes[i].connections.Add(groundNodes[kvStart.Key]);
                    if (!groundNodes[kvStart.Key].connections.Contains(groundNodes[i])) groundNodes[kvStart.Key].connections.Add(groundNodes[i]);
                }
            }
            // Now check for climbables
            Dictionary<int, float> climbNodeDirs = new Dictionary<int, float>();
            for (int c = 0; c < climbableNodes.Length; c++)
            {
                if (i != c)
                {
                    Vector2 pos1 = groundNodes[i].position;
                    Vector2 pos2 = climbableNodes[c].position;

                    if (Vector2.Distance(pos1, pos2) < Constants.AI_CONNECTION_DISTANCE)
                    {
                        // Check if it's directly below, cannot go through ground
                        float angle = Vector2.SignedAngle(pos2 - pos1, Vector2.right);

                        if ((angle > -45 && angle < 30) || (angle > 140 || angle < -135))
                        {
                            climbNodeDirs.Add(climbableNodes[c].number, angle);
                        }
                    }
                }
            }

            List<int> climbToRemove = new List<int>();
            foreach (KeyValuePair<int, float> kvStart in climbNodeDirs)
            {
                bool closest = true;
                foreach (KeyValuePair<int, float> kvEnd in climbNodeDirs)
                {
                    if (kvStart.Key != kvEnd.Key && !climbToRemove.Contains(kvStart.Key) && !climbToRemove.Contains(kvEnd.Key))
                    {
                        float angle1 = (kvStart.Value + 180f) % 360;
                        float angle2 = (kvEnd.Value + 180f) % 360;

                        float angleDiff1 = Mathf.Abs(angle1 - angle2);

                        angle1 = (kvStart.Value + 30f) % 360;
                        angle2 = (kvEnd.Value + 30f) % 360;

                        float angleDiff2 = Mathf.Abs(angle1 - angle2);

                        if (angleDiff1 < 15f || angleDiff2 < 15f)
                        {
                            Vector2 pos1 = groundNodes[i].position;
                            Vector2 node1 = climbableNodes[kvStart.Key - groundArray.Length].position;
                            Vector2 node2 = climbableNodes[kvEnd.Key - groundArray.Length].position;
                            float distDiff = Vector2.Distance(pos1, node1) - Vector2.Distance(pos1, node2);
                            if (distDiff > 0)
                            {
                                closest = false;
                                climbToRemove.Add(kvStart.Key);
                            }
                            else climbToRemove.Add(kvEnd.Key);
                        }
                    }
                    else if (climbToRemove.Contains(kvStart.Key)) closest = false;
                }
                if (closest)
                {
                    if (!groundNodes[i].connections.Contains(climbableNodes[kvStart.Key - groundArray.Length])) groundNodes[i].connections.Add(climbableNodes[kvStart.Key - groundArray.Length]);
                    if (!climbableNodes[kvStart.Key - groundArray.Length].connections.Contains(groundNodes[i])) climbableNodes[kvStart.Key - groundArray.Length].connections.Add(groundNodes[i]);
                }
            }
        }

        for (int i = 0; i < climbableNodes.Length; i++)
        {
            // Ground-climbable connections have already been checked, now check climb-climb connections
            Dictionary<int, float> climbNodeDirs = new Dictionary<int, float>();
            for (int c = 0; c < climbableNodes.Length; c++)
            {
                if (i != c)
                {
                    Vector2 pos1 = climbableNodes[i].position;
                    Vector2 pos2 = climbableNodes[c].position;

                    if (Vector2.Distance(pos1, pos2) < Constants.AI_CONNECTION_DISTANCE)
                    {
                        float angle = Vector2.SignedAngle(pos2 - pos1, Vector2.right);

                        climbNodeDirs.Add(climbableNodes[c].number, angle);
                    }
                }
            }
            List<int> climbToRemove = new List<int>();
            foreach (KeyValuePair<int, float> kvStart in climbNodeDirs)
            {
                bool closest = true;
                foreach (KeyValuePair<int, float> kvEnd in climbNodeDirs)
                {
                    if (kvStart.Key != kvEnd.Key && !climbToRemove.Contains(kvStart.Key) && !climbToRemove.Contains(kvEnd.Key))
                    {
                        float angle1 = (kvStart.Value + 180f) % 360;
                        float angle2 = (kvEnd.Value + 180f) % 360;

                        float angleDiff1 = Mathf.Abs(angle1 - angle2);

                        angle1 = (kvStart.Value + 30f) % 360;
                        angle2 = (kvEnd.Value + 30f) % 360;

                        float angleDiff2 = Mathf.Abs(angle1 - angle2);

                        if (angleDiff1 < 15f || angleDiff2 < 15f)
                        {
                            Vector2 pos1 = climbableNodes[i].position;
                            Vector2 node1 = climbableNodes[kvStart.Key - groundArray.Length].position;
                            Vector2 node2 = climbableNodes[kvEnd.Key - groundArray.Length].position;
                            float distDiff = Vector2.Distance(pos1, node1) - Vector2.Distance(pos1, node2);
                            if (distDiff > 0)
                            {
                                closest = false;
                                climbToRemove.Add(kvStart.Key);
                            }
                            else climbToRemove.Add(kvEnd.Key);
                        }
                    }
                    else if (climbToRemove.Contains(kvStart.Key)) closest = false;
                }
                if (closest)
                {
                    if (!climbableNodes[i].connections.Contains(climbableNodes[kvStart.Key - groundArray.Length])) climbableNodes[i].connections.Add(climbableNodes[kvStart.Key - groundArray.Length]);
                    if (!climbableNodes[kvStart.Key - groundArray.Length].connections.Contains(climbableNodes[i])) climbableNodes[kvStart.Key - groundArray.Length].connections.Add(climbableNodes[i]);
                }
            }
        }

        // Fixes:
        //Node Lengths:
        //182 / 183 for Bathroom - Bedroom
        //124 / 125 for Hallway - BedRoom
        //103 / 104 for Kitchen - Bedroom

        // If in Hallway SideScene
        if (groundArray.Length + climbableArray.Length == 125)
        {
            KeyValuePair<int, int>[] addConnections = new KeyValuePair<int, int>[]
            {
                //new KeyValuePair<int, int>(52, 75),
                new KeyValuePair<int, int>(80, 94),
                new KeyValuePair<int, int>(81, 94),
                new KeyValuePair<int, int>(81, 95),
                new KeyValuePair<int, int>(54, 84),
                new KeyValuePair<int, int>(96, 74),
                new KeyValuePair<int, int>(53, 87),
                new KeyValuePair<int, int>(53, 90),
                new KeyValuePair<int, int>(90, 89),
                new KeyValuePair<int, int>(79, 91)
            };

            for (int i = 0; i < addConnections.Length; i++)
            {
                if (addConnections[i].Key >= groundNodes.Length)
                {
                    if (addConnections[i].Value >= groundNodes.Length)
                    {
                        climbableNodes[addConnections[i].Key - groundNodes.Length].connections.Add(climbableNodes[addConnections[i].Value - groundNodes.Length]);
                        climbableNodes[addConnections[i].Value - groundNodes.Length].connections.Add(climbableNodes[addConnections[i].Key - groundNodes.Length]);
                    }
                    else
                    {
                        climbableNodes[addConnections[i].Key - groundNodes.Length].connections.Add(groundNodes[addConnections[i].Value]);
                        groundNodes[addConnections[i].Value].connections.Add(climbableNodes[addConnections[i].Key - groundNodes.Length]);
                    }
                }
                else
                {
                    if (addConnections[i].Value >= groundNodes.Length)
                    {
                        groundNodes[addConnections[i].Key].connections.Add(climbableNodes[addConnections[i].Value - groundNodes.Length]);
                        climbableNodes[addConnections[i].Value - groundNodes.Length].connections.Add(groundNodes[addConnections[i].Key]);
                    }
                    else
                    {
                        groundNodes[addConnections[i].Key].connections.Add(groundNodes[addConnections[i].Value]);
                        groundNodes[addConnections[i].Value].connections.Add(groundNodes[addConnections[i].Key]);
                    }
                }
            }

            KeyValuePair<int, int>[] removeConnections = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(122, 123),
                new KeyValuePair<int, int>(108, 109),
                new KeyValuePair<int, int>(72, 119),
                new KeyValuePair<int, int>(72, 121)
            };

            for (int i = 0; i < removeConnections.Length; i++)
            {
                if (removeConnections[i].Key >= groundNodes.Length)
                {
                    if (removeConnections[i].Value >= groundNodes.Length)
                    {
                        climbableNodes[removeConnections[i].Key - groundNodes.Length].connections.Remove(climbableNodes[removeConnections[i].Value - groundNodes.Length]);
                        climbableNodes[removeConnections[i].Value - groundNodes.Length].connections.Remove(climbableNodes[removeConnections[i].Key - groundNodes.Length]);
                    }
                    else
                    {
                        climbableNodes[removeConnections[i].Key - groundNodes.Length].connections.Remove(groundNodes[removeConnections[i].Value]);
                        groundNodes[removeConnections[i].Value].connections.Remove(climbableNodes[removeConnections[i].Key - groundNodes.Length]);
                    }
                }
                else
                {
                    if (removeConnections[i].Value >= groundNodes.Length)
                    {
                        groundNodes[removeConnections[i].Key].connections.Remove(climbableNodes[removeConnections[i].Value - groundNodes.Length]);
                        climbableNodes[removeConnections[i].Value - groundNodes.Length].connections.Remove(groundNodes[removeConnections[i].Key]);
                    }
                    else
                    {
                        groundNodes[removeConnections[i].Key].connections.Remove(groundNodes[removeConnections[i].Value]);
                        groundNodes[removeConnections[i].Value].connections.Remove(groundNodes[removeConnections[i].Key]);
                    }
                }
            }
        }
        else if (groundArray.Length + climbableArray.Length == 184)
        {
            KeyValuePair<int, int>[] addConnections = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(45, 46),
                new KeyValuePair<int, int>(58, 84),
                new KeyValuePair<int, int>(26, 144)
            };

            for (int i = 0; i < addConnections.Length; i++)
            {
                if (addConnections[i].Key >= groundNodes.Length)
                {
                    if (addConnections[i].Value >= groundNodes.Length)
                    {
                        climbableNodes[addConnections[i].Key - groundNodes.Length].connections.Add(climbableNodes[addConnections[i].Value - groundNodes.Length]);
                        climbableNodes[addConnections[i].Value - groundNodes.Length].connections.Add(climbableNodes[addConnections[i].Key - groundNodes.Length]);
                    }
                    else
                    {
                        climbableNodes[addConnections[i].Key - groundNodes.Length].connections.Add(groundNodes[addConnections[i].Value]);
                        groundNodes[addConnections[i].Value].connections.Add(climbableNodes[addConnections[i].Key - groundNodes.Length]);
                    }
                }
                else
                {
                    if (addConnections[i].Value >= groundNodes.Length)
                    {
                        groundNodes[addConnections[i].Key].connections.Add(climbableNodes[addConnections[i].Value - groundNodes.Length]);
                        climbableNodes[addConnections[i].Value - groundNodes.Length].connections.Add(groundNodes[addConnections[i].Key]);
                    }
                    else
                    {
                        groundNodes[addConnections[i].Key].connections.Add(groundNodes[addConnections[i].Value]);
                        groundNodes[addConnections[i].Value].connections.Add(groundNodes[addConnections[i].Key]);
                    }
                }
            }

            KeyValuePair<int, int>[] removeConnections = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(150, 151),
                new KeyValuePair<int, int>(64, 155),
                new KeyValuePair<int, int>(68, 69),
                new KeyValuePair<int, int>(69, 70),
                new KeyValuePair<int, int>(69, 102),
                new KeyValuePair<int, int>(69, 103),
                new KeyValuePair<int, int>(69, 177),
                new KeyValuePair<int, int>(103, 177),
                new KeyValuePair<int, int>(70, 177),
                new KeyValuePair<int, int>(102, 176),
                new KeyValuePair<int, int>(70, 176),
                new KeyValuePair<int, int>(101, 175),
                new KeyValuePair<int, int>(98, 174),
                new KeyValuePair<int, int>(174, 182),
                new KeyValuePair<int, int>(100, 172),
                new KeyValuePair<int, int>(99, 173),
                new KeyValuePair<int, int>(98, 173),
                new KeyValuePair<int, int>(21, 34),
                new KeyValuePair<int, int>(21, 76),
                new KeyValuePair<int, int>(13, 75),
                new KeyValuePair<int, int>(13, 76),
                new KeyValuePair<int, int>(77, 112),
                new KeyValuePair<int, int>(78, 112),
                new KeyValuePair<int, int>(79, 112),
                new KeyValuePair<int, int>(82, 112),
                new KeyValuePair<int, int>(111, 112),
                new KeyValuePair<int, int>(77, 113),
                new KeyValuePair<int, int>(78, 113),
                new KeyValuePair<int, int>(79, 113),
                new KeyValuePair<int, int>(57, 60),
                new KeyValuePair<int, int>(59, 85)
            };

            for (int i = 0; i < removeConnections.Length; i++)
            {
                if (removeConnections[i].Key >= groundNodes.Length)
                {
                    if (removeConnections[i].Value >= groundNodes.Length)
                    {
                        climbableNodes[removeConnections[i].Key - groundNodes.Length].connections.Remove(climbableNodes[removeConnections[i].Value - groundNodes.Length]);
                        climbableNodes[removeConnections[i].Value - groundNodes.Length].connections.Remove(climbableNodes[removeConnections[i].Key - groundNodes.Length]);
                    }
                    else
                    {
                        climbableNodes[removeConnections[i].Key - groundNodes.Length].connections.Remove(groundNodes[removeConnections[i].Value]);
                        groundNodes[removeConnections[i].Value].connections.Remove(climbableNodes[removeConnections[i].Key - groundNodes.Length]);
                    }
                }
                else
                {
                    if (removeConnections[i].Value >= groundNodes.Length)
                    {
                        groundNodes[removeConnections[i].Key].connections.Remove(climbableNodes[removeConnections[i].Value - groundNodes.Length]);
                        climbableNodes[removeConnections[i].Value - groundNodes.Length].connections.Remove(groundNodes[removeConnections[i].Key]);
                    }
                    else
                    {
                        groundNodes[removeConnections[i].Key].connections.Remove(groundNodes[removeConnections[i].Value]);
                        groundNodes[removeConnections[i].Value].connections.Remove(groundNodes[removeConnections[i].Key]);
                    }
                }
            }
        }

        List<Node> graph = new List<Node>();
        graph.AddRange(groundNodes);
        graph.AddRange(climbableNodes);

        return graph;

        #region Testing Code
        /*
        List<Vector2Int> edges = new List<Vector2Int>();
        foreach (Node n in groundNodes) 
        {
            GameObject Node = new GameObject("Node" + n.number);
            Node.transform.position = n.position;

            foreach (Node con in n.connections)
            {
                if (!edges.Contains(new Vector2Int(n.number, con.number))
                    && !edges.Contains(new Vector2Int(con.number, n.number)))
                {
                    GameObject Edge = new GameObject("Edge" + n.number + "To" + con.number);

                    Edge.transform.rotation = Quaternion.Euler(Vector2.SignedAngle(con.position - n.position, Vector2.right), 0f, 0f);
                    Edge.transform.position = Vector2.Lerp(n.position, con.position, Vector2.Distance(n.position, con.position) / 2);
                    edges.Add(new Vector2Int(n.number, con.number));
                }
            }
        }

        foreach (Node n in climbableNodes)
        {
            GameObject Node = new GameObject("Node" + n.number);
            Node.transform.position = n.position;

            foreach (Node con in n.connections)
            {
                if (!edges.Contains(new Vector2Int(n.number, con.number))
                    && !edges.Contains(new Vector2Int(con.number, n.number)))
                {
                    GameObject Edge = new GameObject("Edge" + n.number + "To" + con.number);

                    Edge.transform.rotation = Quaternion.Euler(Vector2.SignedAngle(con.position - n.position, Vector2.right), 0f, 0f);
                    Edge.transform.position = Vector2.Lerp(n.position, con.position, Vector2.Distance(n.position, con.position) / 2);
                    edges.Add(new Vector2Int(n.number, con.number));
                }
            }
        }
        return graph;
        */
        #endregion
    }

    static public List<Node> RecreatePath(Dictionary<Node, Node> cameFrom, Node current)
    {
        List<Node> path = new List<Node>();
        path.Insert(0, current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    // A* Search
    static public List<Node> AStar(Node start, Node target)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(start);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        while (openList.Count != 0)
        {

            float leastF = float.MaxValue;
            Node q = openList[0];
            foreach (Node n in openList)
            {
                if (n.f < leastF)
                {
                    q = n;
                    leastF = n.f;
                }
            }

            openList.Remove(q);

            List<Node> successors = new List<Node>();
            foreach (Node conn in q.connections)
            {
                Node s = new Node(conn.number, conn.position, conn.isGround);
                s.connections = conn.connections;
                successors.Add(s);
            }

            foreach (Node succ in successors)
            {
                if (succ.number == target.number)
                {
                    cameFrom.Add(succ, q);
                    return RecreatePath(cameFrom, succ);
                }

                succ.g = q.g + Vector2.Distance(succ.position, q.position);
                succ.h = Vector2.Distance(succ.position, target.position);
                succ.f = succ.g + succ.h;
                
                bool skip = false;
                bool contains = false;
                foreach(Node n in openList)
                {
                    if (n.number == succ.number)
                    {
                        contains = true;
                        if (n.f < succ.f) skip = true;
                    }
                }

                if (!skip)
                {
                    foreach (Node n in closedList)
                    {
                        if (n.number == succ.number)
                        {
                            if (n.f < succ.f) skip = true;
                        }
                    }
                }

                if (!skip && !contains)
                {
                    cameFrom.Add(succ, q);
                    openList.Add(succ);
                }
                else if (!skip && contains)
                {
                    foreach (Node n in openList)
                    {
                        if (n.number == succ.number)
                        {
                            if (!cameFrom.ContainsKey(n)) cameFrom.Add(n, q);
                            else cameFrom[n] = q;
                            n.f = succ.f;
                            n.g = succ.g;
                            n.h = succ.h;
                        }
                    }
                }
            }
            
            closedList.Insert(0, q);
        }

        throw new System.Exception("Broke");
    }
}
public class Node
{
    static public Node GetClosestNode(Vector2 pos, List<Node> graph)
    {
        float minDist = float.MaxValue;
        int closest = -1;
        for (int i = 0; i < graph.Count; i++)
        {
            float dist = Vector2.Distance(graph[i].position, pos);
            if (minDist > dist)
            {
                closest = i;
                minDist = dist;
            }
        }

        return graph[closest];
    }

    [Header("Set In Inspector")]
    public List<Node> connections;
    public Vector2 position;
    public int number;
    public bool isGround;

    [Header("Set Dynamically")]
    public float f, h, g;

    public Node(int num, Vector2 pos, bool floor)
    {
        number = num;
        position = pos;
        connections = new List<Node>();
        f = 0;
        g = 0;
        isGround = floor;
    }

}
