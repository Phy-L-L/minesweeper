using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool IsLandmine{ get; set; }
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private Sprite _sprite;

    void Start()
    {
         _button.onClick.AddListener(() => 
         {
             FindAnyObjectByType<TileController>().OnClick(IsLandmine);
             Destroy(_button);
             _image.sprite = _sprite;
         });
    }
}
