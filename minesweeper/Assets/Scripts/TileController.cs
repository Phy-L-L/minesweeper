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
        // 最初の操作で地雷を踏まないようにするための実装
        if(_gameManager.FirstTime)
        {
            SetupLandmine();
        }

        LandmineDecision(isLandmine);
    }

    private void LandmineDecision(bool isLandmine)
    {
        // 再帰処理


        // 地雷処理
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
