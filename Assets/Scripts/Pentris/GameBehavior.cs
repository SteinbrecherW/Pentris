using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameBehavior : MonoBehaviour
{
    //Singleton
    public static GameBehavior Instance;

    //State machine
    public enum GameState
    {
        Idle,
        Running,
        Paused,
        GameOver
    }

    //Manages the UI windows, updating them whenever the current game state changes
    GameState _currentState;
    public GameState CurrentState
    {
        get => _currentState;
        set
        {
            switch (value)
            {
                case GameState.Idle:
                    _startBox.enabled = true;
                    _gameOverBox.enabled = false;
                    _messageText.fontSize = 40;
                    _messageText.text = "<b>Starting level\n" + BoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
                    break;

                case GameState.Running:
                    _audio.Play();
                    _startBox.enabled = false;
                    _pauseBox.enabled = false;
                    _gameOverBox.enabled = false;
                    _leftArrow.enabled = false;
                    _rightArrow.enabled = false;
                    _scoreText.enabled = true;
                    _highScoreText.enabled = true;
                    _controlText.enabled = true;
                    _powerText.enabled = true;
                    _scoreBox.enabled = true;
                    _highScoreBox.enabled = true;
                    _controlBox.enabled = true;
                    _messageText.text = "";
                    break;

                case GameState.Paused:
                    _startBox.enabled = false;
                    _pauseBox.enabled = true;
                    _gameOverBox.enabled = false;
                    _leftArrow.enabled = false;
                    _rightArrow.enabled = false;
                    _scoreText.enabled = false;
                    _highScoreText.enabled = false;
                    _controlText.enabled = false;
                    _powerText.enabled = false;
                    _scoreBox.enabled = false;
                    _highScoreBox.enabled = false;
                    _controlBox.enabled = false;
                    _messageText.fontSize = 33;
                    _messageText.text = "<b>Paused</b>" +
                        "\n\nControls:" +
                        "\nLeft/Right Arrows: Move" +
                        "\nUp Arrow: Rotate Clockwise" +
                        "\nDown Arrow: Soft Drop" +
                        "\nSpace: Hard Drop" +
                        "\nLeft Shift: Hold" +
                        "\nE: Use Power Up" +
                        "\n\n<b>Esc to resume" +
                        "\nSpace to exit</b>";
                    break;

                case GameState.GameOver:
                    _audio.Stop();
                    _audio.PlayOneShot(_loseAudio);
                    _startBox.enabled = false;
                    _pauseBox.enabled = false;
                    _gameOverBox.enabled = true;
                    _leftArrow.enabled = false;
                    _rightArrow.enabled = false;
                    _scoreText.enabled = true;
                    _highScoreText.enabled = true;
                    _controlText.enabled = false;
                    _powerText.enabled = false;
                    _scoreBox.enabled = true;
                    _highScoreBox.enabled = true;
                    _controlBox.enabled = false;
                    _messageText.fontSize = 40;
                    _messageText.text = "<b>Game over\nFinal score: " +
                        (BoardBehavior.Instance.Player.Score - BoardBehavior.Instance.Player.ScoreOffset) +
                        "\n\nR to restart\nEsc to menu</b>";
                    break;

            }
            _currentState = value;
        }
    }

    public bool PowerUpsEnabled = true;

    //UI Controls
    [SerializeField] KeyCode _start = KeyCode.Space;
    [SerializeField] KeyCode _restart = KeyCode.R;
    [SerializeField] KeyCode _pause = KeyCode.Escape;

    //Index of the current and next pentominoes from their array
    int _currentPentomino;
    int _nextPentomino = -1;

    //The pentomino to appear in the "next" window
    GameObject _nextPent;

    GameObject _currentPowerUp = null;

    //Determines the delay after pressing a button before it gets "held"
    public float CursorInitialDelay = 0.1f;
    //Delay in between every cursor move when "held"
    public float CursorMoveDelay = 0.1f;

    public float ClockDuration = 10.0f;
    public float ClockSlowDownSpeed = 1.0f;

    //Available pentomino objects
    public GameObject[] Pentominoes;
    public GameObject[] PowerUps;

    //The currently active pentomino
    public PentominoBehavior CurrentPentomino;

    //Pentominoes currently active on the board (includes held pents)
    public List<PentominoBehavior> ActivePentominoes = new List<PentominoBehavior>();

    //Audio source
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _loseAudio;
    [SerializeField] AudioClip _selectAudio;

    //Number of blocks that have been dropped
    public int Turn = 0;

    //Text meshes for UI elements
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _highScoreText;
    [SerializeField] TextMeshProUGUI _controlText;
    [SerializeField] TextMeshProUGUI _powerText;
    [SerializeField] SpriteRenderer _startBox;
    [SerializeField] SpriteRenderer _pauseBox;
    [SerializeField] SpriteRenderer _gameOverBox;
    [SerializeField] SpriteRenderer _scoreBox;
    [SerializeField] SpriteRenderer _highScoreBox;
    [SerializeField] SpriteRenderer _controlBox;
    [SerializeField] SpriteRenderer _leftArrow;
    [SerializeField] SpriteRenderer _rightArrow;

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
        CurrentState = GameState.Idle;
        _audio = GetComponent<AudioSource>();
    }

    //State machine and menu controls
    void Update()
    {
        if (Input.GetKeyDown(_restart))
        {
            Restart();
        }
        //When starting the game, allows for choice of starting level
        if (_currentState == GameState.Idle)
        {
            if (Input.GetKeyDown(_start))
            {
                BoardBehavior.Instance.SetStartingLevel();
                CurrentState = GameState.Running;
                _audio.PlayOneShot(_selectAudio);
                ResetGame();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                BoardBehavior.Instance.StartingLevel =
                    BoardBehavior.Instance.StartingLevel == 1 ?
                    20 :
                    BoardBehavior.Instance.StartingLevel - 1
                ;
                _messageText.text = "<b>Starting level\n" + BoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
                _audio.PlayOneShot(_selectAudio);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BoardBehavior.Instance.StartingLevel =
                    BoardBehavior.Instance.StartingLevel == 20 ?
                    1 :
                    BoardBehavior.Instance.StartingLevel + 1
                ;
                _messageText.text = "<b>Starting level\n" + BoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
                _audio.PlayOneShot(_selectAudio);
            }
        }
        else
        {
            if (Input.GetKeyDown(_pause))
            {
                _audio.PlayOneShot(_selectAudio);
                if (CurrentState == GameState.GameOver)
                {
                    SceneManager.LoadScene("Title");
                }
                else
                {
                    CurrentState =
                        CurrentState == GameState.Running
                        ? GameState.Paused
                        : GameState.Running;
                }
            }

            if (Input.GetKeyDown(_restart))
            {
                _audio.PlayOneShot(_selectAudio);
                Restart();
            }

            if (CurrentState == GameState.Paused && Input.GetKeyDown(_start))
            {
                _audio.PlayOneShot(_selectAudio);
                SceneManager.LoadScene("Title");
            }
        }
    }

    //Resets the game (duh)
    void ResetGame()
    {
        foreach(PentominoBehavior t in ActivePentominoes)
            Destroy(t.gameObject);

        ActivePentominoes.Clear();

        BoardBehavior.Instance.ClearBoard();

        SpawnPentomino();

        Turn = 0;
    }


    //Spawns a pentomino
    public void SpawnPentomino()
    {
        //Spawns current active pentomino
        _currentPentomino = _nextPentomino == -1 ? Random.Range(0, Pentominoes.Length) : _nextPentomino;
        PentominoBehavior curPent = Pentominoes[_currentPentomino].GetComponent<PentominoBehavior>();
        GameObject pent = Instantiate(
            Pentominoes[_currentPentomino],
            new Vector3(
                BoardBehavior.Instance.SpawnPoint.x,
                BoardBehavior.Instance.SpawnPoint.y + curPent.YDepth,
                0
            ),
            Quaternion.identity,
            BoardBehavior.Instance.transform
        ) ;
        CurrentPentomino = pent.GetComponent<PentominoBehavior>();

        //Spawns next pentomino itno "next" area
        Destroy(_nextPent);
        _nextPentomino = Random.Range(0, Pentominoes.Length);
        PentominoBehavior nextType = Pentominoes[_nextPentomino].GetComponent<PentominoBehavior>();
        _nextPent = Instantiate(
            Pentominoes[_nextPentomino],
            new Vector3(
                BoardBehavior.Instance.NextPosition.x + nextType.XOffset,
                BoardBehavior.Instance.NextPosition.y + nextType.YOffset,
                0
            ),
            Quaternion.identity,
            BoardBehavior.Instance.transform
        ) ; 
        _nextPent.GetComponent<PentominoBehavior>().IsNext = true;

        //Creates a power up every 10 turns
        Turn++;
        if(Turn % 10 == 0)
        {
            SpawnPowerUp();
        }
        //If it's not picked up in one turn, it is destroyed
        else if(Turn % 10 == 1
            && _currentPowerUp != null
            && (BoardBehavior.Instance.HeldPowerUp == null
            || BoardBehavior.Instance.HeldPowerUp.gameObject != _currentPowerUp)
        )
        {
            RemovePowerUp();
        }
    }

    //Spawns a power up
    void SpawnPowerUp()
    {
        //Finds a valid empty space on the board
        int x, y;
        do
        {
            x = Random.Range(0, 12);
            y = Random.Range(0, 20);
        } while (BoardBehavior.Instance.Board[x, y] != null);

        //Chooses and spawns a power up
        int _powIndex = Random.Range(0, 4);
        _currentPowerUp = Instantiate(
            PowerUps[_powIndex],
            new Vector3(
                (x / 2.0f) + BoardBehavior.Instance.XMin,
                (y / 2.0f) + BoardBehavior.Instance.YMin,
                0
            ),
            Quaternion.identity,
            BoardBehavior.Instance.transform
        );

        BoardBehavior.Instance.Board[x, y] = _currentPowerUp;
        PowerUpBehavior pow = _currentPowerUp.GetComponent<PowerUpBehavior>();
        pow.PowerUpIndex = _powIndex;
        pow.XPos = x;
        pow.YPos = y;
    }

    //Destroys the power up if it isn't picked up
    void RemovePowerUp()
    {
        PowerUpBehavior pow = _currentPowerUp.GetComponent<PowerUpBehavior>();
        BoardBehavior.Instance.Board[pow.XPos, pow.YPos] = null;
        Destroy(_currentPowerUp);
    }

    //Changes the background color if leveled up
    public void LevelUp(int level)
    {
        if (level <= 20)
        {
            BackgroundColorBehavior.Instance.LevelUp(level);
        }
    }

    //Restarts the scene
    void Restart()
    {
        SceneManager.LoadScene("Pentris");
    }
}
