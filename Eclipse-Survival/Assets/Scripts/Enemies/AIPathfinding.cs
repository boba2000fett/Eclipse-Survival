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
            groundNodes[i] = new Node(i, (Vector2)groundArray[i].transform.position + new Vector2(0f, 0.2f));
        }

        GameObject[] climbableArray = GameObject.FindGameObjectsWithTag("Climbable");
        Node[] climbableNodes = new Node[climbableArray.Length];
        for (int i = 0; i < climbableArray.Length; i++)
        {
            climbableNodes[i] = new Node(groundArray.Length + i, climbableArray[i].transform.position);
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
                if (closest) groundNodes[i].connections.Add(groundNodes[kvStart.Key]);
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
                        if (angle < 45 || angle > 135)
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
                    groundNodes[i].connections.Add(climbableNodes[kvStart.Key - groundArray.Length]);
                    climbableNodes[kvStart.Key - groundArray.Length].connections.Add(groundNodes[i]);
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
                if (closest) climbableNodes[i].connections.Add(climbableNodes[kvStart.Key - groundArray.Length]);
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

                    Edge.transform.rotation = Quaternion.Euler(Vector2.SignedAngle(n.position - con.position, Vector2.right), 0f, 0f);
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

                    Edge.transform.rotation = Quaternion.Euler(Vector2.SignedAngle(n.position - con.position, Vector2.right), 0f, 0f);
                    Edge.transform.position = Vector2.Lerp(n.position, con.position, Vector2.Distance(n.position, con.position) / 2);
                    edges.Add(new Vector2Int(n.number, con.number));
                }
            }
        }*/
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
                Node s = new Node(conn.number, conn.position);
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

    [Header("Set Dynamically")]
    public float f, h, g;

    public Node(int num, Vector2 pos)
    {
        number = num;
        position = pos;
        connections = new List<Node>();
        f = 0;
        g = 0;
    }

}
