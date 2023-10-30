using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehavior : PentominoBehavior
{
    //Current position on board
    public int XPos= 0;
    public int YPos = 0;

    //Current power up type
    public int PowerUpIndex = 0;

    //Which blocks to destroy if the power up is a bomb
    int[,] _blastZone = new int[12, 2];
    int _blastZoneHeight = 0;

    //Audio parameters
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _powerUpAudio;
    [SerializeField] AudioClip _bombAudio;

    void Start()
    {
        IsPowerUp = true;

        Parent = GetComponentInParent<BoardBehavior>();
        Player = GetComponentInParent<PlayerBehavior>();

        FallCoroutine = DropOverTime(BoardBehavior.Instance.FallSpeed);

        Falling = false;
        Waiting = true;

        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!Falling)
        {
            if (Input.GetKeyDown(Player.UsePowerUp))
            {
                _audio.PlayOneShot(_powerUpAudio);

                //Use clock if the power up is a clock 
                if (PowerUpIndex == 0)
                    StartCoroutine(UseClock());
                //Otherwise, spawn the bomb on the board and treat like a pentomino
                else
                {
                    foreach(GameObject gb in GameBehavior.Instance.CurrentPentomino.Blocks)
                    {
                        gb.GetComponent<SpriteRenderer>().enabled = false;
                    }

                    GameBehavior.Instance.CurrentPentomino.DestroyInstance();
                    GameBehavior.Instance.CurrentPentomino = this;
                    transform.SetPositionAndRotation(BoardBehavior.Instance.SpawnPoint, Quaternion.identity);

                    Parent.UpdateCurrentPosition(new GameObject[1] { gameObject });

                    Waiting = false;
                    Falling = true;

                    HoldingLeft = false;
                    HoldingRight = false;
                    HoldingDown = false;
                }
            }
        }
        //If falling, move by controls
        else
        {
            if (Input.GetKeyDown(Player.UsePowerUp))
                UseBomb();


            if (Input.GetKey(Player.MoveLeft) && !WaitingLeft)
                MoveLeft();

            if (Input.GetKey(Player.MoveRight) && !WaitingRight)
                MoveRight();

            if (Input.GetKey(Player.MoveDown) && !WaitingDown)
                MoveDown();


            if (Input.GetKeyUp(Player.MoveLeft))
                HoldingLeft = false;

            if (Input.GetKeyUp(Player.MoveRight))
                HoldingRight = false;

            if (Input.GetKeyUp(Player.MoveDown))
                HoldingDown = false;
        }
    }

    //If the bomb would lock in...
    public override void LockIn()
    {
        Falling = false;

        //...boom!
        UseBomb();
    }

    //Slows down time for a brief window
    IEnumerator UseClock()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        GameBehavior.Instance.CurrentPentomino.Falling = false;
        GameBehavior.Instance.CurrentPentomino.FallCoroutine = DropOverTime(GameBehavior.Instance.ClockSlowDownSpeed);
        GameBehavior.Instance.CurrentPentomino.Falling = true;

        yield return new WaitForSeconds(GameBehavior.Instance.ClockDuration);

        GameBehavior.Instance.CurrentPentomino.Falling = false;
        GameBehavior.Instance.CurrentPentomino.FallCoroutine = DropOverTime(BoardBehavior.Instance.FallSpeed);
        GameBehavior.Instance.CurrentPentomino.Falling = true;

        Destroy(this);
    }

    //Destroys blocks in a specified pattern
    void UseBomb()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        //Determines which blocks will be destroyed
        //Positions relative to current position of the bomb
        switch (PowerUpIndex)
        {
            //o
            case 1:
                _blastZone[0, 1] = 2;
                _blastZone[1, 0] = -1;
                _blastZone[1, 1] = 1;
                _blastZone[2, 1] = 1;
                _blastZone[3, 0] = 1;
                _blastZone[3, 1] = 1;
                _blastZone[4, 0] = -2;
                _blastZone[5, 0] = -1;
                _blastZone[6, 0] = 1;
                _blastZone[7, 0] = 2;
                _blastZone[8, 0] = -1;
                _blastZone[8, 1] = -1;
                _blastZone[9, 1] = -1;
                _blastZone[10, 0] = 1;
                _blastZone[10, 1] = -1;
                _blastZone[11, 1] = -2;
                _blastZoneHeight = 2;
                break;

            //+
            case 2:
                _blastZone[0, 1] = 3;
                _blastZone[1, 1] = 2;
                _blastZone[2, 1] = 1;
                _blastZone[3, 0] = -3;
                _blastZone[4, 0] = -2;
                _blastZone[5, 0] = -1;
                _blastZone[6, 0] = 1;
                _blastZone[7, 0] = 2;
                _blastZone[8, 0] = 3;
                _blastZone[9, 1] = -1;
                _blastZone[10, 1] = -2;
                _blastZone[11, 1] = -3;
                _blastZoneHeight = 3;
                break;

            //x
            case 3:
                _blastZone[0, 0] = -3;
                _blastZone[0, 1] = 3;
                _blastZone[1, 0] = -2;
                _blastZone[1, 1] = 2;
                _blastZone[2, 0] = -1;
                _blastZone[2, 1] = 1;
                _blastZone[3, 0] = -3;
                _blastZone[3, 1] = -3;
                _blastZone[4, 0] = -2;
                _blastZone[4, 1] = -2;
                _blastZone[5, 0] = -1;
                _blastZone[5, 1] = -1;
                _blastZone[6, 0] = 3;
                _blastZone[6, 1] = 3;
                _blastZone[7, 0] = 2;
                _blastZone[7, 1] = 2;
                _blastZone[8, 0] = 1;
                _blastZone[8, 1] = 1;
                _blastZone[9, 0] = 3;
                _blastZone[9, 1] = -3;
                _blastZone[10, 0] = 2;
                _blastZone[10, 1] = -2;
                _blastZone[11, 0] = 1;
                _blastZone[11, 1] = -1;
                _blastZoneHeight = 3;
                break;
        }

        //Destroy every block in the array of positions
        for(int i = 0; i < 12; i++)
        {
            int x = BoardBehavior.Instance.CurrentPosition[0, 0] + _blastZone[i, 0];
            int y = BoardBehavior.Instance.CurrentPosition[0, 1] + _blastZone[i, 1];

            if (x >= 0 &&
                y >= 0 &&
                x < BoardBehavior.Instance.BoardWidth &&
                x < BoardBehavior.Instance.BoardWidth &&
                BoardBehavior.Instance.Board[x, y] != null
            )
            {
                Destroy(BoardBehavior.Instance.Board[x, y].gameObject);

                //Add 20 points per block destroyed
                BoardBehavior.Instance.Player.Score = 20;
            }
        }

        //Check all of the rows that had destroyed blocks
        //If any are empty, move all blocks above them down
        for(int i = _blastZoneHeight * -1; i <= _blastZoneHeight; i++)
        {
            int y = BoardBehavior.Instance.CurrentPosition[0, 0] + i;
            if (y >= 0 && y < BoardBehavior.Instance.BoardHeight && BoardBehavior.Instance.CheckRowEmpty(y))
            {
                BoardBehavior.Instance.DestroyRow(BoardBehavior.Instance.CurrentPosition[0, 0] + i, true);
                _blastZoneHeight--;
            }
        }

        _audio.PlayOneShot(_bombAudio);

        BoardBehavior.Instance.LockIn();

        Destroy(this);
    }
}
