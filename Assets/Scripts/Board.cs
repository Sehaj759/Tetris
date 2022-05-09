using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    Mino minoPrefab;

    [SerializeField]
    float tileScale;

    Vector2 topLeft = new Vector2(-2.5f, 6.25f);
    float tileSize = 1f;

    int nCols = 10;
    int nRows = 22; // 1st and 2nd row are the buffer rows where the tetraminos will be initially created

    Mino[,] board;
    void Start()
    {
        board = new Mino[nRows, nCols];

        float posChange = tileSize * tileScale;

        float y = topLeft.y - tileSize / 2;
        for(int i = 0; i < nRows; ++i)
        {
            float x = topLeft.x + tileSize / 2;
            for(int j = 0; j < nCols; ++j)
            {
                Mino mino = Instantiate(minoPrefab, new Vector3(x, y, 0), Quaternion.identity);
                mino.MinoColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                mino.SetScale(tileScale);
                x += posChange;

                board[i, j] = mino;
            }
            y -= posChange;
        }
    }

    
    void Update()
    {
        
    }
}
