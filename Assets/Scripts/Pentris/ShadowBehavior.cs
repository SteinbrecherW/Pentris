using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehavior : MonoBehaviour
{
    PentominoBehavior _parent;
    SpriteRenderer[] _children;

    public int yDepth;

    bool _isHeld;
    public bool IsHeld
    {
        get => _isHeld;
        set
        {
            foreach (SpriteRenderer sr in _children)
                sr.enabled = !value;
        }
    }

    private void Start()
    {
        _parent = GetComponentInParent<PentominoBehavior>();
        if (!_parent.IsNext)
        {
            _children = GetComponentsInChildren<SpriteRenderer>();
            yDepth = 0;
            UpdateMovement();
        }
    }

    public void UpdateMovement()
    {
        if (!_parent.Waiting)
        {
            transform.SetPositionAndRotation(_parent.gameObject.transform.position, _parent.gameObject.transform.rotation);
            yDepth = 0;
            while (BoardBehavior.Instance.CheckTranslation(0, yDepth - 1))
            {
                yDepth--;
            }
            transform.Translate(new Vector3(0, yDepth / 2.0f, 0), Space.World);
            BoardBehavior.Instance.CheckShadowUpdate(yDepth, _children);
        }
    }

    public void LockIn()
    {
        foreach (SpriteRenderer g in _children)
            g.enabled = false;
    }
}
