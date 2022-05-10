using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetramino : MonoBehaviour
{
    [SerializeField]
    Mino minoPrefab;
    
    bool[,] minoExists;
    Mino[] minos;
    float scale;
    float tileSize = 1f;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void InitAsT(float scale)
    {
        this.scale = scale;
        minoExists = new bool[3, 3];

        for(int i = 0; i < minoExists.GetLength(0); ++i)
        {
            for(int j = 0; j < minoExists.GetLength(1); ++j)
            {
                minoExists[i, j] = false;
            }
        }

        minoExists[0, 1] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        minos = new Mino[4];

        for (int i = 0; i < 4; ++i)
        {
            Mino mino = Instantiate(minoPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            mino.MinoColor = Color.red;

            minos[i] = mino;
        }

        SetMinoPositions(new Vector2(-tileSize, tileSize));

        transform.localScale = new Vector3(scale, scale, 1);
    }

    void SetMinoPositions(Vector2 topLeftTileCenter)
    {
        int minoIndex = 0;
        for (int i = 0; i < minoExists.GetLength(0) && minoIndex < 4; ++i)
        {
            for (int j = 0; j < minoExists.GetLength(1) && minoIndex < 4; ++j)
            {
                if(minoExists[i, j])
                {
                    minos[minoIndex].transform.localPosition = new Vector3(topLeftTileCenter.x + j * tileSize, topLeftTileCenter.y - i * tileSize, 0);
                    ++minoIndex;
                }
            }
        }
    }
}
