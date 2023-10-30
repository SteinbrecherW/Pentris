using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehavior : MonoBehaviour
{
    //Singleton
    public static BoardBehavior Instance;

    //Current player
    [SerializeField] PlayerBehavior _player;
    public PlayerBehavior Player { get => _player; }

    //Board heat map
    public GameObject[,] Board;

    //Current blocks
    GameObject[] _currentBlocks = new GameObject[5];

    //Where to spawn pieces
    public Vector3 SpawnPoint = new Vector3(0.25f, 6, 0);

    //Currently held piece
    PentominoBehavior _heldPentomino = null;
    public PentominoBehavior HeldPentomino { get => _heldPentomino; }
    //Where to hold the piece
    [SerializeField] Vector3 _holdPosition = new Vector3(-5, 5, 0);
    int holdTurn = -1;

    //Currently held power up
    PowerUpBehavior _heldPowerUp = new PowerUpBehavior();
    public PowerUpBehavior HeldPowerUp { get => _heldPowerUp; }
    //Where to hold the power up
    [SerializeField] Vector3 _powerPosition = new Vector3(-5, 5, 0);

    //Where to hold the next piece
    public Vector3 NextPosition = new Vector3(-5, 4, 0);

    //Board width and height
    public int BoardWidth = 12;
    public int BoardHeight = 25;

    //Left boundary
    [SerializeField] float _xMin = -2.75f;
    public float XMin { get => _xMin; }

    //Bottom boundary
    [SerializeField] float _yMin = -4.5f;
    public float YMin { get => _yMin; }

    //Fall speed increases over time
    [SerializeField] float[] _fallSpeedPerLevel = new float[20];
    public float FallSpeed { get => _fallSpeedPerLevel[_currentLevel - 1] / 20; }

    //Where the piece is currently located
    public int[,] CurrentPosition = new int[5, 2];
    //Temporary rotation location
    int[,] _tempRotation = new int[5, 2];

    //Level parameters
    public int StartingLevel = 1;
    int _currentLevel = 1;


    //Audio parameters
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _scoreAudio;
    [SerializeField] AudioClip _pickUpAudio;
    [SerializeField] AudioClip _lockInAudio;

    private void Awake()
    {
        //Singleton pattern
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        Board = new GameObject[BoardWidth, BoardHeight];
        _player = GetComponent<PlayerBehavior>();
        _audio = GetComponent<AudioSource>();
    }

    //Sets the starting level based on selection
    public void SetStartingLevel()
    {
        if (StartingLevel > 1)
        {
            _player.ScoreOffset = (StartingLevel - 1) * 1000;
            _player.Score = (StartingLevel - 1) * 1000;
            GameBehavior.Instance.LevelUp(StartingLevel);
            _currentLevel = StartingLevel;
            Debug.Log(FallSpeed);
        }
    }

    //Updates the current position of the piece
    public void UpdateCurrentPosition(GameObject[] blocks)
    {
        _currentBlocks = blocks;
        for(int i = 0; i < blocks.Length; i++)
        {
            CurrentPosition[i, 0] = (int) Mathf.Round((blocks[i].transform.position.x - _xMin) * 2);
            CurrentPosition[i, 1] = (int) Mathf.Round((blocks[i].transform.position.y - _yMin) * 2);
        }
    }

    //Can the piece move by x and y?
    public bool CheckTranslation(int x, int y, bool isShadow = false)
    {
        for (int i = 0; i < _currentBlocks.Length; i++)
        {
            int testX = CurrentPosition[i, 0] + x;
            int testY = CurrentPosition[i, 1] + y;

            if (testY < 0 || testX < 0 || testX >= BoardWidth || testY >= BoardHeight)
                return false;
            else if (Board[testX, testY] != null)
            {
                //If the detected object is a power up, pick it up
                if (Board[testX, testY].CompareTag("PowerUp"))
                {
                    if (!isShadow)
                    {
                        _heldPowerUp = Board[testX, testY].GetComponent<PowerUpBehavior>();
                        _heldPowerUp.transform.SetPositionAndRotation(_powerPosition, Quaternion.identity);
                        Board[testX, testY] = null;
                        _audio.PlayOneShot(_pickUpAudio);
                    }
                    return true;
                }
                else
                    return false;
            }
        }
        return true;
    }

    //Is the desired rotation possible?
    public bool CheckRotation(BlockBehavior[] blocks)
    {
        for (int i = 0; i < _currentBlocks.Length; i++)
        {
            int testX = (int)Mathf.Round((blocks[i].transform.position.x - _xMin) * 2);
            int testY = (int)Mathf.Round((blocks[i].transform.position.y - _yMin) * 2);

            _tempRotation[i, 0] = testX;
            _tempRotation[i, 1] = testY;

            if (testY < 0 || testY >= BoardHeight)
                return false;
            //If the piece would move out of the b ounds of the board, adjust it!
            else if (testX < 0)
            {
                PentominoBehavior pent = blocks[i].GetComponentInParent<PentominoBehavior>();
                pent.transform.position = new Vector3(
                    pent.transform.position.x + 0.5f,
                    pent.transform.position.y,
                    0
                );
                return CheckRotation(blocks);
            }
            else if (testX >= BoardWidth)
            {
                PentominoBehavior pent = blocks[i].GetComponentInParent<PentominoBehavior>();
                pent.transform.position = new Vector3(
                    pent.transform.position.x - 0.5f,
                    pent.transform.position.y,
                    0
                );
                return CheckRotation(blocks);
            }
            else if (Board[testX, testY] != null)
            {
                if (Board[testX, testY].CompareTag("PowerUp"))
                {
                    _heldPowerUp = Board[testX, testY].GetComponent<PowerUpBehavior>();
                    _heldPowerUp.transform.SetPositionAndRotation(_powerPosition, Quaternion.identity);
                    Board[testX, testY] = null;
                    _audio.PlayOneShot(_pickUpAudio);
                    return true;
                }
                else
                    return false;
            }
        }
        return true;
    }

    //Update the translation to the board
    public void UpdateTranslation(int x, int y)
    {
        for (int i = 0; i < _currentBlocks.Length; i++)
        {
            CurrentPosition[i, 0] += x;
            CurrentPosition[i, 1] += y;

            CheckUpdate(i);
        }
    }

    //Update the rotation to the board
    public void UpdateRotation()
    {
        for(int i = 0; i < _currentBlocks.Length; i++)
        {
            CurrentPosition[i, 0] = _tempRotation[i, 0];
            CurrentPosition[i, 1] = _tempRotation[i, 1];

            CheckUpdate(i);
        }
    }

    //Double check that the translation happened appropriately
    void CheckUpdate(int i)
    {
        if (CurrentPosition[i, 0] != (int)Mathf.Round((_currentBlocks[i].transform.position.x - _xMin) * 2) ||
            CurrentPosition[i, 1] != (int)Mathf.Round((_currentBlocks[i].transform.position.y - _yMin) * 2))
        {
            _currentBlocks[i].transform.position = new Vector3((CurrentPosition[i, 0] / 2.0f) + _xMin, (CurrentPosition[i, 1] / 2.0f) + _yMin, 0);
        }
    }

    //Check if the shadow has moved appropriately
    public void CheckShadowUpdate(int yDepth, SpriteRenderer[] sprites)
    {
        for (int i = 0; i < _currentBlocks.Length; i++)
        {
            if (CurrentPosition[i, 0] != (int)Mathf.Round((sprites[i].transform.position.x - _xMin) * 2) ||
                CurrentPosition[i, 1] + yDepth != (int)Mathf.Round((sprites[i].transform.position.y - _yMin) * 2))
            {
                sprites[i].transform.position = new Vector3((CurrentPosition[i, 0] / 2.0f) + _xMin, ((CurrentPosition[i, 1] + yDepth) / 2.0f) + _yMin, 0);
            }
        }
    }

    //Hold the current pentomino
    public bool HoldPentomino(PentominoBehavior p, float xOffset, float yOffset)
    {
        //Can't hold if it's within the same turn
        if (holdTurn != GameBehavior.Instance.Turn)
        {
            holdTurn = GameBehavior.Instance.Turn;

            p.transform.SetPositionAndRotation(
                new Vector3(_holdPosition.x + xOffset, _holdPosition.y + yOffset, 0),
                Quaternion.identity
            );

            //If there is already a held piece, hot swap
            if (_heldPentomino != null)
            {
                _heldPentomino.transform.SetPositionAndRotation(SpawnPoint, Quaternion.identity);
                GameBehavior.Instance.CurrentPentomino = _heldPentomino;

                _heldPentomino.Reset();
                _heldPentomino.Shadow.IsHeld = false;
                _heldPentomino.Shadow.UpdateMovement();
                _heldPentomino.Ghost.Hold();
            }
            //Otherwise, spawn a new piece
            else
                GameBehavior.Instance.SpawnPentomino();

            _heldPentomino = p;

            return true;
        }
        return false;
    }

    //Lock in a piece to the board
    public void LockIn()
    {
        if (_currentBlocks.Length > 1)
        {
            for (int i = 0; i < _currentBlocks.Length; i++)
            {
                Board[CurrentPosition[i, 0], CurrentPosition[i, 1]] = _currentBlocks[i].gameObject;
            }
        }

        //Check if any rows are complete; if so, destroy them
        int rowsDestroyed = 0;
        for (int i = 0; i < BoardHeight; i++)
        {
            if (CheckRowFinished(i))
            {
                rowsDestroyed++;
                DestroyRow(i);
                i--;
            }
        }
        //If rows were destroyed, adjust score accordingly
        if(rowsDestroyed > 0)
        {
            _audio.PlayOneShot(_scoreAudio);
            _player.Score = rowsDestroyed;
            if (_player.Level >= _currentLevel)
                GameBehavior.Instance.LevelUp(_player.Level);
        }
        else
            _audio.PlayOneShot(_lockInAudio);

        if (CheckLost())
            EndGame();
        else
            GameBehavior.Instance.SpawnPentomino();
    }

    //Checks if a row has been completed
    bool CheckRowFinished(int y)
    {
        for(int x = 0; x < BoardWidth; x++)
        {
            if (Board[x, y] == null)
                return false;
        }
        return true;
    }

    //Checks if a row is completely empty
    public bool CheckRowEmpty(int y)
    {
        for (int x = 0; x < BoardWidth; x++)
        {
            if (Board[x, y] != null)
                return false;
        }
        return true;
    }

    //Destroys a row
    public void DestroyRow(int y, bool empty = false)
    {
        //Destroy row if it isn't empty
        if (!empty)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                Destroy(Board[x, y].gameObject);
            }
        }

        //Move all pieces above this row down by one
        for(int curY = y; curY < BoardHeight - 1; curY++)
        {
            for(int x = 0; x < BoardWidth; x++)
            {
                Board[x, curY] = Board[x, curY + 1];

                if(Board[x, curY] != null)
                    Board[x, curY].gameObject.transform.Translate(new Vector3(0, -0.5f, 0), Space.World);
            }
        }

        //Check if any pentominoes are empty; if so, destroy them
        int count = GameBehavior.Instance.ActivePentominoes.Count;
        for (int i = 0; i < count; i++)
        {
            PentominoBehavior p = GameBehavior.Instance.ActivePentominoes[i];
            if (p.CheckEmpty())
            {
                p.DestroyInstance();
                i--;
                count--;
            } 
        }
    }

    //Did we lose?
    bool CheckLost()
    {
        //Check if the piece is locked in above the board
        for (int i = 0; i < _currentBlocks.Length; i++)
        {
            if (CurrentPosition[i, 1] >= 20)
                return true;
        }
        return false;
    }

    //Clear the board
    public void ClearBoard()
    {
        for(int x = 0; x < BoardWidth; x++)
        {
            for(int y = 0; y < BoardHeight; y++)
            {
                if (Board[x, y] != null)
                    Destroy(Board[x, y].gameObject);
            }
        }
    }

    //End the game
    void EndGame()
    {
        GameBehavior.Instance.CurrentState = GameBehavior.GameState.GameOver;
    }
}
