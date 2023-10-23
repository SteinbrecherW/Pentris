using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorBehavior : MonoBehaviour
{
    public static BackgroundColorBehavior Instance;

    Camera _camera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

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
