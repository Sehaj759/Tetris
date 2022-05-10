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

    void Init(int size, float scale)
    {
        this.scale = scale;
        minoExists = new bool[size, size];
        minos = new Mino[4];
        for (int i = 0; i < size; ++i)
        {
            for (int j = 0; j < size; ++j)
            {
                minoExists[i, j] = false;
            }
        }
    }

    void InstantiateMinos(Vector2 topLeftTileCenter, Color color)
    {
        for (int i = 0; i < 4; ++i)
        {
            Mino mino = Instantiate(minoPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            mino.MinoColor = color;

            minos[i] = mino;
        }

        SetMinoPositions(topLeftTileCenter);

        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void InitAsO(float scale)
    {
        Init(3, scale);

        minoExists[0, 1] = true;
        minoExists[0, 2] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), Color.yellow);
    }

    public void InitAsI(float scale)
    {

    }

    public void InitAsT(float scale)
    {
        Init(3, scale);

        minoExists[0, 1] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), new Color(0.5f, 0, 0.5f));
    }

    public void InitAsL(float scale)
    {
        Init(3, scale);

        minoExists[0, 2] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), new Color(1, 0.65f, 0));
    }

    public void InitAsJ(float scale)
    {
        Init(3, scale);

        minoExists[0, 0] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), new Color(0, 0, 0.55f));
    }
    public void InitAsS(float scale)
    {
        Init(3, scale);

        minoExists[0, 1] = true;
        minoExists[0, 2] = true;
        minoExists[1, 1] = true;
        minoExists[1, 0] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), Color.green);
    }
    public void InitAsZ(float scale)
    {
        Init(3, scale);

        minoExists[0, 0] = true;
        minoExists[0, 1] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), Color.red);
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
