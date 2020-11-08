using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class GameController : MonoBehaviour
{
    private Stack<char[,]> boardHistory;
    public Image[] buttonList;
    private string playerSide;
    public GameObject gameOverPanel;
    public GameObject restartButton;
    private float time;
    private bool stopStopWatch;
    public Text stopWatch;
    public Text gameOverText;
    private int moveCount;
    public bool isComputerTurn;
    private float delay;
    private int value;
    public GameObject undoButton;
    public GameObject hintButton;
    public AudioClip winningSoundClip;
    private AudioSource gameOverSoundClip;
    private GameControllerLogic gameControllerLogic;
    string gameMode;


    private void defaultValues() { 
        playerSide = "ExTarget";
        gameOverPanel.SetActive(false);
        moveCount = 0;
        delay = 10;
        time = 0;
        stopStopWatch = false;
        restartButton.SetActive(false);
        if (gameMode == "PlayerVComputer")
        {
            undoButton.SetActive(true);
            hintButton.SetActive(true);
        }
        if (gameMode == "ComputerVComputer")
        {
            isComputerTurn = true;
        }
        else
        {
            isComputerTurn = false;
        }
        SetBoardInteractable(true);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().setImage("Empty");
        }
    }
    void Awake()
    {
        boardHistory = new Stack<char[,]>();
        gameControllerLogic = new GameControllerLogic();
        gameOverSoundClip = gameObject.AddComponent<AudioSource>();
        gameOverSoundClip.clip = winningSoundClip;
        gameMode = PlayerPrefs.GetString("gameMode");
        if (!(gameMode == "PlayerVComputer"))
        {
            undoButton.SetActive(false);
            hintButton.SetActive(false);
        }
        SetGameControllerReferenceOnButtons();
        boardHistory.Push(GenerateBoardFromImages(buttonList));
        defaultValues();
    }
    private void Update()
    {
        if (!stopStopWatch)
        {
            time += Time.deltaTime;
            stopWatch.text = time.ToString("0.0");
        }
        
        if (isComputerTurn)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 50)
            {
                value = UnityEngine.Random.Range(0, buttonList.Length);
                if (buttonList[value].GetComponentInParent<Button>().interactable)
                {
                    buttonList[value].GetComponentInParent<GridSpace>().setImage(GetPlayerSide());
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTurn(buttonList[value].GetComponentInParent<GridSpace>());
                }
            }
        }
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }
    
    void ChangeSides()
    {
        playerSide = (playerSide == "ExTarget") ? "CircleTarget" : "ExTarget";
        delay = 10;
    }
    public void EndTurn(GridSpace btn)
    {
        if (gameMode == "PlayerVComputer")
        {
            isComputerTurn = !isComputerTurn;
        }
        
        
        moveCount++;
        char[,] board = GenerateBoardFromImages(buttonList);
        boardHistory.Push(board);
        if (gameControllerLogic.isWin(board, playerSide))
        {
            GameOver(playerSide);
        }

        else if (moveCount >= 9)
        {
            GameOver("Draw");
        }

        else {
            ChangeSides(); 
        }
        
    }
    void GameOver(string winningPlayer)
    {
        string winText="";
        SetBoardInteractable(false);
        undoButton.SetActive(false);
        hintButton.SetActive(false);
        if (winningPlayer == "ExTarget")
        {
            winText = "Player 1 wins";
        }
        else if (winningPlayer == "CircleTarget")
        {
            winText = "Player 2 wins";
        }
        else if (winningPlayer == "Draw")
        {
            winText = "Draw";
        }
        gameOverSoundClip.Play();
        stopStopWatch = true;
        restartButton.SetActive(true);
        gameOverText.text = winText;
        gameOverPanel.SetActive(true);
    }
    public void RestartGame()
    {
        defaultValues();
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void setBoard(char[,] board)
    {
        SetBoardInteractable(true);
        for(int i = 0; i < 9; i++)
        {
            char symbol = board[i / 3, i % 3];
            if (symbol == 'x')
            {
                buttonList[i].GetComponentInParent<GridSpace>().setImage("ExTarget");
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }
            else if (symbol == 'o')
            {
                buttonList[i].GetComponentInParent<GridSpace>().setImage("CircleTarget");
                buttonList[i].GetComponentInParent<Button>().interactable = false;
            }
            else
            {
                buttonList[i].GetComponentInParent<GridSpace>().setImage("Empty");
            }
        }
    }
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
    public void Undo()
    {
        ////undo the 2 previus steps as 1 is for the computer and 1 for the player.
       
         if (moveCount >= 2&&!isComputerTurn)
        {
            char[,] updatedBoardAfterUndo = gameControllerLogic.getPreviusBoardState(boardHistory);
            setBoard(updatedBoardAfterUndo);
           moveCount -= 2;
        }
        
        
    }
    public void Hint()
    {
        ///if we want the option to have hints we first need to solve the graph.
        ///we will represent it as a char[,], solve it using MiniMax and than we can supply the hint.

        int bestIndex = gameControllerLogic.getHintIndex((GenerateBoardFromImages(buttonList))); 
        buttonList[bestIndex].GetComponentInParent<GridSpace>().Highlight();
    }
    public char[,] GenerateBoardFromImages(Image[] buttonList) {
        char[,] board = {{ '_', '_', '_' },
                        { '_', '_', '_' },
                        { '_', '_', '_' }};
        for(int i = 0; i < buttonList.Length; i++)
        {
                if (getSpriteName(buttonList[i]) == "ExTarget")
                {
                   int x = i / 3;
                   int y = i % 3;
                    board[x, y] = 'x';
                }
                else if (getSpriteName(buttonList[i]) == "CircleTarget")
                {
                  int  x = i / 3;
                   int y = i % 3;
                    board[x, y] = 'o';
                }
            
        }
        return board;
    }
    public string getSpriteName(Image button)
    {
        return button.GetComponentInParent<GridSpace>().btnImg.sprite.name;
    }
}
public class GameControllerLogic {
    public bool isWin(char[,] b, string playerSide)
    {
        char player = ' ';
        if (playerSide == "ExTarget")
        {
            player = 'x';
        }
        else if (playerSide == "CircleTarget")
        {
            player = 'o';
        }
        //check for rows
        for(int row = 0; row < 3; row++)
        {
            if (b[row, 0] == b[row, 1] && b[row, 1] == b[row, 2])
            {
                if (b[row, 0] == player)
                {
                    return true;
                }
            }
        }
        //checks for columns
        for (int col = 0; col < 3; col++)
        {
            if (b[0, col] == b[1, col] && b[1, col] == b[2, col])
            {
                if (b[0, col] == player)
                {
                    return true;
                }
            }
        }

        //checks for diagonals
        if (b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
        {
            if (b[0, 0] == player)
            {
                return true;
            } 
        }
        if (b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
        {
            if (b[0, 2] == player) {
                return true;
            }
        }
        return false;
    }
    public int getHintIndex(char[,] board) {
        return new MiniMaxSolver().findBestMove(board);
    }


    public char[,] getPreviusBoardState(Stack<char[,]> history) {
        if (history.Count < 3)
        {
            return history.Peek();
        }
        history.Pop();
        history.Pop();
        return history.Peek();
    }
}




  