using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLaunchActivate : MonoBehaviour {
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public Button buttonPlay;

    public Text Rate;
    public Text CountPlayers;
    public Text Limit;
    public Text TimeTurn;
    public Text ReplacingSeven;

    // Use this for initialization
    void OnEnable () {
        buttonPlay.interactable  = false;
        player1.SetActive(true);
        if (GameManager.Instance)
            UtilityGame.GetTransformChild(player1.transform, "PlayerName").GetComponent<Text>().text = GameManager.Instance.userid;

        player2.SetActive(false);
        player3.SetActive(false);
        player4.SetActive(false);
        //TODO переделать на вариант с разными языками
        Rate.text = "Ставка: " + GameManager.Instance.currentTableOptions.rate.ToString();
        CountPlayers.text = "Игроки: " + LocalizationManager.Instance.GetText("ROOM_OPTIONS_NUMBER_PLAYER_" + GameManager.Instance.currentTableOptions.numberPlayer.ToString());
        Limit.text = "Лимит: " + LocalizationManager.Instance.GetText("ROOM_OPTIONS_LIMIT_" + GameManager.Instance.currentTableOptions.limit.ToString());
        TimeTurn.text = "Время на ход: " + GameManager.Instance.currentTableOptions.timeTurn.ToString();
        ReplacingSeven.text = "Замена семерки: " + ((GameManager.Instance.currentTableOptions.replacingSeven) ? "+" : "-");
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetPlayers(string playerName1, string playerName2, string playerName3, string playerName4)
    {
        if (playerName1 != "")
        {
            player1.SetActive(true);
            UtilityGame.GetTransformChild(player1.transform, "PlayerName").GetComponent<Text>().text = playerName1;
        }
        if (playerName2 != "")
        {
            player2.SetActive(true);
            UtilityGame.GetTransformChild(player2.transform, "PlayerName").GetComponent<Text>().text = playerName2;
        }
        if (playerName3 != "")
        {
            player3.SetActive(true);
            UtilityGame.GetTransformChild(player3.transform, "PlayerName").GetComponent<Text>().text = playerName3;
        }
        if (playerName4 != "")
        {
            player4.SetActive(true);
            UtilityGame.GetTransformChild(player4.transform, "PlayerName").GetComponent<Text>().text = playerName4;
        }
    }
}
