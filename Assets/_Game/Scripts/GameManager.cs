using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerIOClient;

public class GameManager : MonoBehaviour
{
	public Connection _connection;
	private List<PlayerIOClient.Message> msgList = new List<PlayerIOClient.Message>(); //  Messsage queue implementation
	private bool joinedServer = false;
	private bool joinedroom = false;

	public string infomsg = "";

	public static GameManager Instance { get; private set; }
	public Client _client;

    public NumberPlayer _numberPlayer = NumberPlayer.None;

	public string userid;

    public Text textUI;

    TableManager tableManager;

    public TableOptions currentTableOptions = new TableOptions();

	// Use this for initialization
    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
			Instance = this;

			Screen.sleepTimeout = SleepTimeout.NeverSleep;

			DontDestroyOnLoad(gameObject);
            userid = PlayerPrefs.GetString("username", "Guest" + Random.Range(1, 1000).ToString("000"));
        }

	}

    public void ConnectServer()
    {
		// create a random userid 
        if (userid == "")
            userid = PlayerPrefs.GetString("name", "");

		Debug.Log("Starting");

		var authargs = new Dictionary<string, string> {
			{"userId", userid}				// The id of the user connecting. This can be any string you like. For instance, it might be "fb10239" if youґre building a Facebook app and the user connecting has id 10239
		};

		PlayerIOClient.PlayerIO.Authenticate(
			"deberc-2wqci8fheumyedcfrbbzq",	// Game id (Get your own at playerio.com. 1: Create user, 2:Goto admin pannel, 3:Create game, 4: Copy game id inside the "")
			"public",						// The id of the connection, as given in the settings section of the admin panel. By default, a connection with id='public' is created on all games.
			authargs,							
			null,							// If you are using PlayerInsight, you can specify segments here that the connecting user should be part of.
            delegate (Client client)
            {
				Debug.Log("Successfully connected to Player.IO");
				infomsg = "Successfully connected to Player.IO";

				//target.transform.Find("NameTag").GetComponent<TextMesh>().text = userid;
				//target.transform.name = userid;

				// Uncoment the line below to use the Development Server
                //client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

				_client = client;

                SceneManager.LoadSceneAsync("menu");
				
			},
            delegate (PlayerIOError error)
            {
				Debug.Log("Error connecting: " + error.ToString());
				infomsg = error.ToString();
			}
		);
	}

    public void CreateRoom()
    {
        ConnectRoom();
	}

    public void JoinRoom()
    {
        ConnectRoom();
	}

    public void LeaveRoom()
    {
		if (!joinedroom)
			return;
		
        _connection.Disconnect();
	}

	public void StartGame()
	{
		SceneManager.LoadScene("game");
	}

    public void ConnectRoom()
    {
		//Create or join the room 
        Dictionary<string, string> roomData = new Dictionary<string, string>();
        if (currentTableOptions.numberPlayer != NumberPlayer.None)
            roomData.Add("NumberPlayer", currentTableOptions.numberPlayer.ToString());

		_client.Multiplayer.CreateJoinRoom(
			"UnityDemoRoom",					//Room id. If set to null a random roomid is used
			"DebercRoom",						//The room type started on the server
			true,								//Should the room be visible in the lobby?
            roomData,
			null,
            delegate (Connection connection)
            {
				Debug.Log("Joined Room.");
				infomsg = "Joined Room.";
				// We successfully joined a room so set up the message handler
				_connection = connection;
				_connection.OnMessage += handlemessage;
				joinedroom = true;
                _connection.Send("join");
			},
            delegate (PlayerIOError error)
            {
				Debug.Log("Error Joining Room: " + error.ToString());
				infomsg = error.ToString();
			}
		);
	}

    void handlemessage(object sender, PlayerIOClient.Message m)
    {
        Debug.Log("message.GetString (0) " + m.ToString());
		msgList.Add(m);
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "game" && tableManager == null)
        {
            tableManager = GameObject.Find("Canvas/TableWnd").GetComponent<TableManager>();
        }
		// process message queue
        foreach (PlayerIOClient.Message message in msgList)
        {
            switch (message.Type)
            {
				case "init":
					Debug.Log("init " + message.GetInteger(0) + " " + message.GetString(1));
                    break;

                case "initPlayers":
                    if (SceneManager.GetActiveScene().name == "menu")
                    {
                        GameObject.Find("Canvas/WaitingLaunchPopUp").GetComponent<WaitingLaunchActivate>().SetPlayers(message.GetString(0), message.GetString(1), message.GetString(2), message.GetString(3));
                        Debug.Log("initPlayers " + message.GetString(0) + " " + message.GetString(1) + " " + message.GetString(2) + " " + message.GetString(3));
					}
					break;

                case "connect":
					Debug.Log("connect " + message.GetString(0));
					break;

                case "left":
                    Debug.Log("left " + message.GetString(0));
                    break;

                case "join":
                    GameObject.Find("Canvas/WaitingLaunchPopUp").GetComponent<WaitingLaunchActivate>().buttonPlay.interactable = true;
                    break;

                case "cards":
                    if (SceneManager.GetActiveScene().name == "game")
                    {
                        _numberPlayer = (NumberPlayer)System.Enum.Parse(typeof(NumberPlayer), message.GetString(0));
                        GameObject.Find("Canvas/TableWnd").GetComponent<TableManager>().SetNumberPlayers(message.GetString(1), message.GetString(2), 
                            message.GetString(3), message.GetString(4), message.GetString(5), message.GetString(6), message.GetString(7), message.GetString(8));
                        Debug.Log("startGame " + message.ToString());
                    }
                    break;

                case "choiseTrumpFirst":
                    tableManager.trumpManager.trumpFirst = true;
                    tableManager.trumpManager.gameObject.SetActive(true);
                    tableManager.trumpManager.VisibleTrump(message.GetString(0));
                    break;

                case "choiseTrumpSecond":
                    tableManager.trumpManager.trumpFirst = false;
                    tableManager.trumpManager.gameObject.SetActive(true);
                    tableManager.trumpManager.VisibleTrump(message.GetString(0));
                    break;

                case "setTrumpFirst":
                    GameObject.Find("Canvas/TableWnd").GetComponent<TableManager>().trumpCard.SetActive(true);
                    GameObject.Find("Canvas/TableWnd").GetComponent<TableManager>().lastCard.SetActive(true);
                    GameObject.Find("Canvas/TableWnd").GetComponent<TableManager>().trumpCard.GetComponent<Card>().SetCard(message.GetString(0));
                    GameObject.Find("Canvas/TableWnd").GetComponent<TableManager>().lastCard.GetComponent<Card>().SetCard("back");
                    break;

                    //TODO переделать мб - показываем выбранный козырь
                case "selectedTrump":
                    tableManager.trumpManager.SelectedTrumpVisible(message.GetString(0));
                    break;

                case "hasTurn":
                    if (SceneManager.GetActiveScene().name == "game")
                    {
                        tableManager.setHasTurn(message.GetString(0));
                    }
                    break;

                case "reset":
                    Debug.Log("reset " + message.GetInteger(0));
					break;
			}
		}
        msgList.Clear();
	}

    void Update()
    {
		
	}
}
