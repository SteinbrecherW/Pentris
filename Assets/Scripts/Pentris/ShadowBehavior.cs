using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehavior : MonoBehaviour
{
    //Pentomino that acts as the parent
    PentominoBehavior _parent;
    //Child blocks
    SpriteRenderer[] _children;

    //How far the shadow is down from the pentomino
    public int yDepth;

    //Is this pentomino currently being held?
    bool _isHeld;
    public bool IsHeld
    {
        get => _isHeld;
        set
        {
            //Toggle sprites on/off
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

    //Updates movement (duh)
    public void UpdateMovement()
    {
        //If the parent isn't waiting...
        if (!_parent.Waiting)
        {
            //Set position to equal that of parent pentomino
            transform.SetPositionAndRotation(_parent.gameObject.transform.position, _parent.gameObject.transform.rotation);
            yDepth = 0;

            //Translate down until no longer possible
            while (BoardBehavior.Instance.CheckTranslation(0, yDepth - 1, true))
            {
                yDepth--;
            }

            //Translate to lowest possible position, check the update
            transform.Translate(new Vector3(0, yDepth / 2.0f, 0), Space.World);
            BoardBehavior.Instance.CheckShadowUpdate(yDepth, _children);
        }
    }

    //If the pentomino gets locked in...
    public void LockIn()
    {
        //Turn off all sprites
        foreach (SpriteRenderer g in _children)
            g.enabled = false;
    }
}
