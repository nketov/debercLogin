using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerIO.GameLibrary;

namespace ServerDeberc
{
    public class Player : BasePlayer
    {
        public Boolean IsReady = true;
        public Boolean IsStartGame = false;
        public Boolean IsChoseTrumpFirst = false;
        public Boolean IsChoseTrumpSecond = false;
        public string[] Cards = new string[9];
    }

    [RoomType("DebercRoom")]
    public class GameCode : Game<Player>
    {
        private Player[] playersArray;
        private Player hasTurn;
        private Player cardDealer;
        private Player choosingTrump;

        List<string> CardDeck = new List<string>();

        private Suit trump = Suit.None;

        // This method is called when an instance of your the game is created
        public override void GameStarted()
        {
            // anything you write to the Console will show up in the 
            // output window of the development server
            Console.WriteLine("Game is started");

            NumberPlayer _numberPlayer = (NumberPlayer)Enum.Parse(typeof(NumberPlayer), RoomData["NumberPlayer"]);

            switch (_numberPlayer) {
                case NumberPlayer.TwoPlayers:
                    playersArray = new Player[2];
                    break;
                case NumberPlayer.ThreePlayers:
                    playersArray = new Player[3];
                    break;
                case NumberPlayer.FourthPlayers:
                case NumberPlayer.PlayACouple:
                    playersArray = new Player[4];
                    break;

            }
            Console.WriteLine("NumberPlayer " + RoomData["NumberPlayer"]);
        }


        // This method is called when the last player leaves the room, and it's closed down.
        public override void GameClosed()
        {
            Console.WriteLine("RoomId: " + RoomId);
        }


        public override void UserJoined(Player player)
        {
            //joinGame(player);

            /*foreach (Player _player in Players)
            {
                if (_player.ConnectUserId != player.ConnectUserId)
                {
                    _player.Send("Connect", "Connecting:" + player.ConnectUserId.ToString());
                }
            }*/
        }

        public override void UserLeft(Player player)
        {
            for (int i = 0; i < playersArray.Length; i++)
            {
                if (playersArray[i]== player)
                    playersArray[i] = null;
            }

            Console.WriteLine("UserLeft");

            Broadcast("left", GetPlayerConnectUserId(0), GetPlayerConnectUserId(1), GetPlayerConnectUserId(2), GetPlayerConnectUserId(3));
        }

        public override void GotMessage(Player player, Message message)
        {
            switch (message.Type)
            {
                case "reset":
                    {
                        if (playersArray.Length == CountPlayerIsNotNull())
                        {
                            player.IsReady = true;
                            if (playersArray.Length == CountPlayerIsReady())
                            {
                                resetGame(null);
                            }
                        }
                        break;
                    }
                case "join":
                    {
                        joinGame(player);
                        break;
                    }
                case "startGame":
                    {
                        player.IsStartGame = true;
                        startGame(player);
                        break;
                    }

                case "setTrump":
                    if (player == choosingTrump) {
                        trump = (Suit)Enum.Parse(typeof(Suit), message.GetString(0));
                        hasTurn = player;
                        Broadcast("hasTurn", hasTurn.ConnectUserId);
                        Broadcast("selectedTrump", trump.ToString());
                        Console.WriteLine("setTrump " + trump.ToString() + " hasTurn " + player.ConnectUserId);
                    }
                    break;

                //TODO перейти к состояниям
                case "passTrumpFirst":
                    player.IsChoseTrumpFirst = true;
                    choosingTrump = nextPlayer(player);
                    Broadcast("hasTurn", choosingTrump.ConnectUserId);

                    if (!choosingTrump.IsChoseTrumpFirst)
                        choosingTrump.Send("choiseTrumpFirst", CardDeck[0]);
                    else
                        choosingTrump.Send("choiseTrumpSecond", CardDeck[0]);
                    break;

                case "passTrumpSecond":
                    player.IsChoseTrumpSecond = true;
                    choosingTrump = nextPlayer(player);
                    Broadcast("hasTurn", choosingTrump.ConnectUserId);

                    if (!choosingTrump.IsChoseTrumpSecond)
                    {
                        choosingTrump.Send("choiseTrumpSecond", CardDeck[0]);
                    }
                    else
                    {
                        trump = GetSuit(CardDeck[0]);
                        hasTurn = cardDealer;
                        Broadcast("hasTurn", hasTurn.ConnectUserId);
                        Broadcast("selectedTrump", trump.ToString());
                        Console.WriteLine("setTrump " + trump.ToString() + " hasTurn " + player.ConnectUserId);
                    }
                    break;
            }
        }

        private void joinGame(Player user)
        {
            Console.WriteLine("joinGame");

            NumberPlayer _numberPlayer = (NumberPlayer)Enum.Parse(typeof(NumberPlayer), RoomData["NumberPlayer"]);

            int i = 0;
            bool flagInit = false;
            foreach (Player _player in playersArray)
            {
                if (_player == null && !flagInit)
                {
                    Broadcast("init", i, user.ConnectUserId);
                    playersArray[i] = user;
                    flagInit = true;
                }
                i++;
            }

            Broadcast("initPlayers", GetPlayerConnectUserId(0), GetPlayerConnectUserId(1), GetPlayerConnectUserId(2), GetPlayerConnectUserId(3));

            if (!flagInit) 
            {
                //Send current game state to spectators
               /* user.Send("spectator", player1.ConnectUserId, player2.ConnectUserId,
                    field[0, 0].Type, field[1, 0].Type, field[2, 0].Type,
                    field[0, 1].Type, field[1, 1].Type, field[2, 1].Type,
                    field[0, 2].Type, field[1, 2].Type, field[2, 2].Type
                );*/
                //return;
            }

            Console.WriteLine("CountPlayerIsNotNull()" + CountPlayerIsNotNull().ToString() + " players.Length " + playersArray.Length.ToString());
            if (CountPlayerIsReady() == playersArray.Length)
            {
                Console.WriteLine("Broadcast joinGame " + GetPlayerConnectUserId(0) + " " + GetPlayerConnectUserId(1) + " " + GetPlayerConnectUserId(2) + " " + GetPlayerConnectUserId(3));
                Broadcast("join", GetPlayerConnectUserId(0), GetPlayerConnectUserId(1), GetPlayerConnectUserId(2), GetPlayerConnectUserId(3));
                //user.Send("join", GetPlayerConnectUserId(0), GetPlayerConnectUserId(1), GetPlayerConnectUserId(2), GetPlayerConnectUserId(3));
                //resetGame(user);
            }
        }

        private void startGame(Player user)
        {
            Console.WriteLine("CountPlayerIsStartGame()" + CountPlayerIsNotNull().ToString() + " players.Length " + playersArray.Length.ToString());

            if (cardDealer == null)
            {
                cardDealer = user;
                Console.WriteLine("cardDealer " + cardDealer.ConnectUserId);
                cardDealer.IsChoseTrumpFirst = true;

                choosingTrump = nextPlayer(user);
                Broadcast("hasTurn", choosingTrump.ConnectUserId);
            }

            if (CountPlayerIsStartGame() == playersArray.Length)
            {
                resetGame(user);
            }
        }

        private void resetGame(Player user)
        {
            //TODO reset cards
            ResetCardDeck();

            for (int i = 0; i < playersArray.Length; i++)
            {
                playersArray[i].IsReady = false;
                playersArray[i].IsStartGame = false;
                playersArray[i].IsChoseTrumpFirst = false;
                playersArray[i].IsChoseTrumpSecond = false;
            }

            Console.WriteLine("set6PlayerCards");
            //заполняем первые 6 карт у игроков
            for (int i = 0; i < playersArray.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    playersArray[i].Cards[j] = CardDeck[0];
                    CardDeck.RemoveAt(0);
                }
            }

            // показываем игрокам карты
            SetCards();

            // следующий за сдающим выберает козырь
            choosingTrump.Send("choiseTrumpFirst", CardDeck[0]);
            Broadcast("hasTurn", choosingTrump.ConnectUserId);
            //?choosingTrump.IsChoseTrumpFirst = true;
            //показываем всем первый козырь
            Broadcast("setTrumpFirst", CardDeck[0]);
        }

        private Player GetPlayer(int index) {
            Player _player = null;
            if (index < playersArray.Length)
                _player = playersArray[index];
            return _player;
        }

        //id игрока из массива игроков
        //TODO поменять на массив в комнате; 
        private string GetPlayerConnectUserId(int index)
        {
            Player _player = GetPlayer(index);

            if (_player != null)
                return _player.ConnectUserId;
            
            return "";
        }

        private int CountPlayerIsNotNull() {
            int countPlayer = 0;
            foreach (Player _player in playersArray)
            {
                if (_player != null)
                {
                    countPlayer++;
                }
            }
            return countPlayer;
        }

        // TODO заменить на массив игроков
        private int CountPlayerIsReady()
        {
            int countPlayerIsReady = 0;
            foreach (Player _player in playersArray)
            {
                if (_player != null && _player.IsReady)
                {
                    countPlayerIsReady++;
                }
            }
            return countPlayerIsReady;
        }

        // количество игроков которые зашли в комнату и подтвердили участие
        private int CountPlayerIsStartGame()
        {
            int countPlayerIsStartGame = 0;
            foreach (Player _player in playersArray)
            {
                if (_player != null && _player.IsStartGame)
                {
                    countPlayerIsStartGame++;
                }
            }
            return countPlayerIsStartGame;
        }

        //следующий игрок
        private Player nextPlayer(Player user)
        {
            int index = -1;
            for (int i = 0; i < playersArray.Length; i++) {
                if (user == playersArray[i])
                    index = i;
            }

            if (index + 1 > playersArray.Length - 1)
                index = 0;
            else
                index++;

            return playersArray[index];
        }

        // сбрасываем и перемешивам карты
        private void ResetCardDeck() {
            string [] CardDeckArray = new string[] { "C7", "C8", "C9", "C10", "CA", "CJ", "CK", "CQ", "D7", "D8", "D9", "D10", "DA", "DJ", "DK", "DQ",
                              "H7", "H8", "H9", "H10", "HA", "HJ", "HK", "HQ",
                              "S7", "S8", "S9", "S10", "SA", "SJ", "SK", "SQ"};
            CardDeck = new List<string>(CardDeckArray);

            //перемешали
            Random random = new System.Random();

            for (int i = CardDeck.Count-1; i >= 1; i--)
            { 
                int j = random.Next(i + 1);
                // обменять значения data[j] и data[i]
                var temp = CardDeck[j];
                CardDeck[j] = CardDeck[i];
                CardDeck[i] = temp;
            }
        }

        //посылаем карты, которые на руках у игроков
        private void SetCards() {

            string[] playerCards = new string[4];
            string[] playerCardsOther = new string[4];
            for (int i = 0; i < 4; i++)
            {
                if (GetPlayer(i) == null)
                {
                    playerCards[i] = "";
                    playerCardsOther[i] = "";
                }
                else if (GetPlayer(i) != null)
                {
                    playerCards[i] = string.Join(",", playersArray[i].Cards);
                    for (int j = 0; j < playersArray[i].Cards.Length; j++)
                    {
                        playerCardsOther[i] += (playersArray[i].Cards[j] != "") ? "back," : ",";
                    }
                    playerCardsOther[i] = playerCardsOther[i].Remove(playerCardsOther[i].Length - 1);
                }
                else
                {
                    playerCards[i] = "";
                    playerCardsOther[i] = "";
                }
            }

            Console.WriteLine("Broadcast cards " + GetPlayerConnectUserId(0) + " " + GetPlayerConnectUserId(1) + " " + GetPlayerConnectUserId(2) + " " + GetPlayerConnectUserId(3));

            for (int i = 0; i < playersArray.Length; i++)
            {
                playersArray[i].Send("cards", RoomData["NumberPlayer"],
                                        GetPlayerConnectUserId(0), (GetPlayerConnectUserId(0) == playersArray[i].ConnectUserId) ? playerCards[0] : playerCardsOther[0],
                                        GetPlayerConnectUserId(1), (GetPlayerConnectUserId(1) == playersArray[i].ConnectUserId) ? playerCards[1] : playerCardsOther[1],
                                        GetPlayerConnectUserId(2), (GetPlayerConnectUserId(2) == playersArray[i].ConnectUserId) ? playerCards[2] : playerCardsOther[2],
                                        GetPlayerConnectUserId(3), (GetPlayerConnectUserId(3) == playersArray[i].ConnectUserId) ? playerCards[3] : playerCardsOther[3]);
            }
        }

        private Suit GetSuit(string suitStr) {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                if (suit.ToString()[0] == suitStr[0])
                    return suit;
            return Suit.None;
        }
    }
}