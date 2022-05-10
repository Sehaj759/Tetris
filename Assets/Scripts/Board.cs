using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    Mino minoPrefab;

    [SerializeField]
    Tetramino tetraminoPrefab;

    [SerializeField]
    float tileScale;

    Vector2 topLeft = new Vector2(-2.0f, 5.75f);
    float tileSize = 1f;

    int nCols = 10;
    int nRows = 22; // 1st and 2nd row are the buffer rows where the tetraminos will be initially created

    Mino[,] board;
    void Start()
    {
        board = new Mino[nRows, nCols];

        float posOffset = tileSize * tileScale;

        float y = topLeft.y;
        for(int i = 0; i < nRows; ++i)
        {
            float x = topLeft.x;
            for(int j = 0; j < nCols; ++j)
            {
                Mino mino = Instantiate(minoPrefab, new Vector3(x, y, 0), Quaternion.identity);
                mino.SetScale(tileScale);
                x += posOffset;

                board[i, j] = mino;
            }
            y -= posOffset;
        }

        int row = 3;
        int col = 1;
        Tetramino T = Instantiate(tetraminoPrefab, new Vector3(topLeft.x + col * posOffset, topLeft.y - row * posOffset, 0), Quaternion.identity);
        T.InitAsT(tileScale);
    }

    
    void Update()
    {
        
    }
}
