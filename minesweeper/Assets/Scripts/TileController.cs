using System.Linq;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private TileGenerator _tileGenerator;

    public void OnClick(bool isLandmine)
    {
        // Å‰‚Ì‘€ì‚Å’n—‹‚ğ“¥‚Ü‚È‚¢‚æ‚¤‚É‚·‚é‚½‚ß‚ÌÀ‘•
        if(_gameManager.FirstTime)
        {
            SetupLandmine();
        }

        LandmineDecision(isLandmine);
    }

    private void LandmineDecision(bool isLandmine)
    {
        // Ä‹Aˆ—


        // ’n—‹ˆ—
        if (isLandmine)
        {
            Debug.Log("GameOver");
            return;
        }

        Debug.Log("Safe");
    }

    private void ChangeSafeTile()
    {

    }


    private void SetupLandmine()
    {
        _gameManager.FirstTime = false;
        _gameManager.IsGameOver = false;
        int landmineCount = _gameManager.LandmineCount;

        while (true)
        {
            int element = Random.Range(0, _tileGenerator.Tiles.Count);
            var tile = _tileGenerator.Tiles.ElementAt(element);
            if (!tile.IsLandmine)
            {
                tile.IsLandmine = true;
                --landmineCount;
            }

            if (landmineCount <= 0)
            {
                return;
            }
        }
    }
}
