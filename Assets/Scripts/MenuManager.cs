using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private string playerName;

    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;


    }

    public void ReadInputField(string s)
    {
        playerName = s;
        Debug.Log(playerName);
    }


}
