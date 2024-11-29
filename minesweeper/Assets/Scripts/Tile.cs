using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum TileCategory
    {
        Empty,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Mine,
        // Å‰‚Ì‘€ì‚Å’n—‹‚ð“¥‚Ü‚È‚¢‚æ‚¤‚É‚·‚é‚½‚ß
        FirstTimeEmpty
    }

    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Sprite _tileSprite;
    [SerializeField]
    private TMPro.TextMeshProUGUI _textMeshProUGUI;

    public TileCategory TileType { get; set; } = TileCategory.FirstTimeEmpty;

    void Start()
    {
         _button.onClick.AddListener(() => 
         {
             FindAnyObjectByType<TileController>().OpenTile(GetComponent<Tile>());
             Destroy(_button);
             _image.sprite = _tileSprite;
         });
    }

    public void ChangeTileType(TileCategory tileType)
    {
        _textMeshProUGUI.text = ((int)tileType).ToString();
    }
}
