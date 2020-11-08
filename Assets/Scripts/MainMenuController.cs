using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    

    ToggleGroup gameModeGroup;
    public Button startGame;
    
    public Toggle currentSelection{
        get { return gameModeGroup.ActiveToggles().FirstOrDefault(); }
    }
    void Start()
    {
        PlayerPrefs.SetString("oImageSrc", "John Doe");
        PlayerPrefs.SetString("xImageSrc", "xImageSrc");


        Button btn = startGame.GetComponent<Button>();
        btn.onClick.AddListener(StartGame);
        gameModeGroup = GetComponent<ToggleGroup>();
        
    }

   
    public void StartGame()
    {
        PlayerPrefs.SetString("gameMode", currentSelection.name);
        PlayerPrefs.Save();
        SceneManager.LoadScene("TicTacToe");
    }
}
