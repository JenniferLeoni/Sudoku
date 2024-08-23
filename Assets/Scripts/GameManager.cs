using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private float _offsetX, _offsetY;
    [SerializeField] private SubGrid _subGridPrefab;
    [SerializeField] private TMP_Text _levelText;
    // private int numberOfMistakes = 0;

    private bool hasGameFinished;
    private Cell[,] cells;
    private Cell selectedCell;
    private int level;

    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        hasGameFinished = false;
        cells = new Cell[GRID_SIZE, GRID_SIZE];
        selectedCell = null;
        SpawnCells();
    }

    private void SpawnCells()
    {
        int[,] puzzleGrid = new int[GRID_SIZE, GRID_SIZE];
        level = PlayerPrefs.GetInt("Level", 0);

        if (level == 0)
        {
            // TEST LEVEL tmpTest = int 1-20
            int tmpTest = 20;
            CreateAndStoreLevel(puzzleGrid, tmpTest);
            level = tmpTest;
            PlayerPrefs.SetInt("Level", tmpTest);

            // RANDOMIZE LEVEL 1-20
            // int randomNumber = Random.Range(1, 20);
            // CreateAndStoreLevel(puzzleGrid, randomNumber);
            // PlayerPrefs.GetInt("Level", randomNumber);
            // level = randomNumber;

            Debug.Log("level: " + level);
        }
        else
        {
            GetCurrentLevel(puzzleGrid);
        }

        _levelText.text = "TYPE " + level.ToString();


        for (int i = 0; i < GRID_SIZE; i++)
        {
            Vector3 spawnPos = _startPos + i % 3 * _offsetX * Vector3.right + i / 3 * _offsetY * Vector3.up;
            SubGrid subGrid = Instantiate(_subGridPrefab, spawnPos, Quaternion.identity);
            List<Cell> subgridCells = subGrid.cells;
            int startRow = (i / 3) * 3;
            int startCol = (i % 3) * 3;
            for (int j = 0; j < GRID_SIZE; j++)
            {
                subgridCells[j].Row = startRow + j / 3;
                subgridCells[j].Col = startCol + j % 3;
                int cellValue = puzzleGrid[subgridCells[j].Row, subgridCells[j].Col];
                subgridCells[j].Init(cellValue);
                cells[subgridCells[j].Row, subgridCells[j].Col] = subgridCells[j];
            }
        }
    }

    private void CreateAndStoreLevel(int[,] grid, int level)
    {
        int[,] tempGrid = Generator.GeneratePuzzle(level);
        string arrayString = "";
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                arrayString += tempGrid[i, j].ToString() + ",";
                grid[i, j] = tempGrid[i, j];
            }
        }

        arrayString = arrayString.TrimEnd(',');
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetString("Grid", arrayString);
    }

    private void GetCurrentLevel(int[,] grid)
    {
        string arrayString = PlayerPrefs.GetString("Grid");
        string[] arrayValue = arrayString.Split(',');
        int index = 0;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                grid[i, j] = int.Parse(arrayValue[index]);
                index++;
            }
        }
    }

    private void GoToEndScene()
    {
        // int level = PlayerPrefs.GetInt("Level", 0);
        // CreateAndStoreLevel(new int[GRID_SIZE, GRID_SIZE], level + 1);
        Debug.Log("GoToEndScene");
        SceneManager.LoadScene("EndScene");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("1");
            UpdateCellValue(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("2");
            UpdateCellValue(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("3");
            UpdateCellValue(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("4");
            UpdateCellValue(4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("5");
            UpdateCellValue(5);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("6");
            UpdateCellValue(6);
        }
        if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("7");
            UpdateCellValue(7);
        }
        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("8");
            UpdateCellValue(8);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("9");
            UpdateCellValue(9);
        }

        if (hasGameFinished || !Input.GetMouseButton(0)) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        Cell tempCell;

        if (hit
            && hit.collider.gameObject.TryGetComponent(out tempCell)
            && tempCell != selectedCell
            && !tempCell.IsLocked
            )
        {
            ResetGrid();
            selectedCell = tempCell;
            HighLight();
        }
        // if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     UpdateCellValue(0);
        //     Debug.Log("0");
        // }
    }

    private void ResetGrid()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].Reset();
            }
        }
    }

    public void UpdateCellValue(int value)
    {
        if (hasGameFinished || selectedCell == null) return;
        selectedCell.UpdateValue(value);
        HighLight();
        CheckWin();
    }

    private void CheckWin()
    {
        Debug.Log("masukCheckwin");
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (cells[i, j].IsIncorrect || cells[i, j].Value == 0) return;
            }
        }

        hasGameFinished = true;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].UpdateWin();
            }
        }

        PlayerPrefs.SetInt("scoreValue", 900);
        Invoke("GoToEndScene", 2f);
    }

    private void HighLight()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                cells[i, j].IsIncorrect = !IsValid(cells[i, j], level);
            }
        }

        int currentRow = selectedCell.Row;
        int currentCol = selectedCell.Col;
        int subGridRow = currentRow - currentRow % SUBGRID_SIZE;
        int subGridCol = currentCol - currentCol % SUBGRID_SIZE;

        for (int i = 0; i < GRID_SIZE; i++)
        {
            cells[i, currentCol].HighLight();
            cells[currentRow, i].HighLight();
            cells[subGridRow + i % 3, subGridCol + i / 3].HighLight();
        }

        cells[currentRow, currentCol].Select();
    }

    private bool IsValid(Cell cell, int level)
    {
        int row = cell.Row;
        int col = cell.Col;
        int value = cell.Value;
        int[,] completedGrid = GetAnswerGrid(level);
        bool isValid = true; // Initialize a flag to track validity

        // Check Row
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (completedGrid[row, i] == value && i != col)
            {
                isValid = false;
                break;
            }
        }

        // Check Column
        for (int i = 0; i < GRID_SIZE; i++)
        {
            if (completedGrid[i, col] == value && i != row)
            {
                isValid = false;
                break;
            }
        }

        // Check Subgrid
        int subGridStartRow = row - row % SUBGRID_SIZE;
        int subGridStartCol = col - col % SUBGRID_SIZE;
        for (int i = subGridStartRow; i < subGridStartRow + SUBGRID_SIZE; i++)
        {
            for (int j = subGridStartCol; j < subGridStartCol + SUBGRID_SIZE; j++)
            {
                if (completedGrid[i, j] == value && (i != row || j != col))
                {
                    isValid = false;
                    break;
                }
            }
        }

        // Increment numberOfMistakes if the input is invalid
        if (!isValid)
        {
            if (PlayerPrefs.GetInt("mistakesValue", 0) == 0)
            {
                PlayerPrefs.SetInt("mistakesValue", 1);
            }
            else
            {
                PlayerPrefs.SetInt("mistakesValue", (PlayerPrefs.GetInt("mistakesValue", 0) + 1));
            }
            // PlayerPrefs.SetInt("mistakesValue", numberOfMistakes);
            if ((PlayerPrefs.GetInt("mistakesValue", 0)) > 5)
            {
                SceneManager.LoadScene("EndScene");
                return false;
            }
            // numberOfMistakes++;
            // PlayerPrefs.SetInt("mistakesValue", numberOfMistakes);
            // if (numberOfMistakes > 5)
            // {
            //     SceneManager.LoadScene("EndScene");
            //     return false;
            // }
        }

        return isValid;
    }

    public static int[,] GetAnswerGrid(int gridNumber)
    {
        int[,] grid;
        switch (gridNumber)
        {
            case 1:
                grid = new int[,]
                {
                    {4,8,9,5,2,1,7,3,6},
                    {7,2,3,8,6,4,9,1,5},
                    {1,6,5,7,9,3,2,8,4},
                    {8,3,6,4,7,9,5,2,1},
                    {9,1,4,2,3,5,6,7,8},
                    {2,5,7,1,8,6,3,4,9},
                    {6,9,8,3,1,7,4,5,2},
                    {5,7,1,9,4,2,8,6,3},
                    {3,4,2,6,5,8,1,9,7}
                };
                break;
            case 2:
                grid = new int[,]
                {
                    {6,7,9,4,8,5,1,2,3},
                    {4,5,3,1,2,9,8,7,6},
                    {8,2,1,7,3,6,9,5,4},
                    {2,8,6,9,1,3,5,4,7},
                    {3,4,7,8,5,2,6,1,9},
                    {1,9,5,6,7,4,3,8,2},
                    {7,3,2,5,9,8,4,6,1},
                    {9,6,8,2,4,1,7,3,5},
                    {5,1,4,3,6,7,2,9,8}
                };
                break;
            case 3:
                grid = new int[,]
                {
                    {5,6,2,4,3,7,1,8,9},
                    {9,4,3,8,6,1,7,5,2},
                    {8,1,7,9,2,5,6,3,4},
                    {4,7,5,3,1,9,8,2,6},
                    {2,9,6,5,8,4,3,1,7},
                    {3,8,1,2,7,6,9,4,5},
                    {6,3,4,7,5,8,2,9,1},
                    {1,5,8,6,9,2,4,7,3},
                    {7,2,9,1,4,3,5,6,8}
                };
                break;
            case 4:
                grid = new int[,]
                {
                    {4,1,3,2,5,7,8,6,9},
                    {9,5,2,6,1,8,7,3,4},
                    {6,8,7,3,9,4,5,1,2},
                    {2,3,5,8,6,9,4,7,1},
                    {8,4,9,1,7,3,6,2,5},
                    {1,7,6,4,2,5,3,9,8},
                    {3,6,8,9,4,2,1,5,7},
                    {7,9,1,5,8,6,2,4,3},
                    {5,2,4,7,3,1,9,8,6}
                };
                break;
            case 5:
                grid = new int[,]
                {
                    {4,5,9,8,1,6,3,2,7},
                    {8,3,7,2,4,9,1,6,5},
                    {2,1,6,5,7,3,4,9,8},
                    {9,7,8,1,6,4,2,5,3},
                    {3,2,4,9,8,5,6,7,1},
                    {5,6,1,3,2,7,8,4,9},
                    {1,9,2,4,5,8,7,3,6},
                    {7,4,3,6,9,1,5,8,2},
                    {6,8,5,7,3,2,9,1,4}
                };
                break;
            case 6:
                grid = new int[,]
                {
                    {6,8,5,1,7,4,2,9,3},
                    {4,3,7,2,9,5,6,8,1},
                    {1,2,9,8,6,3,4,5,7},
                    {2,1,4,7,8,6,5,3,9},
                    {8,7,3,5,4,9,1,6,2},
                    {5,9,6,3,2,1,8,7,4},
                    {9,4,2,6,3,8,7,1,5},
                    {7,5,8,9,1,2,3,4,6},
                    {3,6,1,4,5,7,9,2,8}
                };
                break;
            case 7:
                grid = new int[,]
                {
                    {7,4,6,1,3,9,8,5,2},
                    {1,3,8,2,7,5,9,4,6},
                    {5,9,2,6,8,4,1,3,7},
                    {8,1,5,4,6,3,7,2,9},
                    {2,7,3,8,9,1,5,6,4},
                    {4,6,9,5,2,7,3,8,1},
                    {3,2,4,7,1,8,6,9,5},
                    {9,5,1,3,4,6,2,7,8},
                    {6,8,7,9,5,2,4,1,3}
                };
                break;
            case 8:
                grid = new int[,]
                {
                    {5,3,6,1,8,9,2,7,4},
                    {7,8,1,4,6,2,5,9,3},
                    {4,2,9,3,7,5,8,1,6},
                    {8,7,2,5,9,6,4,3,1},
                    {3,6,5,7,4,1,9,2,8},
                    {1,9,4,2,3,8,7,6,5},
                    {2,4,7,6,5,3,1,8,9},
                    {9,5,3,8,1,7,6,4,2},
                    {6,1,8,9,2,4,3,5,7}
                };
                break;
            case 9:
                grid = new int[,]
                {
                    {3,5,4,9,2,8,1,7,6},
                    {1,6,9,7,5,4,8,2,3},
                    {8,7,2,3,6,1,9,4,5},
                    {5,1,8,4,7,9,3,6,2},
                    {9,2,6,5,8,3,4,1,7},
                    {7,4,3,2,1,6,5,8,9},
                    {6,9,5,1,4,2,7,3,8},
                    {2,3,1,8,9,7,6,5,4},
                    {4,8,7,6,3,5,2,9,1}
                };
                break;
            case 10:
                grid = new int[,]
                {
                    {1,5,7,3,6,8,4,2,9},
                    {3,2,8,5,4,9,1,6,7},
                    {9,4,6,2,7,1,5,3,8},
                    {7,9,2,8,3,5,6,1,4},
                    {5,1,4,7,9,6,2,8,3},
                    {8,6,3,4,1,2,9,7,5},
                    {6,3,5,1,8,4,7,9,2},
                    {4,8,1,9,2,7,3,5,6},
                    {2,7,9,6,5,3,8,4,1}
                };
                break;
            case 11:
                grid = new int[,]
                {
                    {7,5,6,3,2,8,4,9,1},
                    {3,1,8,9,5,4,6,2,7},
                    {4,9,2,1,6,7,5,8,3},
                    {2,8,7,5,3,6,1,4,9},
                    {5,3,9,2,4,1,8,7,6},
                    {1,6,4,8,7,9,2,3,5},
                    {6,7,5,4,9,2,3,1,8},
                    {9,4,1,6,8,3,7,5,2},
                    {8,2,3,7,1,5,9,6,4}
                };
                break;
            case 12:
                grid = new int[,]
                {
                    {5,6,9,7,2,1,8,3,4},
                    {1,7,8,4,5,3,6,9,2},
                    {4,2,3,9,6,8,1,7,5},
                    {9,4,2,6,8,5,7,1,3},
                    {8,5,1,3,7,9,4,2,6},
                    {6,3,7,1,4,2,5,8,9},
                    {7,8,5,2,9,6,3,4,1},
                    {2,1,4,5,3,7,9,6,8},
                    {3,9,6,8,1,4,2,5,7}
                };
                break;
            case 13:
                grid = new int[,]
                {
                    {9,5,3,7,8,4,6,2,1},
                    {6,8,4,2,1,9,3,7,5},
                    {7,1,2,6,5,3,4,8,9},
                    {1,7,5,3,9,2,8,6,4},
                    {8,3,6,4,7,1,5,9,2},
                    {4,2,9,5,6,8,7,1,3},
                    {5,9,1,8,4,7,2,3,6},
                    {2,6,8,9,3,5,1,4,7},
                    {3,4,7,1,2,6,9,5,8}
                };
                break;
            case 14:
                grid = new int[,]
                {
                    {4,8,9,7,1,6,2,5,3},
                    {5,1,6,3,8,2,7,9,4},
                    {7,3,2,4,9,5,8,6,1},
                    {8,7,3,6,5,4,1,2,9},
                    {9,2,1,8,7,3,5,4,6},
                    {6,4,5,9,2,1,3,7,8},
                    {2,5,4,1,6,8,9,3,7},
                    {3,9,8,5,4,7,6,1,2},
                    {1,6,7,2,3,9,4,8,5}
                };
                break;
            case 15:
                grid = new int[,]
                {
                    {6,8,3,1,2,4,7,9,5},
                    {9,4,2,5,6,7,8,1,3},
                    {7,5,1,3,9,8,6,4,2},
                    {2,6,7,9,1,3,5,8,4},
                    {4,3,9,2,8,5,1,6,7},
                    {8,1,5,7,4,6,2,3,9},
                    {5,9,8,6,3,2,4,7,1},
                    {3,7,4,8,5,1,9,2,6},
                    {1,2,6,4,7,9,3,5,8}
                };
                break;
            case 16:
                grid = new int[,]
                {
                    {6,4,7,9,3,5,1,8,2},
                    {2,5,1,8,4,7,9,6,3},
                    {8,9,3,2,6,1,7,5,4},
                    {4,7,8,5,9,2,3,1,6},
                    {3,1,6,4,7,8,2,9,5},
                    {5,2,9,6,1,3,8,4,7},
                    {1,8,2,7,5,4,6,3,9},
                    {9,3,4,1,2,6,5,7,8},
                    {7,6,5,3,8,9,4,2,1}
                };
                break;
            case 17:
                grid = new int[,]
                {
                    {9,2,5,3,6,7,1,8,4},
                    {3,6,4,5,8,1,9,2,7},
                    {8,1,7,2,9,4,5,3,6},
                    {5,7,1,8,4,2,3,6,9},
                    {4,3,8,6,5,9,2,7,1},
                    {2,9,6,1,7,3,4,5,8},
                    {7,4,3,9,2,8,6,1,5},
                    {1,5,9,7,3,6,8,4,2},
                    {6,8,2,4,1,5,7,9,3}
                };
                break;
            case 18:
                grid = new int[,]
                {
                    {7,3,2,9,1,4,8,6,5},
                    {5,9,1,7,6,8,2,4,3},
                    {6,4,8,3,2,5,1,7,9},
                    {8,2,4,6,9,3,5,1,7},
                    {3,6,9,1,5,7,4,2,8},
                    {1,7,5,4,8,2,3,9,6},
                    {2,5,7,8,4,6,9,3,1},
                    {9,8,3,2,7,1,6,5,4},
                    {4,1,6,5,3,9,7,8,2}
                };
                break;
            case 19:
                grid = new int[,]
                {
                    {4,9,5,8,7,2,1,6,3},
                    {2,7,8,6,3,1,9,5,4},
                    {3,6,1,4,9,5,7,8,2},
                    {9,5,3,2,1,7,8,4,6},
                    {8,2,6,3,4,9,5,7,1},
                    {1,4,7,5,8,6,3,2,9},
                    {5,1,4,7,6,3,2,9,8},
                    {7,8,9,1,2,4,6,3,5},
                    {6,3,2,9,5,8,4,1,7}
                };
                break;
            case 20:
                grid = new int[,]
                {
                    {9,4,3,7,6,2,8,1,5},
                    {8,1,7,9,4,5,2,6,3},
                    {5,2,6,1,8,3,4,7,9},
                    {6,5,9,3,7,4,1,2,8},
                    {4,8,1,5,2,6,9,3,7},
                    {3,7,2,8,9,1,6,5,4},
                    {7,6,4,2,5,8,3,9,1},
                    {2,3,5,4,1,9,7,8,6},
                    {1,9,8,6,3,7,5,4,2}
                };
                break;
            default:
                Debug.LogError("Invalid grid number!");
                return null;
        }

        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int[,] reversedGrid = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                reversedGrid[i, j] = grid[rows - 1 - i, j];
            }
        }

        return reversedGrid;
        // PrintGrid(grid);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
