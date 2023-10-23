using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonBehavior : MonoBehaviour
{
    [SerializeField] string _scene;

    [SerializeField] SpriteRenderer _spriteR;

    [SerializeField] Sprite _selectedSprite;
    [SerializeField] Sprite _normalSprite;

    [SerializeField] bool _isQuit = false;

    bool _isSelected;
    public bool IsSelected {
        get => _isSelected;
        set
        {
            _spriteR.sprite = value ? _selectedSprite : _normalSprite;
            _isSelected = value;
        }
    }

    private void Start()
    {
        _spriteR = GetComponent<SpriteRenderer>();
    }

    public void LoadSelection()
    {
        MenuBehavior.Instance.Selecting = false;
        if (_isQuit)
            Application.Quit();
        else
            StartCoroutine(Selected());
    }

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
