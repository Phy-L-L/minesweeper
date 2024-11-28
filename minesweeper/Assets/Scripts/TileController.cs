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
        // �ŏ��̑���Œn���𓥂܂Ȃ��悤�ɂ��邽�߂̎���
        if(_gameManager.FirstTime)
        {
            SetupLandmine();
        }

        LandmineDecision(isLandmine);
    }

    private void LandmineDecision(bool isLandmine)
    {
        // �ċA����


        // �n������
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
