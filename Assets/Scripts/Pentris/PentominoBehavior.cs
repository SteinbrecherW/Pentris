using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentominoBehavior : MonoBehaviour
{
    //Child blocks
    public GameObject[] Blocks;

    //Parent board
    public BoardBehavior Parent;

    //Current player
    public PlayerBehavior Player;

    //Child ghost
    [SerializeField] GhostBehavior _ghost;
    public GhostBehavior Ghost { get => _ghost; }

    //Child shadow
    [SerializeField] ShadowBehavior _shadow;
    public ShadowBehavior Shadow { get => _shadow; }

    //Coroutine for falling over time
    public IEnumerator FallCoroutine;

    //Are we falling?
    bool _falling;
    public bool Falling
    {
        get => _falling;
        set
        {
            _falling = value;

            //Toggle the coroutine for falling over time
            if (value)
                StartCoroutine(FallCoroutine);
            else
                StopCoroutine(FallCoroutine);
        }
    }

    //Are we allowed to move in each direction?
    //Used for timing when holding a button down
    public bool WaitingLeft;
    public bool WaitingRight;
    public bool WaitingDown;
    public bool Waiting
    {
        get => WaitingLeft && WaitingRight && WaitingDown;
        set
        {
            WaitingLeft = value;
            WaitingRight = value;
            WaitingDown = value;
        }
    }

    //Are we holding the button for any direction?
    public bool HoldingLeft;
    public bool HoldingRight;
    public bool HoldingDown;

    //Is this pentomino a ghost?
    public bool IsGhost = false;

    //Is this pentomino the next one up?
    public bool IsNext;

    //How much do we have to move the pieces so they fit in the held/next boxes?
    //Also used for spawning at the top of the board
    [SerializeField] float _xOffset;
    public float XOffset { get => _xOffset; }
    [SerializeField] float _yOffset;
    public float YOffset { get => _yOffset; }
    [SerializeField] float _yDepth;
    public float YDepth { get => _yDepth; }

    //Is this a power up?
    public bool IsPowerUp;

    //What's the name of this piece?
    //Not actually used, just for funsies
    [SerializeField] string _name;

    void Start()
    {
        if (!IsNext)
        {
            GameBehavior.Instance.ActivePentominoes.Add(this);

            Parent = GetComponentInParent<BoardBehavior>();
            Player = GetComponentInParent<PlayerBehavior>();

            _ghost = GetComponentInChildren<GhostBehavior>();
            _shadow = GetComponentInChildren<ShadowBehavior>();

            FallCoroutine = DropOverTime(BoardBehavior.Instance.FallSpeed);

            Reset();
        }
    }

    //Used to reset the board between games
    public virtual void Reset()
    {
        Parent.UpdateCurrentPosition(Blocks);

        Waiting = false;
        Falling = true;

        HoldingLeft = false;
        HoldingRight = false;
        HoldingDown = false;
    }

    //Checks for movement controls
    void Update()
    {
        if (GameBehavior.Instance.CurrentState == GameBehavior.GameState.Running)
        {
            if (_falling)
            {
                if (Input.GetKey(Player.MoveLeft) && !WaitingLeft)
                    MoveLeft();
                
                if (Input.GetKey(Player.MoveRight) && !WaitingRight)
                    MoveRight();
                
                if (Input.GetKey(Player.MoveDown) && !WaitingDown)
                    MoveDown();

                
                if (Input.GetKeyDown(Player.RotateClockwise))
                    RotateClockwise();

                if (Input.GetKeyDown(Player.HardDrop))
                    HardDrop();

                if (Input.GetKeyDown(Player.Hold))
                    Hold();


                if (Input.GetKeyUp(Player.MoveLeft))
                    HoldingLeft = false;

                if (Input.GetKeyUp(Player.MoveRight))
                    HoldingRight = false;

                if (Input.GetKeyUp(Player.MoveDown))
                    HoldingDown = false;
            }
        }
    }

    //Drops the piece over time
    public IEnumerator DropOverTime(float fallDelay)
    {
        Debug.Log("Coroutine Started: " + fallDelay);
        while (_falling)
        {
            yield return new WaitForSeconds(fallDelay);
            if(!WaitingDown && GameBehavior.Instance.CurrentState == GameBehavior.GameState.Running)
                MoveDown();
        }
    }

    //Translates the piece left
    public void MoveLeft()
    {
        if (Parent.CheckTranslation(-1, 0))
        {
            transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
            Parent.UpdateTranslation(-1, 0);

            //Power ups don't have shadows, so check before updating
            if(!IsPowerUp)
                _shadow.UpdateMovement();

            //If we're holding the piece, delay movement so it isn't too fast
            if (HoldingLeft)
            {
                StartCoroutine(WaitLeft(GameBehavior.Instance.CursorMoveDelay));
            }
            //If we aren't, wait a bit longer and hold the piece
            //Makes subtle one-block movements easier
            else
            {
                HoldingLeft = true;
                StartCoroutine(WaitLeft(GameBehavior.Instance.CursorInitialDelay));
            }
        }
    }

    //Translates the piece right
    //Similar implementation to "MoveLeft"
    public void MoveRight()
    {
        if (BoardBehavior.Instance.CheckTranslation(1, 0))
        {
            transform.Translate(new Vector3(0.5f, 0, 0), Space.World);
            Parent.UpdateTranslation(1, 0);

            if(!IsPowerUp)
                _shadow.UpdateMovement();

            if (HoldingRight)
            {
                StartCoroutine(WaitRight(GameBehavior.Instance.CursorMoveDelay));
            }
            else
            {
                HoldingRight = true;
                StartCoroutine(WaitRight(GameBehavior.Instance.CursorInitialDelay));
            }
        }
    }

    //Translates the piece right
    //Similar implementation to "MoveLeft"
    public void MoveDown()
    {
        if (Parent.CheckTranslation(0, -1))
        {
            transform.Translate(new Vector3(0, -0.5f, 0), Space.World);
            Parent.UpdateTranslation(0, -1);

            if(!IsPowerUp)
                _shadow.UpdateMovement();

            if (HoldingDown)
            {
                StartCoroutine(WaitDown(GameBehavior.Instance.CursorMoveDelay));
            }
            else
            {
                HoldingDown = true;
                StartCoroutine(WaitDown(GameBehavior.Instance.CursorInitialDelay));
            }
        }
        //If the piece can't move down, lock it in
        else
        {
            LockIn();
        }
    }

    //Don't let the piece move left for a bit
    IEnumerator WaitLeft(float seconds)
    {
        WaitingLeft = true;
        yield return new WaitForSeconds(seconds);
        WaitingLeft = false;
    }

    //Don't let the piece move right for a bit
    IEnumerator WaitRight(float seconds)
    {
        WaitingRight = true;
        yield return new WaitForSeconds(seconds);
        WaitingRight = false;
    }

    //Don't let the piece move down for a bit
    IEnumerator WaitDown(float seconds)
    {
        WaitingDown = true;
        yield return new WaitForSeconds(seconds);
        WaitingDown = false;
    }

    //Rotate the piece clockwise
    void RotateClockwise()
    {
        //If the ghost can do it, move the parent
        if (_ghost.TryRotation(90))
        {
            transform.Rotate(0, 0, 90);
            _ghost.transform.Rotate(0, 0, -90);
            Parent.UpdateRotation();
            _shadow.UpdateMovement();
        }
    }

    //Drops the piece to the bottom of the board
    public virtual void HardDrop()
    {
        //Moves parent to the current location of the shadow
        Falling = false;
        transform.SetPositionAndRotation(_shadow.transform.position, _shadow.transform.rotation);
        Parent.UpdateTranslation(0, _shadow.yDepth);
        LockIn();
    }

    //Holds a piece for future use
    public virtual void Hold()
    {
        if (Parent.HoldPentomino(this, _xOffset, _yOffset))
        {
            Falling = false;
            Waiting = true;
            Shadow.IsHeld = true;
            Ghost.Hold();
        }
    }

    //Locks the piece onto the board
    public virtual void LockIn()
    {
        Falling = false;

        //StartCoroutine(LockInAnimation());

        transform.DetachChildren();

        _shadow.LockIn();

        Parent.LockIn();
    }

    //IEnumerator LockInAnimation()
    //{
    //    _sprites = GetComponentsInChildren<SpriteRenderer>();
    //    yield return new WaitForSeconds(0.05f);
    //    foreach (SpriteRenderer s in _sprites)
    //    {
    //        Color newColor = s.color;
    //        newColor.a -= 0.5f;
    //        s.color = newColor;
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    foreach (SpriteRenderer s in _sprites)
    //    {
    //        Color newColor = s.color;
    //        newColor.a += 0.5f;
    //        s.color = newColor;
    //    }
    //}

    //Are the blocks in this piece all destroyed?
    public bool CheckEmpty()
    {
        foreach(GameObject b in Blocks)
        {
            if (b != null)
                return false;
        }
        return true;
    }

    //Removes pentomino from the board and destroys it
    public void DestroyInstance()
    {
        GameBehavior.Instance.ActivePentominoes.Remove(this);
        Destroy(gameObject);
    }
}
