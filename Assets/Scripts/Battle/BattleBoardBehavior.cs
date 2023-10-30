using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBoardBehavior : MonoBehaviour
{
    //public static BattleBoardBehavior Player1Board;
    //public static BattleBoardBehavior Player2Board;

    //[SerializeField] BattlePlayerBehavior _player;
    //public BattlePlayerBehavior Player { get => _player; }

    //public GameObject[,] Board;

    //GameObject[] _currentBlocks = new GameObject[5];

    //public Vector3 SpawnPoint = new Vector3(0.25f, 6, 0);

    //PentominoBehavior _heldPentomino = null;
    //public PentominoBehavior HeldPentomino { get => _heldPentomino; }
    //[SerializeField] Vector3 _holdPosition = new Vector3(-5, 5, 0);
    //int holdTurn = -1;

    //PowerUpBehavior _heldPowerUp = new PowerUpBehavior();
    //public PowerUpBehavior HeldPowerUp { get => _heldPowerUp; }
    //[SerializeField] Vector3 _powerPosition = new Vector3(-5, 5, 0);

    //public Vector3 NextPosition = new Vector3(-5, 4, 0);

    //public int BoardWidth = 12;
    //public int BoardHeight = 25;

    //[SerializeField] float _xMin = -2.75f;
    //public float XMin { get => _xMin; }

    //[SerializeField] float _yMin = -4.5f;
    //public float YMin { get => _yMin; }

    //[SerializeField] float[] _fallSpeedPerLevel = new float[20];
    //public float FallSpeed { get => _fallSpeedPerLevel[_currentLevel - 1] / 20; }

    //public int[,] CurrentPosition = new int[5, 2];
    //int[,] _tempRotation = new int[5, 2];

    //public int StartingLevel = 1;
    //int _currentLevel = 1;

    //private void Awake()
    //{
    //    if (Player1Board == null)
    //        Player1Board = this;
    //    else
    //        Player2Board = this;
    //}

    //void Start()
    //{
    //    Board = new GameObject[BoardWidth, BoardHeight];
    //    _player = GetComponent<BattlePlayerBehavior>();
    //}

    //public void SetStartingLevel()
    //{
    //    if (StartingLevel > 1)
    //    {
    //        _player.ScoreOffset = (StartingLevel - 1) * 1000;
    //        _player.Score = (StartingLevel - 1) * 1000;
    //        GameBehavior.Instance.LevelUp(StartingLevel);
    //        _currentLevel = StartingLevel;
    //        Debug.Log(FallSpeed);
    //    }
    //}

    //public void UpdateCurrentPosition(GameObject[] blocks)
    //{
    //    _currentBlocks = blocks;
    //    for(int i = 0; i < blocks.Length; i++)
    //    {
    //        CurrentPosition[i, 0] = (int) Mathf.Round((blocks[i].transform.position.x - _xMin) * 2);
    //        CurrentPosition[i, 1] = (int) Mathf.Round((blocks[i].transform.position.y - _yMin) * 2);
    //    }
    //}

    //public bool CheckTranslation(int x, int y, bool isShadow = false)
    //{
    //    for (int i = 0; i < _currentBlocks.Length; i++)
    //    {
    //        int testX = CurrentPosition[i, 0] + x;
    //        int testY = CurrentPosition[i, 1] + y;

    //        if (testY < 0 || testX < 0 || testX >= BoardWidth || testY >= BoardHeight)
    //            return false;
    //        else if (Board[testX, testY] != null)
    //        {
    //            if (Board[testX, testY].CompareTag("PowerUp"))
    //            {
    //                if (!isShadow)
    //                {
    //                    _heldPowerUp = Board[testX, testY].GetComponent<PowerUpBehavior>();
    //                    _heldPowerUp.transform.SetPositionAndRotation(_powerPosition, Quaternion.identity);
    //                    Board[testX, testY] = null;
    //                }
    //                return true;
    //            }
    //            else
    //                return false;
    //        }
    //    }
    //    return true;
    //}

    //public bool CheckRotation(BlockBehavior[] blocks)
    //{
    //    for (int i = 0; i < _currentBlocks.Length; i++)
    //    {
    //        int testX = (int)Mathf.Round((blocks[i].transform.position.x - _xMin) * 2);
    //        int testY = (int)Mathf.Round((blocks[i].transform.position.y - _yMin) * 2);

    //        _tempRotation[i, 0] = testX;
    //        _tempRotation[i, 1] = testY;

    //        if (testY < 0 || testY >= BoardHeight)
    //            return false;
    //        else if (testX < 0)
    //        {
    //            PentominoBehavior pent = blocks[i].GetComponentInParent<PentominoBehavior>();
    //            pent.transform.position = new Vector3(
    //                pent.transform.position.x + 0.5f,
    //                pent.transform.position.y,
    //                0
    //            );
    //            return CheckRotation(blocks);
    //        }
    //        else if (testX >= BoardWidth)
    //        {
    //            PentominoBehavior pent = blocks[i].GetComponentInParent<PentominoBehavior>();
    //            pent.transform.position = new Vector3(
    //                pent.transform.position.x - 0.5f,
    //                pent.transform.position.y,
    //                0
    //            );
    //            return CheckRotation(blocks);
    //        }
    //        else if (Board[testX, testY] != null)
    //        {
    //            if (Board[testX, testY].CompareTag("PowerUp"))
    //            {
    //                _heldPowerUp = Board[testX, testY].GetComponent<PowerUpBehavior>();
    //                _heldPowerUp.transform.SetPositionAndRotation(_powerPosition, Quaternion.identity);
    //                Board[testX, testY] = null;
    //                return true;
    //            }
    //            else
    //                return false;
    //        }
    //    }
    //    return true;
    //}

    //public void UpdateTranslation(int x, int y)
    //{
    //    for (int i = 0; i < _currentBlocks.Length; i++)
    //    {
    //        CurrentPosition[i, 0] += x;
    //        CurrentPosition[i, 1] += y;

    //        CheckUpdate(i);
    //    }
    //}

    //public void UpdateRotation()
    //{
    //    for(int i = 0; i < _currentBlocks.Length; i++)
    //    {
    //        CurrentPosition[i, 0] = _tempRotation[i, 0];
    //        CurrentPosition[i, 1] = _tempRotation[i, 1];

    //        CheckUpdate(i);
    //    }
    //}

    //void CheckUpdate(int i)
    //{
    //    if (CurrentPosition[i, 0] != (int)Mathf.Round((_currentBlocks[i].transform.position.x - _xMin) * 2) ||
    //        CurrentPosition[i, 1] != (int)Mathf.Round((_currentBlocks[i].transform.position.y - _yMin) * 2))
    //    {
    //        _currentBlocks[i].transform.position = new Vector3((CurrentPosition[i, 0] / 2.0f) + _xMin, (CurrentPosition[i, 1] / 2.0f) + _yMin, 0);
    //    }
    //}

    //public void CheckShadowUpdate(int yDepth, SpriteRenderer[] sprites)
    //{
    //    for (int i = 0; i < _currentBlocks.Length; i++)
    //    {
    //        if (CurrentPosition[i, 0] != (int)Mathf.Round((sprites[i].transform.position.x - _xMin) * 2) ||
    //            CurrentPosition[i, 1] + yDepth != (int)Mathf.Round((sprites[i].transform.position.y - _yMin) * 2))
    //        {
    //            sprites[i].transform.position = new Vector3((CurrentPosition[i, 0] / 2.0f) + _xMin, ((CurrentPosition[i, 1] + yDepth) / 2.0f) + _yMin, 0);
    //        }
    //    }
    //}

    //public bool HoldPentomino(PentominoBehavior p, float xOffset, float yOffset)
    //{
    //    if (holdTurn != GameBehavior.Instance.Turn)
    //    {
    //        holdTurn = GameBehavior.Instance.Turn;

    //        p.transform.SetPositionAndRotation(
    //            new Vector3(_holdPosition.x + xOffset, _holdPosition.y + yOffset, 0),
    //            Quaternion.identity
    //        );

    //        if (_heldPentomino != null)
    //        {
    //            _heldPentomino.transform.SetPositionAndRotation(SpawnPoint, Quaternion.identity);

    //            _heldPentomino.Reset();
    //            _heldPentomino.Shadow.IsHeld = false;
    //            _heldPentomino.Shadow.UpdateMovement();
    //            _heldPentomino.Ghost.Hold();
    //        }
    //        else
    //            GameBehavior.Instance.SpawnPentomino();

    //        _heldPentomino = p;
    //        return true;
    //    }
    //    return false;
    //}

    //public void LockIn()
    //{
    //    if (_currentBlocks.Length > 1)
    //    {
    //        for (int i = 0; i < _currentBlocks.Length; i++)
    //        {
    //            Board[CurrentPosition[i, 0], CurrentPosition[i, 1]] = _currentBlocks[i].gameObject;
    //        }
    //    }

    //    int rowsDestroyed = 0;
    //    for (int i = 0; i < BoardHeight; i++)
    //    {
    //        if (CheckRowFinished(i))
    //        {
    //            rowsDestroyed++;
    //            DestroyRow(i);
    //            i--;
    //        }
    //    }
    //    if(rowsDestroyed > 0)
    //    {
    //        _player.Score = rowsDestroyed;
    //        if (_player.Level >= _currentLevel)
    //            GameBehavior.Instance.LevelUp(_player.Level);
    //    }

    //    if (CheckLost())
    //        EndGame();
    //    else
    //        GameBehavior.Instance.SpawnPentomino();
    //}

    //bool CheckRowFinished(int y)
    //{
    //    for(int x = 0; x < BoardWidth; x++)
    //    {
    //        if (Board[x, y] == null)
    //            return false;
    //    }
    //    return true;
    //}

    //public bool CheckRowEmpty(int y)
    //{
    //    for (int x = 0; x < BoardWidth; x++)
    //    {
    //        if (Board[x, y] != null)
    //            return false;
    //    }
    //    return true;
    //}

    //public void DestroyRow(int y, bool empty = false)
    //{
    //    if (!empty)
    //    {
    //        for (int x = 0; x < BoardWidth; x++)
    //        {
    //            Destroy(Board[x, y].gameObject);
    //        }
    //    }

    //    for(int curY = y; curY < BoardHeight - 1; curY++)
    //    {
    //        for(int x = 0; x < BoardWidth; x++)
    //        {
    //            Board[x, curY] = Board[x, curY + 1];

    //            if(Board[x, curY] != null)
    //                Board[x, curY].gameObject.transform.Translate(new Vector3(0, -0.5f, 0), Space.World);
    //        }
    //    }

    //    int count = GameBehavior.Instance.ActivePentominoes.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        PentominoBehavior p = GameBehavior.Instance.ActivePentominoes[i];
    //        if (p.CheckEmpty())
    //        {
    //            p.DestroyInstance();
    //            i--;
    //            count--;
    //        } 
    //    }
    //}

    //bool CheckLost()
    //{
    //    for (int i = 0; i < _currentBlocks.Length; i++)
    //    {
    //        if (CurrentPosition[i, 1] >= 20)
    //            return true;
    //    }
    //    return false;
    //}

    //public void ClearBoard()
    //{
    //    for(int x = 0; x < BoardWidth; x++)
    //    {
    //        for(int y = 0; y < BoardHeight; y++)
    //        {
    //            if (Board[x, y] != null)
    //                Destroy(Board[x, y].gameObject);
    //        }
    //    }
    //}

    //void EndGame()
    //{
    //    GameBehavior.Instance.CurrentState = GameBehavior.GameState.GameOver;
    //}
}
