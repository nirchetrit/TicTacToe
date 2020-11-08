using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    private GameController gameController;
    public AudioClip audioClip;
    private AudioSource selectSound;
    public Button button;
    public Image btnImg;
    public Sprite xImg;
    public Sprite oImg;
    public Sprite highlightSprite;
    private float highlightDelay;
    public Sprite emptySprite;

    private void Awake()
    {
        highlightDelay = 10;
        selectSound = gameObject.AddComponent<AudioSource>();
        selectSound.clip = audioClip;
    }
    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
  
    public void SetSpace()
    {
        if (!gameController.isComputerTurn)
        {
            selectSound.Play();
            btnImg.sprite = gameController.GetPlayerSide() == "ExTarget" ? xImg : oImg;
            button.interactable = false;
            gameController.EndTurn(this);
        }
    }
    private void Update()
    {
        if (btnImg.sprite == highlightSprite)
        //means the gridspace is highlight right now..
        {
            highlightDelay += highlightDelay * Time.deltaTime;
            if (highlightDelay >= 50)
            {
                btnImg.sprite = emptySprite;
            }
        }
    }
    public void Highlight() {
        highlightDelay = 10;
        btnImg.sprite = highlightSprite;
    }

    public void setImage(string str)
    {
        if (str == "ExTarget")
        {
            btnImg.sprite = xImg;
        }
        else if (str == "CircleTarget")
        {
            btnImg.sprite = oImg;
        }
        else if (str == "Empty")
        {
            btnImg.sprite = emptySprite;
        }
        
    }
}
