using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    WALKABLE,
    DESTRUCTABLE,
    NONDESTRUCTABLE
}

public class Node
{
    private NodeState nodeState;
    private Vector2 position;

    public Node(NodeState _nodeState, Vector2 _position)
    {
        nodeState = _nodeState;
        position = _position;
    }
}
