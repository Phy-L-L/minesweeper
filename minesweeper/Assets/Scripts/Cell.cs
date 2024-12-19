using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public enum CellCategory
    {
        Empty = -100,
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Mine = 100
    }

    private Dictionary<CellCategory, string> _colors = new()
    {
        { CellCategory.One, "#0000F7" },
        { CellCategory.Two, "#007C00" },
        { CellCategory.Three, "#EC1F1F" },
        { CellCategory.Four, "#00007C" },
        { CellCategory.Five, "#7C0000" },
        { CellCategory.Six, "#007C7C" },
        { CellCategory.Seven, "#000000" },
        { CellCategory.Eight, "#7C7C7C" }
    };

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Sprite _safeCellSprite;
    [SerializeField]
    private Sprite _mineCellSprite;
    [SerializeField]
    private TMPro.TextMeshProUGUI _textMeshProUGUI;

    private bool _isOpened = false;
    public bool IsOpened => _isOpened;

    public CellCategory CellType { get; set; } = CellCategory.Empty;

    void Start()
    {
         _button.onClick.AddListener(OnClickCell);
    }

    private void OnClickCell()
    {
        FindAnyObjectByType<CellController>().OnClickCell(this);
    }

    public void OpenCell(CellCategory cellType)
    {
        if (_isOpened) { return; }
        _isOpened = true;

        void changeColor(string colorCode)
        {
            _textMeshProUGUI.text = $"<color={colorCode}>{((int)cellType)}</color>";
        }
                
        switch (cellType)
        {
            case CellCategory.Zero :
                CellType = CellCategory.Empty;
                _image.sprite = _safeCellSprite;
                Destroy(_button);
                _textMeshProUGUI.text = string.Empty;
                break;
            case CellCategory.One :
            case CellCategory.Two :
            case CellCategory.Three :
            case CellCategory.Four :
            case CellCategory.Five :
            case CellCategory.Six :
            case CellCategory.Seven :
            case CellCategory.Eight :
                _image.sprite = _safeCellSprite;
                Destroy(_button);
                changeColor(_colors[cellType]);
                break;
            case CellCategory.Mine :
                _image.sprite = _mineCellSprite;
                Destroy(_button);
                _textMeshProUGUI.text = string.Empty;
                break;
        }
    }
}
