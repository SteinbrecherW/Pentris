using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentominoBehavior : MonoBehaviour
{
    public BlockBehavior[] Blocks;

    BoardBehavior _parent;

    PlayerBehavior _player;

    [SerializeField] GhostBehavior _ghost;
    public GhostBehavior Ghost { get => _ghost; }

    [SerializeField] ShadowBehavior _shadow;
    public ShadowBehavior Shadow { get => _shadow; }

    SpriteRenderer[] _sprites;

    IEnumerator _fallCoroutine;
    bool _falling;
    public bool Falling
    {
        get => _falling;
        set
        {
            _falling = value;

            if (value)
                StartCoroutine(_fallCoroutine);
            else
                StopCoroutine(_fallCoroutine);
        }
    }

    bool _waitingLeft;
    bool _waitingRight;
    bool _waitingDown;
    public bool Waiting
    {
        get => _waitingLeft && _waitingRight && _waitingDown;
        set
        {
            _waitingLeft = value;
            _waitingRight = value;
            _waitingDown = value;
        }
    }

    bool _holdingLeft;
    bool _holdingRight;
    bool _holdingDown;

    public bool IsGhost = false;

    public bool IsNext;

    [SerializeField] float _xOffset;
    public float XOffset { get => _xOffset; }
    [SerializeField] float _yOffset;
    public float YOffset { get => _yOffset; }

    [SerializeField] string _name;

    void Start()
    {
        if (!IsNext)
        {
            GameBehavior.Instance.ActivePentominoes.Add(this);

            _parent = GetComponentInParent<BoardBehavior>();
            _player = GetComponentInParent<PlayerBehavior>();

            _ghost = GetComponentInChildren<GhostBehavior>();
            _shadow = GetComponentInChildren<ShadowBehavior>();

            _fallCoroutine = DropOverTime();

            Reset();
        }
    }

    public void Reset()
    {
        _parent.UpdateCurrentPosition(Blocks);

        Waiting = false;
        Falling = true;

        _holdingLeft = false;
        _holdingRight = false;
        _holdingDown = false;
    }

    void Update()
    {
        if (GameBehavior.Instance.CurrentState == GameBehavior.GameState.Running)
        {
            if (_falling)
            {
                if (Input.GetKey(_player.MoveLeft) && !_waitingLeft)
                    MoveLeft();
                
                if (Input.GetKey(_player.MoveRight) && !_waitingRight)
                    MoveRight();
                
                if (Input.GetKey(_player.MoveDown) && !_waitingDown)
                    MoveDown();

                
                if (Input.GetKeyDown(_player.RotateClockwise))
                    RotateClockwise();

                if (Input.GetKeyDown(_player.HardDrop))
                    HardDrop();

                if (Input.GetKeyDown(_player.Hold))
                    Hold();


                if (Input.GetKeyUp(_player.MoveLeft))
                    _holdingLeft = false;

                if (Input.GetKeyUp(_player.MoveRight))
                    _holdingRight = false;

                if (Input.GetKeyUp(_player.MoveDown))
                    _holdingDown = false;
            }
        }
    }

    IEnumerator DropOverTime()
    {
        while (_falling)
        {
            yield return new WaitForSeconds(BoardBehavior.Instance.FallSpeed);
            if(!_waitingDown && GameBehavior.Instance.CurrentState == GameBehavior.GameState.Running)
                MoveDown();
        }
    }

    void MoveLeft()
    {
        if (_parent.CheckTranslation(-1, 0))
        {
            transform.Translate(new Vector3(-0.5f, 0, 0), Space.World);
            _parent.UpdateTranslation(-1, 0);
            _shadow.UpdateMovement();

            if (_holdingLeft)
            {
                StartCoroutine(WaitLeft(GameBehavior.Instance.CursorMoveDelay));
            }
            else
            {
                _holdingLeft = true;
                StartCoroutine(WaitLeft(GameBehavior.Instance.CursorInitialDelay));
            }
        }
    }

    void MoveRight()
    {
        if (BoardBehavior.Instance.CheckTranslation(1, 0))
        {
            transform.Translate(new Vector3(0.5f, 0, 0), Space.World);
            _parent.UpdateTranslation(1, 0);
            _shadow.UpdateMovement();

            if (_holdingRight)
            {
                StartCoroutine(WaitRight(GameBehavior.Instance.CursorMoveDelay));
            }
            else
            {
                _holdingRight = true;
                StartCoroutine(WaitRight(GameBehavior.Instance.CursorInitialDelay));
            }
        }
    }

    void MoveDown()
    {
        if (_parent.CheckTranslation(0, -1))
        {
            transform.Translate(new Vector3(0, -0.5f, 0), Space.World);
            _parent.UpdateTranslation(0, -1);
            _shadow.UpdateMovement();

            if (_holdingDown)
            {
                StartCoroutine(WaitDown(GameBehavior.Instance.CursorMoveDelay));
            }
            else
            {
                _holdingDown = true;
                StartCoroutine(WaitDown(GameBehavior.Instance.CursorInitialDelay));
            }
        }
        else
        {
            LockIn();
        }
    }

    IEnumerator WaitLeft(float seconds)
    {
        _waitingLeft = true;
        yield return new WaitForSeconds(seconds);
        _waitingLeft = false;
    }

    IEnumerator WaitRight(float seconds)
    {
        _waitingRight = true;
        yield return new WaitForSeconds(seconds);
        _waitingRight = false;
    }

    IEnumerator WaitDown(float seconds)
    {
        _waitingDown = true;
        yield return new WaitForSeconds(seconds);
        _waitingDown = false;
    }

    void RotateClockwise()
    {
        if (_ghost.TryRotation(90))
        {
            transform.Rotate(0, 0, 90);
            _ghost.transform.Rotate(0, 0, -90);
            _parent.UpdateRotation();
            _shadow.UpdateMovement();
        }
    }

    void HardDrop()
    {
        transform.SetPositionAndRotation(_shadow.transform.position, _shadow.transform.rotation);
        _parent.UpdateTranslation(0, _shadow.yDepth);
        LockIn();
    }

    void Hold()
    {
        if (_parent.HoldPentomino(this, _xOffset, _yOffset))
        {
            Falling = false;
            Waiting = true;
            Shadow.IsHeld = true;
            Ghost.Hold();
        }
    }

    public void LockIn()
    {
        _falling = false;

        transform.DetachChildren();

        _shadow.LockIn();

        StartCoroutine(LockInAnimation());

        _parent.LockIn();
    }

    IEnumerator LockInAnimation()
    {
        _sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in _sprites)
        {
            Color newColor = s.color;
            newColor.a -= 50;
            s.color = newColor;
        }
        yield return new WaitForSeconds(0.2f);
        foreach (SpriteRenderer s in _sprites)
        {
            Color newColor = s.color;
            newColor.a += 50;
            s.color = newColor;
        }
    }

    public bool CheckEmpty()
    {
        foreach(BlockBehavior b in Blocks)
        {
            if (b != null)
                return false;
        }
        return true;
    }

    public void DestroyInstance()
    {
        GameBehavior.Instance.ActivePentominoes.Remove(this);
        Destroy(this);
    }
}
