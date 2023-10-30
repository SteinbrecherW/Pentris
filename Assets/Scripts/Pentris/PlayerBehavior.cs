using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class PlayerBehavior : MonoBehaviour
{
    //Buttons for player movement
    public KeyCode MoveLeft = KeyCode.LeftArrow;
    public KeyCode MoveRight = KeyCode.RightArrow;
    public KeyCode MoveDown = KeyCode.DownArrow;
    public KeyCode RotateClockwise = KeyCode.UpArrow;
    public KeyCode HardDrop = KeyCode.Space;
    public KeyCode Hold = KeyCode.LeftShift;
    public KeyCode UsePowerUp = KeyCode.E;

    //Text meshes for score data
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _highScoreText;

    //Score parameters
    int _score = 0;
    int _highScore = 0;
    public int ScoreOffset = 0;
    public int Score
    {
        get => _score;
        set
        {
            //Add a specified score based on number of rows destroyed
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

                //Add the number to score if it isn't the number of rows destroyed
                default:
                    _score += value;
                    break;
            }

            //Update score text anytime score is modified
            _scoreText.text = "Score: " + (_score - ScoreOffset) + "\nLevel: " + Level;

            //If the current score is greater than the high score, current score gets saved as high score
            if(_score - ScoreOffset > _highScore)
            {
                _highScore = _score;
                _highScoreText.text = "\nHigh Score:\n" + _highScore;
                SaveHighScore();
            }
        }
    }

    //Levels go up to 20, based on intervals of 1000
    public int Level
    {
        get => _score < 20000 ? (_score / 1000) + 1 : 20;
    }

    private void Start()
    {
        Score = 0;
        LoadHighScore();
        _highScoreText.text = "\nHigh Score:\n" + _highScore;
    }

    //Saves high score value to a serial file
    public void SaveHighScore()
    {
        if (Directory.Exists(Application.dataPath + "/Save data/") == false)
            Directory.CreateDirectory(Application.dataPath + "/Save data/");

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/Save data/Score.secure");
        ScoreData data = new ScoreData();

        data.highscore = _highScore;

        bf.Serialize(file, data);
        file.Close();
    }

    //Loads high score from file
    public void LoadHighScore()
    {
        if (File.Exists(Application.dataPath + "/Save data/Score.secure"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Save data/Score.secure", FileMode.Open);
            ScoreData data = (ScoreData)bf.Deserialize(file);
            file.Close();

            _highScore = data.highscore;
        }
    }
}

//High score data
[Serializable]
class ScoreData
{
    public int highscore;
}
