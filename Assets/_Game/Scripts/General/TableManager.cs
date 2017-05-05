using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    public Player playerLocal;

    public Player playerOther1;
    public Player playerOther2;
    public Player playerOther3;

    public List<Player> playersList = new List<Player>();

    public TrumpManager trumpManager;

    public GameObject trumpCard;
    public GameObject lastCard;

    // Use this for initialization
    void OnEnable()
    {
        playerOther1.gameObject.SetActive(false);
        playerOther2.gameObject.SetActive(false);
        playerOther3.gameObject.SetActive(false);

		if (trumpManager != null)
            trumpManager.gameObject.SetActive(false);
		
		if (trumpCard != null)
			trumpCard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public struct Data
    {
		public int IntegerData { get; private set; }
        public string StringData { get; private set; }
        public string StringData2 { get; private set; }

        public Data(string strValue, string strValue2, int intValue)
        {
            this.StringData = strValue;
            this.StringData2 = strValue2;
            this.IntegerData = intValue;
        }
    }

    public void SetNumberPlayers(string playerName1, string playerName2, string playerName3, string playerName4) {
        SetNumberPlayers(playerName1, "", playerName2, "", playerName3, "", playerName4, "");

    }

    public void SetNumberPlayers(string playerName1, string playerCard1, string playerName2, string playerCard2, string playerName3, string playerCard3, string playerName4, string playerCard4)
    {
        //TODO переделать ссылки на массив
        List<Data> playerNameList = new List<Data>();
        List<Data> playerNameListSort = new List<Data>();
        if (playerName1 !="")
            playerNameList.Add(new Data(playerName1, playerCard1, 1));
        if (playerName2 != "")
            playerNameList.Add(new Data(playerName2, playerCard2, 2));
        if (playerName3 != "")
            playerNameList.Add(new Data(playerName3, playerCard3, 3));
        if (playerName4 != "")
            playerNameList.Add(new Data(playerName4, playerCard4, 4));

        // ищем локального игрока
        int i = 0;
        int index = -1;
        foreach (Data data in playerNameList) {
            if (data.StringData == GameManager.Instance.userid) {
                index = i;
            }
            i++;
        }

        i = 0;
        foreach (Data data in playerNameList)
        {
            if (i < index)
            {
                playerNameListSort.Add(new Data(data.StringData, data.StringData2, i + playerNameList.Count));
            }
            if (i > index)
            {
                playerNameListSort.Add(new Data(data.StringData, data.StringData2, i));
            }
            i++;
        }
        playerLocal.setCardFromString(playerNameList[index].StringData2);
        playersList.Add(playerLocal);
        playerLocal.setNick(GameManager.Instance.userid);
        playerNameList.RemoveAt(index);

        playerNameListSort.Sort(delegate (Data x, Data y)
        {
            return x.IntegerData.CompareTo(y.IntegerData);
        });


        switch (GameManager.Instance._numberPlayer)
        {
            case NumberPlayer.TwoPlayers:
                //включаем напротив
                if (playerNameListSort[0].StringData != "")
                {
                    playerOther2.gameObject.SetActive(true);
                    playerOther2.setNick(playerNameListSort[0].StringData);
                    playerOther2.setCardFromString(playerNameList[0].StringData2);
                    playersList.Add(playerOther2);
                }
                break;
            case NumberPlayer.ThreePlayers:
                //включаем слева и напротив
                if (playerNameListSort[0].StringData != "")
                {
                    playerOther1.gameObject.SetActive(true);
                    playerOther1.setNick(playerNameListSort[0].StringData);
                    playerOther1.setCardFromString(playerNameList[0].StringData2);
                    playersList.Add(playerOther1);
                }
                if (playerNameListSort[1].StringData != "")
                {
                    playerOther2.gameObject.SetActive(true);
                    playerOther2.setNick(playerNameListSort[1].StringData);
                    playerOther2.setCardFromString(playerNameList[1].StringData2);
                    playersList.Add(playerOther2);
                }
                break;
            case NumberPlayer.FourthPlayers:
            case NumberPlayer.PlayACouple:
                //включаем всех
                if (playerNameListSort[0].StringData != "")
                {
                    playerOther1.gameObject.SetActive(true);
                    playerOther1.setNick(playerNameListSort[0].StringData);
                    playerOther1.setCardFromString(playerNameList[0].StringData2);
                    playersList.Add(playerOther1);
                }
                if (playerNameListSort[1].StringData != "")
                {
                    playerOther2.gameObject.SetActive(true);
                    playerOther2.setNick(playerNameListSort[1].StringData);
                    playerOther2.setCardFromString(playerNameList[1].StringData2);
                    playersList.Add(playerOther2);
                }
                if (playerNameListSort[2].StringData != "")
                {
                    playerOther3.gameObject.SetActive(true);
                    playerOther3.setNick(playerNameListSort[2].StringData);
                    playerOther3.setCardFromString(playerNameList[2].StringData2);
                    playersList.Add(playerOther3);
                }
                break;
        }
    }

    public void setHasTurn(string userid) {
        foreach (Player _player in playersList) {
            _player.setHasTurn(_player.userid == userid);
        }
    }
}
