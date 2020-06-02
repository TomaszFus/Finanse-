using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Finanse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int User_ID; //id uzytkownika przekazane przez konstruktor
        Transaction transaction = new Transaction();
        User user = new User();


        public MainWindow(string UserLogin, int UserID)
        {
            InitializeComponent();
            Lb_LoginUser.Content ="Zalogowany użytkownik: "+UserLogin;
            User_ID = UserID;
             
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Grid1.Visibility = Visibility.Visible;
            Grid2.Visibility = Visibility.Hidden;
            Grid3.Visibility = Visibility.Hidden;

            ShowTransListView();
            GetAvailableFunds();
                                   

        }
        private void Bt_page1_Click(object sender, RoutedEventArgs e)
        {
            
            Grid1.Visibility = Visibility.Visible;
            Grid2.Visibility = Visibility.Hidden;
            Grid3.Visibility = Visibility.Hidden;
            
        }
        private void Bt_page2_Click(object sender, RoutedEventArgs e)
        {
            
            Grid2.Visibility = Visibility.Visible;
            Grid1.Visibility = Visibility.Hidden;
            Grid3.Visibility = Visibility.Hidden;
        }

        private void Bt_page3_Click(object sender, RoutedEventArgs e)
        {
            
            Grid3.Visibility = Visibility.Visible;
            Grid1.Visibility = Visibility.Hidden;
            Grid2.Visibility = Visibility.Hidden;
        }

        private void Bt_LogOut_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            Close();
            loginWindow.Show();
            
        }

        private void Bt_AddTrans_Click(object sender, RoutedEventArgs e)
        {
            AddTransaction();
            ShowTransListView();
            SetAvailableFunds();
            GetAvailableFunds();

        }





        /// <summary>
        /// Metoda wyświetlająca transakcje zalogowanego użytkownika w listview w transakcjach
        /// </summary>
        public void ShowTransListView()
        {
            using (FinanseEntities db = new FinanseEntities())
            {
                ListViewTransaction.ItemsSource = db.Transactions.Where(s=>s.UserID==User_ID).ToList();
            }
        }

        /// <summary>
        /// Metoda dodajaca transakcje
        /// </summary>
        public void AddTransaction()
        {
            bool NameCheck = false;
            bool AmountCheck = false;
            bool TypeCheck = false;

            
            if(String.IsNullOrWhiteSpace(TxB_Name.Text))
            {
                Lb_NameError.Content = "Podaj nazwe transakcji";
                Lb_NameError.Visibility = Visibility.Visible;
                NameCheck = false;
            }
            else
            {
                Lb_NameError.Visibility = Visibility.Collapsed;
                transaction.Name = TxB_Name.Text.Trim();
                NameCheck = true;
            }


            if(String.IsNullOrWhiteSpace(TxB_Amount.Text))
            {
                Lb_AmountError.Content = "Podaj kwotę transakcji";
                Lb_AmountError.Visibility = Visibility.Visible;
                AmountCheck = false;
            }
            else
            {
                Lb_AmountError.Visibility = Visibility.Collapsed;
                try
                {
                    transaction.Amount = double.Parse(TxB_Amount.Text);
                    AmountCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_AmountError.Content = ex.Message;
                    Lb_AmountError.Visibility = Visibility.Visible;
                    AmountCheck = false;
                    //throw;
                }
                
            }


            if(RadioB_Expense.IsChecked==true)
            {
                Lb_TypeError.Visibility = Visibility.Collapsed;
                transaction.Type = "wydatek";
                TypeCheck = true;
            }
            else if(RadioB_Income.IsChecked==true)
            {
                Lb_TypeError.Visibility = Visibility.Collapsed;
                transaction.Type = "przychod";
                TypeCheck = true;
            }
            else
            {
                Lb_TypeError.Content = "Dokonaj wyboru";
                Lb_TypeError.Visibility = Visibility.Visible;
                TypeCheck = false;
            }


            transaction.Description = TxB_Desc.Text;
            transaction.UserID = User_ID;
            transaction.Date = DateTime.Now;


            if(NameCheck && AmountCheck && TypeCheck==true)
            {
                
                try
                {
                 using(FinanseEntities db=new FinanseEntities())
                    {
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Transakcja dodana");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //throw;
                }
                
            }

        }


        /// <summary>
        /// Metoda wyswietlajaca dostepne sroki
        /// </summary>
        public void GetAvailableFunds()
        {
            try
            {
                using(FinanseEntities db=new FinanseEntities())
                {
                    user = db.Users.Where(i => i.ID_User == User_ID).FirstOrDefault();
                    txb_test.Text = user.AvailableFunds.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }


        /// <summary>
        /// Metoda modyfikujaca dostepne srodki dostepne srodki
        /// </summary>
        public void SetAvailableFunds()
        {
            double _avFunds = user.AvailableFunds;
            double _amount = transaction.Amount;
            double _newAFunds = 0;


            if (transaction.Type == "wydatek")
            {
                _newAFunds = _avFunds - _amount;
            }
            else
            {
                _newAFunds = _avFunds + _amount;
            }

            try
            {
                using (FinanseEntities db = new FinanseEntities())
                {
                    user=db.Users.Where(i => i.ID_User == User_ID).FirstOrDefault();
                    user.AvailableFunds = _newAFunds;
                    db.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }

        }

        private void ListViewTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("test");
        }
    }
}
