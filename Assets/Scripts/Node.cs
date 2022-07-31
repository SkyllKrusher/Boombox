using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    WALKABLE,
    DESTRICTABLE,
    NONDESTRUCTABLE
}

public class Node : MonoBehaviour
{
    public NodeState nodeState;
    public Vector2 position;
    public SpriteRenderer spriteRenderer;

    // public Node(NodeState _nodeState, Vector2 _position)
    // {
    //     nodeState = _nodeState;
    //     position = _position;
    // }
}
