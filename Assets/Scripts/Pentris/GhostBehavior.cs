using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    PentominoBehavior _parent;

    [SerializeField] BlockBehavior[] _ghostBlocks;

    void Start()
    {
        _parent = gameObject.GetComponentInParent<PentominoBehavior>();
    }

    public bool TryRotation(float z)
    {
        transform.SetPositionAndRotation(_parent.transform.position, _parent.transform.rotation);

        transform.Rotate(0, 0, z);

        return BoardBehavior.Instance.CheckRotation(_ghostBlocks);
    }

    public void Hold()
    {
        transform.SetPositionAndRotation(_parent.transform.position, _parent.transform.rotation);
    }
}
