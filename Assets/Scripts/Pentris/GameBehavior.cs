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
                    _messageText.fontSize = 30;
                    _messageText.text = "<b>Starting level\n" + BoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
                    break;

                case GameState.Running:
                    _startBox.enabled = false;
                    _pauseBox.enabled = false;
                    _gameOverBox.enabled = false;
                    _leftArrow.enabled = false;
                    _rightArrow.enabled = false;
                    _scoreText.enabled = true;
                    _controlText.enabled = true;
                    _scoreBox.enabled = true;
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
                    _controlText.enabled = false;
                    _scoreBox.enabled = false;
                    _controlBox.enabled = false;
                    _messageText.fontSize = 25;
                    _messageText.text = "<b>Paused</b>" +
                        "\n\nControls:" +
                        "\nLeft/Right Arrows: Move" +
                        "\nUp Arrow: Rotate Clockwise" +
                        "\nDown Arrow: Soft Drop" +
                        "\nSpace: Hard Drop" +
                        "\nLeft Shift: Hold" +
                        "\n\n<b>Esc to resume" +
                        "\nSpace to exit</b>";
                    break;

                case GameState.GameOver:
                    _startBox.enabled = false;
                    _pauseBox.enabled = false;
                    _gameOverBox.enabled = true;
                    _leftArrow.enabled = false;
                    _rightArrow.enabled = false;
                    _scoreText.enabled = true;
                    _controlText.enabled = false;
                    _scoreBox.enabled = true;
                    _controlBox.enabled = false;
                    _messageText.fontSize = 30;
                    _messageText.text = "<b>Game over\nFinal score: " +
                        (BoardBehavior.Instance.Player.Score - BoardBehavior.Instance.Player.ScoreOffset) +
                        "\n\nR to restart\nEsc to menu</b>";
                    break;

            }
            _currentState = value;
        }
    }

    //UI Controls
    [SerializeField] KeyCode _start = KeyCode.Space;
    [SerializeField] KeyCode _restart = KeyCode.R;
    [SerializeField] KeyCode _pause = KeyCode.Escape;

    //Index of the current and next pentominoes from their array
    int _currentPentomino;
    int _nextPentomino = -1;

    //The pentomino to appear in the "next" window
    GameObject _nextPent;

    //Determines the delay after pressing a button before it gets "held"
    public float CursorInitialDelay = 0.1f;
    //Delay in between every cursor move when "held"
    public float CursorMoveDelay = 0.1f;

    //Available pentomino objects
    public GameObject[] Pentominoes;

    //Pentominoes currently active on the board (includes held pents)
    public List<PentominoBehavior> ActivePentominoes = new List<PentominoBehavior>();

    //Number of blocks that have been dropped
    public int Turn;

    //Text meshes for UI elements
    [SerializeField] TextMeshProUGUI _messageText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _controlText;
    [SerializeField] SpriteRenderer _startBox;
    [SerializeField] SpriteRenderer _pauseBox;
    [SerializeField] SpriteRenderer _gameOverBox;
    [SerializeField] SpriteRenderer _scoreBox;
    [SerializeField] SpriteRenderer _controlBox;
    [SerializeField] SpriteRenderer _leftArrow;
    [SerializeField] SpriteRenderer _rightArrow;

    //Position where "next" pentomino should appear
    Vector3 _nextPosition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        CurrentState = GameState.Idle;
        _nextPosition = BoardBehavior.Instance.NextPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(_restart))
        {
            Restart();
        }
        if (_currentState == GameState.Idle)
        {
            if (Input.GetKeyDown(_start))
            {
                BoardBehavior.Instance.SetStartingLevel();
                CurrentState = GameState.Running;
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
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                BoardBehavior.Instance.StartingLevel =
                    BoardBehavior.Instance.StartingLevel == 20 ?
                    1 :
                    BoardBehavior.Instance.StartingLevel + 1
                ;
                _messageText.text = "<b>Starting level\n" + BoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
            }
        }
        else
        {
            if (Input.GetKeyDown(_pause))
            {
                if(CurrentState == GameState.GameOver)
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
                Restart();
            }

            if (CurrentState == GameState.Paused && Input.GetKeyDown(_start))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    void ResetGame()
    {
        foreach(PentominoBehavior t in ActivePentominoes)
            Destroy(t.gameObject);

        ActivePentominoes.Clear();

        BoardBehavior.Instance.ClearBoard();

        SpawnPentomino();

        Turn = 0;
    }

    public void SpawnPentomino()
    {
        _currentPentomino = _nextPentomino == -1 ? Random.Range(0, Pentominoes.Length) : _nextPentomino;
        GameObject pent = Instantiate(
            Pentominoes[_currentPentomino],
            BoardBehavior.Instance.SpawnPoint,
            Quaternion.identity,
            BoardBehavior.Instance.transform
        ) ;

        Destroy(_nextPent);
        _nextPentomino = Random.Range(0, Pentominoes.Length);
        PentominoBehavior nextType = Pentominoes[_nextPentomino].GetComponent<PentominoBehavior>();
        _nextPent = Instantiate(
            Pentominoes[_nextPentomino],
            new Vector3(
                _nextPosition.x + nextType.XOffset,
                _nextPosition.y + nextType.YOffset,
                0
            ),
            Quaternion.identity,
            BoardBehavior.Instance.transform
        ) ; 
        _nextPent.GetComponent<PentominoBehavior>().IsNext = true;
        Turn++;
    }

    public void LevelUp(int level)
    {
        if (level <= 20)
        {
            BackgroundColorBehavior.Instance.LevelUp(level);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene("Pentris");
    }
}
