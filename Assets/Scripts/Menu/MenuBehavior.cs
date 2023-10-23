using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehavior : MonoBehaviour
{
    public static MenuBehavior Instance;

    [SerializeField] MenuButtonBehavior[] _buttons;
    int _selectedButton = 0;

    public bool Selecting = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Selecting)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _buttons[_selectedButton].IsSelected = false;
                _selectedButton = _selectedButton < _buttons.Length - 1 ? _selectedButton + 1 : 0;
                _buttons[_selectedButton].IsSelected = true;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _buttons[_selectedButton].IsSelected = false;
                _selectedButton = _selectedButton > 0 ? _selectedButton - 1 : _buttons.Length - 1;
                _buttons[_selectedButton].IsSelected = true;
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                _buttons[_selectedButton].LoadSelection();
        }
    }
}
