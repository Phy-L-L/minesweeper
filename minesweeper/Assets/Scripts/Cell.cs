using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public enum CellCategory
    {
        // 最初の操作で地雷を踏まないようにするため
        FirstTimeEmpty = -1,
        Empty = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        // 旗
        Flag = 100,
        Mine = 101
    }

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Sprite _safeCellSprite;
    [SerializeField]
    private Sprite _flagCellSprite;
    [SerializeField]
    private TMPro.TextMeshProUGUI _textMeshProUGUI;

    public CellCategory CellType { get; set; } = CellCategory.FirstTimeEmpty;

    void Start()
    {
         _button.onClick.AddListener(OnClickCell);
    }

    private void OnClickCell()
    {
        FindAnyObjectByType<CellController>().OnClickCell(this);
        Destroy(_button);
        _image.sprite = _safeCellSprite;
    }

    public void ChangeCellType(CellCategory cellType)
    {
        // TODO : 旗や地雷、テキストカラーを変更する
        _textMeshProUGUI.text = cellType != CellCategory.Empty ? ((int)cellType).ToString() : string.Empty;
    }
}
