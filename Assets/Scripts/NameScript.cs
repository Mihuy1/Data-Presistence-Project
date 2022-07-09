using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameScript : MonoBehaviour
{
    public Text PlayerNameText;

    public string playerName;
    public string bestScore;


    private void Awake()
    {
        playerName = MenuManager.Instance.playerName;
    }

    private void Update()
    {
        PlayerNameText.text = playerName;
    }
}
