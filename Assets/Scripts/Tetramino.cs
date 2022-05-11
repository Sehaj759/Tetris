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

            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateLeft();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateRight();
            }

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

        if ((col + leftCol) > 0)
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
                for(int c = 0; c < minoExists.GetLength(1); ++c)
                {
                    for(int r = minoExists.GetLength(0) - 1; r >= 0; --r)
                    {
                        if(minoExists[r, c] && board[row + r + 1, col + c])
                        {
                            canDrop = false;
                            break;
                        }
                    }
                    if (!canDrop)
                        break;
                }

                if (canDrop)
                {
                    pos.y -= scale * tileSize;
                    row++;
                }
            }
            else
            {
                canDrop = false;
            }
            curTime = 0;
        }
    }

    void RotateLeft()
    {
        int n = minoExists.GetLength(0);
        bool[,] temp = new bool[n, n];
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                temp[i, j] = minoExists[j, n - i - 1];
            }
        }

        // check if new orientation puts the tetramino out of bounds
        bool outOfBounds = false;

        for(int i = 0; i < temp.GetLength(0); ++i)
        {
            for(int j = 0; j < temp.GetLength(1); ++j)
            {
                if(temp[i, j] && !((col + j) >= 0 && (col + j) < board.GetLength(1)))
                {
                    outOfBounds = true;
                    break;
                }
            }
            if (outOfBounds)
                break;
        }

        if (!outOfBounds)
        {
            minoExists = temp;
            SetMinoPositions();
        }
    }

    void RotateRight()
    {
        int n = minoExists.GetLength(0);
        bool[,] temp = new bool[n, n];
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                temp[i, j] = minoExists[n - j - 1, i];
            }
        }

        // check if new orientation puts the tetramino out of bounds
        bool outOfBounds = false;

        for (int i = 0; i < temp.GetLength(0); ++i)
        {
            for (int j = 0; j < temp.GetLength(1); ++j)
            {
                if (temp[i, j] && !((col + j) >= 0 && (col + j) < board.GetLength(1)))
                {
                    outOfBounds = true;
                    break;
                }
            }
            if (outOfBounds)
                break;
        }

        if (!outOfBounds)
        {
            minoExists = temp;
            SetMinoPositions();
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

    void InstantiateMinos(Color color)
    {
        for (int i = 0; i < 4; ++i)
        {
            Mino mino = Instantiate(minoPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            mino.MinoColor = color;

            minos[i] = mino;
        }

        SetMinoPositions();

        transform.localScale = new Vector3(scale, scale, 1);

        setupComplete = true;
    }

    public void InitAsO(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(2, scale, initRow, initCol, board);

        minoExists[0, 0] = true;
        minoExists[0, 1] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;

        InstantiateMinos(Color.yellow);
    }

    public void InitAsI(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(4, scale, initRow, initCol, board);

        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;
        minoExists[1, 3] = true;

        InstantiateMinos(new Color(0.68f, 0.85f, 0.90f));
    }

    public void InitAsT(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 1] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Color(0.5f, 0, 0.5f));
    }

    public void InitAsL(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 2] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Color(1, 0.65f, 0));
    }

    public void InitAsJ(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 0] = true;
        minoExists[1, 0] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(new Color(0, 0, 0.55f));
    }
    public void InitAsS(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 1] = true;
        minoExists[0, 2] = true;
        minoExists[1, 1] = true;
        minoExists[1, 0] = true;

        InstantiateMinos(Color.green);
    }
    public void InitAsZ(float scale, int initRow, int initCol, bool[,] board)
    {
        Init(3, scale, initRow, initCol, board);

        minoExists[0, 0] = true;
        minoExists[0, 1] = true;
        minoExists[1, 1] = true;
        minoExists[1, 2] = true;

        InstantiateMinos(Color.red);
    }

    void SetMinoPositions()
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
