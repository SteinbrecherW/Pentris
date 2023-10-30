using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehavior : MonoBehaviour
{
    //Singleton
    public static MenuBehavior Instance;

    //List of buttons to choose from (i.e. Start, Exit)
    [SerializeField] MenuButtonBehavior[] _buttons;
    //Index of the selected button
    int _selectedButton = 0;

    //Are we still choosing an option?
    public bool Selecting = true;

    //Audio parameters
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _moveAudio;
    [SerializeField] AudioClip _selectAudio;

    private void Awake()
    {
        //Singleton pattern
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //If we haven't made a decision yet...
        if (Selecting)
        {
            //If down is pressed, move down one option (or wrap to the top)
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _buttons[_selectedButton].IsSelected = false;
                _selectedButton = _selectedButton < _buttons.Length - 1 ? _selectedButton + 1 : 0;
                _buttons[_selectedButton].IsSelected = true;
                _audio.PlayOneShot(_moveAudio);
            }

            //If up is pressed, move up one option (or wrap to the bottom)
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _buttons[_selectedButton].IsSelected = false;
                _selectedButton = _selectedButton > 0 ? _selectedButton - 1 : _buttons.Length - 1;
                _buttons[_selectedButton].IsSelected = true;
                _audio.PlayOneShot(_moveAudio);
            }

            //When we select an option, load it
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _buttons[_selectedButton].LoadSelection();
                _audio.PlayOneShot(_selectAudio);
            }
        }
    }
}
