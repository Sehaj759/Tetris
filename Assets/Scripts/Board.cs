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

    Vector2 topLeft = new Vector2(-2.0f, 5.75f);
    float tileSize = 1f;

    float posOffset;

    int nCols = 10;
    int nRows = 22; // 1st and 2nd row are the buffer rows where the tetraminos will be initially created

    Mino[,] board;
    bool[,] minoExists;
    Tetramino curPiece;

    int qIndex;
    int[] instantiateQueue;

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
        int[,] storeIndices;
        if (curPiece && curPiece.Store(out storeIndices))
        {
            for(int i = 0; i < storeIndices.GetLength(0); ++i)
            {
                int row = storeIndices[i, 0];
                int col = storeIndices[i, 1];
                minoExists[row, col] = true;
                board[row, col].MinoColor = curPiece.TetraMinoColor;
            }

            curPiece.gameObject.SetActive(false);
            Destroy(curPiece);
            InstantiateTetraMino();
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

        int row = 3;
        int col = 1;
        curPiece = Instantiate(tetraminoPrefab, new Vector3(topLeft.x + col * posOffset, topLeft.y - row * posOffset, 0), Quaternion.identity);

        switch (qIndex)
        {
            case 0:
                curPiece.InitAsO(tileScale, row, col, minoExists);
                break;
            case 1:
                curPiece.InitAsI(tileScale, row, col, minoExists);
                break;
            case 2:
                curPiece.InitAsT(tileScale, row, col, minoExists);
                break;
            case 3:
                curPiece.InitAsL(tileScale, row, col, minoExists);
                break;
            case 4:
                curPiece.InitAsJ(tileScale, row, col, minoExists);
                break;
            case 5:
                curPiece.InitAsS(tileScale, row, col, minoExists);
                break;
            case 6:
                curPiece.InitAsZ(tileScale, row, col, minoExists);
                break;
        }

        qIndex++;
    }
}
