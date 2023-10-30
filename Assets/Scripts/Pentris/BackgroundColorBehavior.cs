using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorBehavior : MonoBehaviour
{
    //Singleton
    public static BackgroundColorBehavior Instance;

    //Current camera
    Camera _camera;

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
        _camera = GetComponent<Camera>();
    }

    //When the player levels up, change the background color
    public void LevelUp(int level)
    {
        Color newColor;
        if(level <= 10)
            newColor = new Color(0.1f * (level - 1.0f), 1.0f, 0.4f, 1.0f);
        else
            newColor = new Color(1.0f, 1.0f - (0.1f * (level - 10)), 0.4f, 1.0f);

        _camera.backgroundColor = newColor;
    }
}
