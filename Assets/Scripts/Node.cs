using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    WALKABLE,
    DESTRUCTIBLE,
    NONDESTRUCTiBLE
}

public class Node : MonoBehaviour
{
    public NodeState nodeState;
    public Vector2Int positionInGrid;
    public SpriteRenderer spriteRenderer;

    // public Node(NodeState _nodeState, Vector2 _position)
    // {
    //     nodeState = _nodeState;
    //     position = _position;
    // }
}
