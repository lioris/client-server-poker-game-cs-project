using Contracts;
using LinqToSql;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace client
{
    /// <summary>
    /// Interaction logic for table.xaml
    /// </summary>
    public partial class table : Window
    {
        public int tableID;
        public int seatid;
        public TableData table_data_binding { get; set; }
        Image[] comunitycards;
        Button[] sitBTNS;
        Image[] firstCard;
        Image[] secondeCard;
        Image[] profilePic;
        Label[] betLbl;

        public table(int tableId, TableData table_data)
        {
            InitializeComponent();

            lblPlayerTurn.Content = "no game";

            lblUserName.Content = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Name;
            lblMoney.Content = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Money;
            lblTableName.Content = table_data.TableName;

            tableID = tableId;

            comunitycards = new Image[5];
            comunitycards[0] = Flop1;
            comunitycards[1] = Flop2;
            comunitycards[2] = Flop3;
            comunitycards[3] = Tern;
            comunitycards[4] = River;

            sitBTNS = new Button[6];
            sitBTNS[0] = btnPlayer1;
            sitBTNS[1] = btnPlayer2;
            sitBTNS[2] = btnPlayer3;
            sitBTNS[3] = btnPlayer4;
            sitBTNS[4] = btnPlayer5;
            sitBTNS[5] = btnPlayer6;

            firstCard = new Image[6];
            secondeCard = new Image[6];

            firstCard[0] = card11; secondeCard[0] = card12;
            firstCard[1] = card21; secondeCard[1] = card22;
            firstCard[2] = card31; secondeCard[2] = card32;
            firstCard[3] = card41; secondeCard[3] = card42;
            firstCard[4] = card51; secondeCard[4] = card52;
            firstCard[5] = card61; secondeCard[5] = card62;

            profilePic = new Image[6];
            profilePic[0] = imgPlayer1;
            profilePic[1] = imgPlayer2;
            profilePic[2] = imgPlayer3;
            profilePic[3] = imgPlayer4;
            profilePic[4] = imgPlayer5;
            profilePic[5] = imgPlayer6;

            betLbl = new Label[6];
            betLbl[0] = lblBet1;
            betLbl[1] = lblBet2;
            betLbl[2] = lblBet3;
            betLbl[3] = lblBet4;
            betLbl[4] = lblBet5;
            betLbl[5] = lblBet6;

            table_data_binding = table_data;

            UpdataUI(table_data_binding);

            ClientCallBack.Instance.updateui += (sender, t_data) =>
            {
                Dispatcher.Invoke(() => UpdataUI(t_data));
            };

            ClientCallBack.Instance.chat_massage += (sender, massage_content) =>
            {
                Dispatcher.Invoke(() => updateChate(massage_content));
            };

        }

        public void updateChate(string massage)
        {
            txtChatLog.Text += massage;
        }

        public void UpdataUI(TableData data)
        {
            // BANK - V , Current Bet - V , CommunityCards - V, Seats - 
            table_data_binding = data;

            if (table_data_binding.seatTurn == seatid && !table_data_binding.Seats[seatid].isFolded)
            {
                btnCall.Visibility = System.Windows.Visibility.Visible;
                btnRaise.Visibility = System.Windows.Visibility.Visible;
                btnFold.Visibility = System.Windows.Visibility.Visible;
                txtMoneyToBet.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                btnCall.Visibility = System.Windows.Visibility.Hidden;
                btnRaise.Visibility = System.Windows.Visibility.Hidden;
                btnFold.Visibility = System.Windows.Visibility.Hidden;
                txtMoneyToBet.Visibility = System.Windows.Visibility.Hidden;
            }

            lblBank.Content = data.Bank.ToString();

            lblCurrentBet.Content = data.CurrentBet;
            lblPlayerTurn.Content = table_data_binding.seatTurn.ToString();

            // need to refactor
            lblMoney.Content = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Money;

            comunityCardsSet(data);

            sitBtnsSet(data);

            seatCardsSet(data);

            for (int i = 0; i < 6; i++)
            {
                if (data.Seats[i] != null)
                {
                    profilePic[i].Source = new BitmapImage(new Uri(Constants.DEFULT_PIC_PLAYER_PATH + (i + 1) + ".jpg", UriKind.Relative));
                    betLbl[i].Content = data.Seats[i].myBet.ToString();
                }
            }
        }

        private void sitBtnsSet(TableData data)
        {
            for (int i = 0; i < data.Seats.Length; i++)
            {
                if (data.Seats[i] != null)
                {
                    if (data.Seats[i].IsSeatTaken)
                        sitBTNS[i].Visibility = System.Windows.Visibility.Hidden;
                    else
                        sitBTNS[i].Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void seatCardsSet(TableData data)
        {
            for (int i = 0; i < firstCard.Length; i++)
            {
                if (data.Seats[i] != null)
                {
                    if (!data.Seats[i].isFolded)
                    {
                        firstCard[i].Source = new BitmapImage(new Uri(data.Seats[i].cards[0].getPath(), UriKind.Relative));
                        secondeCard[i].Source = new BitmapImage(new Uri(data.Seats[i].cards[1].getPath(), UriKind.Relative));
                    }
                    else
                    {
                        firstCard[i].Source = new BitmapImage(new Uri(Constants.DEFULT_PIC_CARD_PATH + Constants.DEFULT_PIC_CARD, UriKind.Relative));
                        secondeCard[i].Source = new BitmapImage(new Uri(Constants.DEFULT_PIC_CARD_PATH + Constants.DEFULT_PIC_CARD, UriKind.Relative));
                    }
                }
            }
        }

        private void comunityCardsSet(TableData data)
        {
            for (int i = 0; i < 5; i++)
                comunitycards[i].Source = new BitmapImage(new Uri(data.CommunityCards[i].getPath(), UriKind.Relative));
        }


        private void btn_sit(int seat)
        {
            string userName = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Name;
            int tableid = ClientCallBack.Instance.tableid;
            Proxy.Instance.GetProxy().Sit(tableid, seat, userName);

            this.seatid = seat;
        }

        private void btnPlayer1_Click(object sender, RoutedEventArgs e)
        {
            btn_sit(0);
        }

        private void btnPlayer2_Click(object sender, RoutedEventArgs e)
        {
            btn_sit(1);
        }

        private void btnPlayer3_Click(object sender, RoutedEventArgs e)
        {
            btn_sit(2);
        }

        private void btnPlayer4_Click(object sender, RoutedEventArgs e)
        {
            btn_sit(3);
        }

        private void btnPlayer5_Click(object sender, RoutedEventArgs e)
        {
            btn_sit(4);
        }

        private void btnPlayer6_Click(object sender, RoutedEventArgs e)
        {
            btn_sit(5);
        }


        private void btnSendMassage_Click(object sender, RoutedEventArgs e)
        {
            string name = ((User)Application.Current.Resources[Constants.CURRENT_USER]).Name;
            int tableid = ClientCallBack.Instance.tableid;
            Proxy.Instance.GetProxy().sendMassage(tableid, name + ": " + txtChatMassage.Text + "\n");
        }

        private void btnCall_Click(object sender, RoutedEventArgs e)
        {
            Proxy.Instance.GetProxy().Call(ClientCallBack.Instance.tableid, seatid);
        }

        private void btnRaise_Click(object sender, RoutedEventArgs e)
        {
            txtMoneyToBet.IsReadOnly = true;
            Proxy.Instance.GetProxy().Raise(ClientCallBack.Instance.tableid, seatid, int.Parse(txtMoneyToBet.Text));
        }

        private void btnFold_Click(object sender, RoutedEventArgs e)
        {
            Proxy.Instance.GetProxy().Fold(ClientCallBack.Instance.tableid, seatid);
        }

    }
}
