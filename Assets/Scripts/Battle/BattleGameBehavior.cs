using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleGameBehavior : MonoBehaviour
{
    ////Singleton
    //public static BattleGameBehavior Instance;

    ////State machine
    //public enum GameState
    //{
    //    Idle,
    //    Running,
    //    Paused,
    //    GameOver
    //}

    ////Manages the UI windows, updating them whenever the current game state changes
    //GameState _currentState;
    //public GameState CurrentState
    //{
    //    get => _currentState;
    //    set
    //    {
    //        switch (value)
    //        {
    //            case GameState.Idle:
    //                _startBox.enabled = true;
    //                _gameOverBox.enabled = false;
    //                _messageText.fontSize = 40;
    //                _messageText.text = "<b>Starting level\n" + BattleBoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
    //                break;

    //            case GameState.Running:
    //                _startBox.enabled = false;
    //                _pauseBox.enabled = false;
    //                _gameOverBox.enabled = false;
    //                _leftArrow.enabled = false;
    //                _rightArrow.enabled = false;
    //                _controlText.enabled = true;
    //                _powerText.enabled = true;
    //                _controlBox.enabled = true;
    //                _messageText.text = "";
    //                break;

    //            case GameState.Paused:
    //                _startBox.enabled = false;
    //                _pauseBox.enabled = true;
    //                _gameOverBox.enabled = false;
    //                _leftArrow.enabled = false;
    //                _rightArrow.enabled = false;
    //                _controlText.enabled = false;
    //                _powerText.enabled = false;
    //                _controlBox.enabled = false;
    //                _messageText.fontSize = 35;
    //                _messageText.text = "<b>Paused</b>" +
    //                    "\n\nControls:" +
    //                    "\nLeft/Right Arrows: Move" +
    //                    "\nUp Arrow: Rotate Clockwise" +
    //                    "\nDown Arrow: Soft Drop" +
    //                    "\nSpace: Hard Drop" +
    //                    "\nLeft Shift: Hold" +
    //                    "\n\n<b>Esc to resume" +
    //                    "\nSpace to exit</b>";
    //                break;

    //            case GameState.GameOver:
    //                _startBox.enabled = false;
    //                _pauseBox.enabled = false;
    //                _gameOverBox.enabled = true;
    //                _leftArrow.enabled = false;
    //                _rightArrow.enabled = false;
    //                _controlText.enabled = false;
    //                _powerText.enabled = false;
    //                _controlBox.enabled = false;
    //                _messageText.fontSize = 40;
    //                _messageText.text = "<b>Game over\n\nR to restart\nEsc to menu</b>";
    //                break;

    //        }
    //        _currentState = value;
    //    }
    //}

    //public bool PowerUpsEnabled = true;

    ////UI Controls
    //[SerializeField] KeyCode _start = KeyCode.Space;
    //[SerializeField] KeyCode _restart = KeyCode.R;
    //[SerializeField] KeyCode _pause = KeyCode.Escape;

    ////Index of the current and next pentominoes from their array
    //int _currentPentomino;
    //int _nextPentomino = -1;

    ////The pentomino to appear in the "next" window
    //GameObject _nextPent;

    //GameObject _currentPowerUp = null;

    ////Determines the delay after pressing a button before it gets "held"
    //public float CursorInitialDelay = 0.1f;
    ////Delay in between every cursor move when "held"
    //public float CursorMoveDelay = 0.1f;

    //public float ClockDuration = 10.0f;
    //public float ClockSlowDownSpeed = 1.0f;

    ////Available pentomino objects
    //public GameObject[] Pentominoes;
    //public GameObject[] PowerUps;

    ////The currently active pentomino
    //public PentominoBehavior CurrentPentomino;

    ////Pentominoes currently active on the board (includes held pents)
    //public List<PentominoBehavior> ActivePentominoes = new List<PentominoBehavior>();

    ////Number of blocks that have been dropped
    //public int Turn = 0;

    ////Text meshes for UI elements
    //[SerializeField] TextMeshProUGUI _messageText;
    //[SerializeField] TextMeshProUGUI _scoreText;
    //[SerializeField] TextMeshProUGUI _highScoreText;
    //[SerializeField] TextMeshProUGUI _controlText;
    //[SerializeField] TextMeshProUGUI _powerText;
    //[SerializeField] SpriteRenderer _startBox;
    //[SerializeField] SpriteRenderer _pauseBox;
    //[SerializeField] SpriteRenderer _gameOverBox;
    //[SerializeField] SpriteRenderer _scoreBox;
    //[SerializeField] SpriteRenderer _highScoreBox;
    //[SerializeField] SpriteRenderer _controlBox;
    //[SerializeField] SpriteRenderer _leftArrow;
    //[SerializeField] SpriteRenderer _rightArrow;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //        Destroy(this);
    //    else
    //        Instance = this;
    //}

    //void Start()
    //{
    //    CurrentState = GameState.Idle;
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(_restart))
    //    {
    //        Restart();
    //    }
    //    if (_currentState == GameState.Idle)
    //    {
    //        if (Input.GetKeyDown(_start))
    //        {
    //            BattleBoardBehavior.Player1Board.SetStartingLevel();
    //            BattleBoardBehavior.Player2Board.SetStartingLevel();
    //            CurrentState = GameState.Running;
    //            ResetGame();
    //        }
    //        if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        {
    //            BattleBoardBehavior.Instance.StartingLevel =
    //                BattleBoardBehavior.Instance.StartingLevel == 1 ?
    //                20 :
    //                BattleBoardBehavior.Instance.StartingLevel - 1
    //            ;
    //            _messageText.text = "<b>Starting level\n" + BattleBoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
    //        }
    //        if (Input.GetKeyDown(KeyCode.RightArrow))
    //        {
    //            BattleBoardBehavior.Instance.StartingLevel =
    //                BattleBoardBehavior.Instance.StartingLevel == 20 ?
    //                1 :
    //                BattleBoardBehavior.Instance.StartingLevel + 1
    //            ;
    //            _messageText.text = "<b>Starting level\n" + BattleBoardBehavior.Instance.StartingLevel + "\n\nSpace to begin</b>";
    //        }
    //    }
    //    else
    //    {
    //        if (Input.GetKeyDown(_pause))
    //        {
    //            if(CurrentState == GameState.GameOver)
    //            {
    //                SceneManager.LoadScene("Title");
    //            }
    //            else
    //            {
    //                CurrentState =
    //                    CurrentState == GameState.Running
    //                    ? GameState.Paused
    //                    : GameState.Running;
    //            }
    //        }

    //        if (Input.GetKeyDown(_restart))
    //        {
    //            Restart();
    //        }

    //        if (CurrentState == GameState.Paused && Input.GetKeyDown(_start))
    //        {
    //            SceneManager.LoadScene("Title");
    //        }
    //    }
    //}

    //void ResetGame()
    //{
    //    foreach(PentominoBehavior t in ActivePentominoes)
    //        Destroy(t.gameObject);

    //    ActivePentominoes.Clear();

    //    BattleBoardBehavior.Player1Board.ClearBoard();
    //    BattleBoardBehavior.Player2Board.ClearBoard();

    //    SpawnPentomino();

    //    Turn = 0;
    //}

    //public void SpawnPentomino()
    //{
    //    _currentPentomino = _nextPentomino == -1 ? Random.Range(0, Pentominoes.Length) : _nextPentomino;
    //    PentominoBehavior curPent = Pentominoes[_currentPentomino].GetComponent<PentominoBehavior>();
    //    GameObject pent = Instantiate(
    //        Pentominoes[_currentPentomino],
    //        new Vector3(
    //            BattleBoardBehavior.Instance.SpawnPoint.x,
    //            BattleBoardBehavior.Instance.SpawnPoint.y + curPent.YDepth,
    //            0
    //        ),
    //        Quaternion.identity,
    //        BattleBoardBehavior.Instance.transform
    //    ) ;
    //    CurrentPentomino = pent.GetComponent<PentominoBehavior>();

    //    Destroy(_nextPent);
    //    _nextPentomino = Random.Range(0, Pentominoes.Length);
    //    PentominoBehavior nextType = Pentominoes[_nextPentomino].GetComponent<PentominoBehavior>();
    //    _nextPent = Instantiate(
    //        Pentominoes[_nextPentomino],
    //        new Vector3(
    //            BattleBoardBehavior.Instance.NextPosition.x + nextType.XOffset,
    //            BattleBoardBehavior.Instance.NextPosition.y + nextType.YOffset,
    //            0
    //        ),
    //        Quaternion.identity,
    //        BattleBoardBehavior.Instance.transform
    //    ) ; 
    //    _nextPent.GetComponent<PentominoBehavior>().IsNext = true;

    //    Turn++;
    //    if(Turn % 10 == 0)
    //    {
    //        SpawnPowerUp();
    //    }
    //    else if(Turn % 10 == 1
    //        && _currentPowerUp != null
    //        && (BattleBoardBehavior.Instance.HeldPowerUp == null
    //        || BattleBoardBehavior.Instance.HeldPowerUp.gameObject != _currentPowerUp)
    //    )
    //    {
    //        RemovePowerUp();
    //    }
    //}

    //void SpawnPowerUp()
    //{
    //    int x, y;
    //    do
    //    {
    //        x = Random.Range(0, 12);
    //        y = Random.Range(0, 20);
    //    } while (BattleBoardBehavior.Instance.Board[x, y] != null);

    //    int _powIndex = Random.Range(0, 4);
    //    _currentPowerUp = Instantiate(
    //        PowerUps[_powIndex],
    //        new Vector3(
    //            (x / 2.0f) + BattleBoardBehavior.Instance.XMin,
    //            (y / 2.0f) + BattleBoardBehavior.Instance.YMin,
    //            0
    //        ),
    //        Quaternion.identity,
    //        BattleBoardBehavior.Instance.transform
    //    );

    //    BattleBoardBehavior.Instance.Board[x, y] = _currentPowerUp;
    //    PowerUpBehavior pow = _currentPowerUp.GetComponent<PowerUpBehavior>();
    //    pow.PowerUpIndex = _powIndex;
    //    pow.XPos = x;
    //    pow.YPos = y;
    //}

    //void RemovePowerUp()
    //{
    //    PowerUpBehavior pow = _currentPowerUp.GetComponent<PowerUpBehavior>();
    //    BattleBoardBehavior.Instance.Board[pow.XPos, pow.YPos] = null;
    //    Destroy(_currentPowerUp);
    //}

    //public void LevelUp(int level)
    //{
    //    if (level <= 20)
    //    {
    //        BackgroundColorBehavior.Instance.LevelUp(level);
    //    }
    //}

    //void Restart()
    //{
    //    SceneManager.LoadScene("Pentris");
    //}
}
