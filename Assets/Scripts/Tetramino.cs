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

    Vector3 pos;

    float dropTime = 0.55f;
    float curTime = 0;
    bool canDrop = true;

    int row;
    int col;
    bool[,] board;

    bool setupComplete = false;

    public Color TetraMinoColor { get => minos[0].MinoColor; }

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        if (setupComplete)
        {
            float deltaTime = Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveRight();
            }

            Drop(deltaTime);

            transform.position = pos;
        }
    }

    void MoveLeft()
    {
        // Find LeftMost Column in minoExists that has minos
        int leftCol = 0;
        for(int c = 0; c < minoExists.GetLength(1); ++c)
        {
            bool hasMino = false;
            for(int r = 0; r < minoExists.GetLength(0); ++r)
            {
                if(minoExists[r, c])
                {
                    hasMino = true;
                    break;
                }
            }

            if (hasMino)
            {
                leftCol = c;
                break;
            }
        }

        if ((col - leftCol) > 0)
        {
            pos.x -= scale * tileSize;
            col--;
        }
    }

    void MoveRight()
    {
        // Find RightMost Column in minoExists that has minos
        int rightCol = minoExists.GetLength(1) - 1;
        for (int c = minoExists.GetLength(1) - 1; c >= 0 ; --c)
        {
            bool hasMino = false;
            for (int r = 0; r < minoExists.GetLength(0); ++r)
            {
                if (minoExists[r, c])
                {
                    hasMino = true;
                    break;
                }
            }

            if (hasMino)
            {
                rightCol = c;
                break;
            }
        }

        if ((col + rightCol) < board.GetLength(1) - 1)
        {
            pos.x += scale * tileSize;
            col++;
        }
    }

    void Drop(float deltaTime)
    {
        if (!canDrop)
            return;

        curTime += deltaTime;
        if (curTime >= dropTime)
        {
            // Find Botoom most Row in minoExists that has minos
            int bottomRow = minoExists.GetLength(0) - 1;
            for (int r = minoExists.GetLength(0) - 1; r >= 0; --r)
            {
                bool hasMino = false;
                for (int c = 0; c < minoExists.GetLength(1); ++c)
                {
                    if (minoExists[r, c])
                    {
                        hasMino = true;
                        break;
                    }
                }

                if (hasMino)
                {
                    bottomRow = r;
                    break;
                }
            }

            if ((row + bottomRow) < board.GetLength(0) - 1)
            {
                pos.y -= scale * tileSize;
                row++;
            }
            else
            {
                canDrop = false;
            }
            curTime = 0;
        }
    }

    void Init(int size, float scale, int initRow, int initCol, bool[,] board)
    {
        this.scale = scale;
        row = initRow;
        col = initCol;
        this.board = board;
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

        setupComplete = true;
    }

    public void InitAsO(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 1] = true;
        minoExists[0, 2] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), Color.yellow);
    }

    public void InitAsI(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(4, scale, initRow, initCol, board);

        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;
        minoExists[1, 3] = true;

        InstantiateMinos(new Vector2(-1.5f * tileSize, 1.5f * tileSize), new Color(0.68f, 0.85f, 0.90f));
    }

    public void InitAsT(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 1] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), new Color(0.5f, 0, 0.5f));
    }

    public void InitAsL(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 2] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), new Color(1, 0.65f, 0));
    }

    public void InitAsJ(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 0] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), new Color(0, 0, 0.55f));
    }
    public void InitAsS(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 1] = true;
        minoExists[0, 2] = true;
        minoExists[1, 1] = true;
        minoExists[1, 0] = true;

        InstantiateMinos(new Vector2(-tileSize, tileSize), Color.green);
    }
    public void InitAsZ(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

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
                    minos[minoIndex].transform.localPosition = new Vector3(j * tileSize, -i * tileSize, 0);
                    ++minoIndex;
                }
            }
        }
    }

    public bool Store(out int[,] indices)
    {
        if (canDrop)
        {
            indices = new int[0, 0];
            return false;
        }

        indices = new int[4, 2];

        int storeIndex = 0;
        for(int i = 0; i < minoExists.GetLength(0); ++i)
        {
            for(int j = 0; j < minoExists.GetLength(1); ++j)
            {
                if(minoExists[i, j])
                {
                    indices[storeIndex, 0] = row + i;
                    indices[storeIndex, 1] = col + j;
                    storeIndex++;
                }
            }
        }

        return true;
    }
}
