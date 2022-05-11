using System;
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

    [SerializeField]
    GameObject gameOverText;

    Vector2 topLeft = new Vector2(-2.0f, 5.75f);
    float tileSize = 1f;

    float posOffset;

    int nCols = 10;
    int nRows = 22; // 1st and 2nd row are the buffer rows where the tetraminos will be initially created

    int startRow = 0;
    int startCol = 4;

    Mino[,] board;
    bool[,] minoExists;
    Tetramino curPiece;

    int qIndex;
    int[] instantiateQueue;

    bool gameOver = false;

    void Start()
    {
        board = new Mino[nRows, nCols];
        minoExists = new bool[nRows, nCols];
        posOffset = tileSize * tileScale;

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
                minoExists[i, j] = false;
            }
            y -= posOffset;
        }

        qIndex = 7;
        instantiateQueue = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

        InstantiateTetraMino();
    }

    
    void Update()
    {
        if (!gameOver)
        {
            int[,] storeIndices;
            if (curPiece && curPiece.Store(out storeIndices))
            {
                // Row corresponding to the smallest index where the current piece was stored
                int storeRow = 22;
                for (int i = 0; i < storeIndices.GetLength(0); ++i)
                {
                    int row = storeIndices[i, 0];
                    int col = storeIndices[i, 1];
                    minoExists[row, col] = true;
                    board[row, col].MinoColor = curPiece.TetraMinoColor;

                    if (row < storeRow)
                        storeRow = row;
                }

                curPiece.gameObject.SetActive(false);
                Destroy(curPiece);

                gameOver = storeRow <= 1;

                if (!gameOver)
                {
                    CheckRowComplete(storeRow);
                    InstantiateTetraMino();
                }
                else
                {
                    gameOverText.SetActive(true);
                }
            }
        }
    }

    void CheckRowComplete(int storeRow)
    {
        for(int row = storeRow; row < minoExists.GetLength(0); ++row)
        {
            bool rowIsComplete = true;
            for(int col = 0; col < minoExists.GetLength(1); ++col)
            {
                rowIsComplete &= minoExists[row, col];
            }

            if (rowIsComplete)
            {
                // clear row
                for(int col = 0; col < minoExists.GetLength(1); ++col)
                {
                    minoExists[row, col] = false;
                    board[row, col].MinoColor = Color.black;
                }

                // drop minos above this row down

                for(int r = row; r > 0; --r)
                {
                    for(int c = 0; c < minoExists.GetLength(1); ++c)
                    {
                        minoExists[r, c] = minoExists[r - 1, c];
                        board[r, c].MinoColor = board[r - 1, c].MinoColor;
                    }
                }
            }
        }
    }

    void InstantiateTetraMino()
    {
        if(qIndex >= instantiateQueue.Length)
        {
            System.Random rng = new System.Random();
            int n = instantiateQueue.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                int temp = instantiateQueue[n];
                instantiateQueue[n] = instantiateQueue[k];
                instantiateQueue[k] = temp;
            }
            qIndex = 0;
        }

        curPiece = Instantiate(tetraminoPrefab, new Vector3(topLeft.x + startCol * posOffset, topLeft.y - startRow * posOffset, 0), Quaternion.identity);

        switch (instantiateQueue[qIndex])
        {
            case 0:
                curPiece.InitAsO(tileScale, startRow, startCol, minoExists);
                break;
            case 1:
                curPiece.InitAsI(tileScale, startRow, startCol, minoExists);
                break;
            case 2:
                curPiece.InitAsT(tileScale, startRow, startCol, minoExists);
                break;
            case 3:
                curPiece.InitAsL(tileScale, startRow, startCol, minoExists);
                break;
            case 4:
                curPiece.InitAsJ(tileScale, startRow, startCol, minoExists);
                break;
            case 5:
                curPiece.InitAsS(tileScale, startRow, startCol, minoExists);
                break;
            case 6:
                curPiece.InitAsZ(tileScale, startRow, startCol, minoExists);
                break;
        }

        qIndex++;
    }
}
