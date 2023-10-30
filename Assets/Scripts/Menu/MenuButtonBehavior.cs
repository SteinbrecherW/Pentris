using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonBehavior : MonoBehaviour
{
    //Scene to load from this button
    [SerializeField] string _scene;

    //This object's sprite renderer;
    [SerializeField] SpriteRenderer _spriteR;

    //Sprites for selected and deselected
    [SerializeField] Sprite _selectedSprite;
    [SerializeField] Sprite _normalSprite;

    //Is this the "quit" button?
    [SerializeField] bool _isQuit = false;

    //Is this button currently highlighted?
    bool _isSelected;
    public bool IsSelected {
        get => _isSelected;
        set
        {
            //Changes the sprite any time selected status changes
            _spriteR.sprite = value ? _selectedSprite : _normalSprite;
            _isSelected = value;
        }
    }

    private void Start()
    {
        _spriteR = GetComponent<SpriteRenderer>();
    }

    //What to do when a button is selected
    public void LoadSelection()
    {
        //No longer selecting
        MenuBehavior.Instance.Selecting = false;

        //Quit the game if this is the "quit" button, otherwise load the desired scene
        if (_isQuit)
            Application.Quit();
        else
            StartCoroutine(Selected());
    }

    //When a button is selected, animate the sprite
    IEnumerator Selected()
    {
        for(int i = 0; i < 4; i++)
        {
            _spriteR.sprite = _normalSprite;
            yield return new WaitForSeconds(0.07f);
            _spriteR.sprite = _selectedSprite;
            yield return new WaitForSeconds(0.07f);
        }
        SceneManager.LoadScene(_scene);
    }
}
