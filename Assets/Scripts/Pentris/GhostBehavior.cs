using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    //Parent pentomino
    PentominoBehavior _parent;

    //Blocks
    [SerializeField] BlockBehavior[] _ghostBlocks;

    void Start()
    {
        _parent = gameObject.GetComponentInParent<PentominoBehavior>();
    }

    //Can we rotate?
    public bool TryRotation(float z)
    {
        transform.SetPositionAndRotation(_parent.transform.position, _parent.transform.rotation);

        transform.Rotate(0, 0, z);

        return BoardBehavior.Instance.CheckRotation(_ghostBlocks);
    }

    //If the piece is held, set position to parent
    public void Hold()
    {
        transform.SetPositionAndRotation(_parent.transform.position, _parent.transform.rotation);
    }
}
