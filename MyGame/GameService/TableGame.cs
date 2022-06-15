using Contracts;
using System;
using System.Collections.Generic;
using System.Timers;

namespace GameService
{
    public enum Status
    {
        FLOP = 1,
        TERN = 2,
        RIVER = 3,
        END_GAME = 4
    }



    public class TableGame
    {
        public Dictionary<int, IClientCallBack> playing_list = new Dictionary<int, IClientCallBack>();
        public Dictionary<int, string> playing_names_list = new Dictionary<int, string>();
        public List<IClientCallBack> watching_list = new List<IClientCallBack>();

        private Deck deck;
        public TableData table_data = null;
        private Timer timer;

        string Name;
        int maxPlayers;
        public int currentPlaying;
        int bigBliend;

        Status status;
        int dealer;
        int playerBetTurn;

        public TableGame(string tableName, int numPlayers, int bigBliend)
        {
            this.Name = tableName;
            this.bigBliend = bigBliend;
            this.maxPlayers = numPlayers;
            this.currentPlaying = 0;
            this.status = Status.FLOP;
            table_data = new TableData(tableName, 0);
            dealer = 0;
            deck = new Deck();

            timer = new Timer(20000);
            timer.Elapsed += timer_Elapsed;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Run();
        }

        public bool addPlayer(string username, IClientCallBack callback, int seat, bool isPlaying)
        {
            if (isPlaying)
            {
                if (table_data.Seats[seat] != null)
                {
                    Console.WriteLine("seat taken");
                    return false;
                }

                table_data.Seats[seat] = new Seat(seat);
                playing_list.Add(seat, callback);
                playing_names_list.Add(seat, username);
                currentPlaying++;
            }
            else
                watching_list.Add(callback);


            return true;
        }

        public void sit(int seat, string username, IClientCallBack callback)
        {
            // remove from watching and put to playing list
            // give the sit
            lock (this)
            {

                if (table_data.Seats[seat] != null)
                {
                    Console.WriteLine("seat taken");
                    return;
                }

                table_data.Seats[seat] = new Seat(seat);

                watching_list.Remove(callback);
                playing_list.Add(seat, callback);
                playing_names_list.Add(seat, username);
                currentPlaying++;
            }
        }

        internal void Run()
        {
            if (playing_list.Count < 2)
            {
                Console.WriteLine("can not start a game, waiting");
                timer.Start();
            }
            else
            {
                smallBlindBet();
                bigBliendBet();
                dealHandCards();
                switchTurn();
                update();
            }
        }

        private void dealHandCards()
        {
            for (int i = 0; i < 6; i++)
            {
                if (table_data.Seats[i] != null)
                {
                    table_data.Seats[i].cards[0] = deck.getCard();
                    table_data.Seats[i].cards[1] = deck.getCard();
                }
            }
        }

        private void smallBlindBet()
        {
            playerBetTurn = nextPlayerIndex(dealer);
            bet(playerBetTurn, bigBliend / 2);
        }

        private void bigBliendBet()
        {
            playerBetTurn = nextPlayerIndex(playerBetTurn);
            bet(playerBetTurn, bigBliend);
        }

        private void switchTurn()
        {
            playerBetTurn = nextPlayerIndex(playerBetTurn);
            table_data.seatTurn = playerBetTurn;
        }

        private int nextPlayerIndex(int next)
        {
            next = (next + 1) % 6;
            while (!playing_list.ContainsKey(next))
                next = (next + 1) % 6;

            return next;
        }

        private void bet(int turn, int theBet)
        {
            table_data.Seats[turn].myBet += theBet;
            table_data.Bank += theBet;

            if (table_data.CurrentBet < theBet)
                table_data.CurrentBet = theBet;

            GameService.DBproxy.updataUserMoney(playing_names_list[turn], theBet);
        }

        private void update()
        {
            foreach (var players in playing_list)
                players.Value.updataTable(table_data);
            foreach (var watching in watching_list)
                watching.updataTable(table_data);
        }

        private void OpenCards()
        {
            switch (status)
            {
                case Status.FLOP:
                    Flop();
                    status = Status.TERN;
                    break;
                case Status.TERN:
                    Tern();
                    status = Status.RIVER;
                    break;
                case Status.RIVER:
                    River();
                    status = Status.END_GAME;
                    break;
                case Status.END_GAME:
                    win();
                    break;
                default:
                    break;
            }
        }

        private void Flop()
        {
            table_data.CommunityCards[Constants.FLOP1] = deck.getCard();
            table_data.CommunityCards[Constants.FLOP2] = deck.getCard();
            table_data.CommunityCards[Constants.FLOP3] = deck.getCard();
        }

        private void Tern()
        {
            table_data.CommunityCards[Constants.TERN] = deck.getCard();
        }

        private void River()
        {
            table_data.CommunityCards[Constants.RIVER] = deck.getCard();
        }

        private void win()
        {
            Console.WriteLine("the game has ended , need to impliment this and restart");
            //restart() and run();
        }

        private void isNextStep()
        {
            if (table_data.Seats[playerBetTurn].myBet == table_data.CurrentBet)
            {
                OpenCards();
                playerBetTurn = nextPlayerIndex(dealer);
                table_data.seatTurn = playerBetTurn;
            }
        }

        internal void Call(int seatid)
        {
            int toUpdate = table_data.CurrentBet - table_data.Seats[seatid].myBet;
            table_data.Seats[seatid].myBet = table_data.CurrentBet;
            table_data.Bank += table_data.CurrentBet;
            GameService.DBproxy.updataUserMoney(playing_names_list[seatid], toUpdate);

            switchTurn();

            isNextStep();
        }

        public void Raise(int seatid, int raise)
        {
            table_data.Seats[seatid].myBet += raise;
            table_data.CurrentBet = table_data.Seats[seatid].myBet;
            table_data.Bank += raise;

            GameService.DBproxy.updataUserMoney(playing_names_list[seatid], raise);

            switchTurn();
        }

        internal void Fold(int seatid)
        {
            table_data.Seats[seatid].isFolded = true;
            switchTurn();

            isNextStep();
        }
    }
}
