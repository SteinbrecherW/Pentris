using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehavior : MonoBehaviour
{
    public KeyCode MoveLeft = KeyCode.LeftArrow;
    public KeyCode MoveRight = KeyCode.RightArrow;
    public KeyCode MoveDown = KeyCode.DownArrow;
    public KeyCode RotateClockwise = KeyCode.UpArrow;
    public KeyCode HardDrop = KeyCode.Space;
    public KeyCode Hold = KeyCode.LeftShift;

    int _score = 0;
    public int ScoreOffset = 0;
    public int Score
    {
        get => _score;
        set
        {
            switch (value)
            {
                case 0:
                    break;

                case 1:
                    _score += 100;
                    break;

                case 2:
                    _score += 250;
                    break;

                case 3:
                    _score += 500;
                    break;

                case 4:
                    _score += 1000;
                    break;

                case 5:
                    _score += 2000;
                    break;

                default:
                    _score += value;
                    break;
            }

            _text.text = "Score: " + (_score - ScoreOffset) + "\nLevel: " + Level;
        }
    }

    public int Level
    {
        get => _score < 20000 ? (_score / 1000) + 1 : 20;
    }

    [SerializeField] TextMeshProUGUI _text;
}
