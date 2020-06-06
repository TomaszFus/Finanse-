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
        PayablesReceivable payablesReceivable = new PayablesReceivable();
        User user = new User();
        List<Transaction> transactionsList;


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
            Balance();
            ShowPayablesReceivable();



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
            ClearTrans();
            ShowTransListView();
            SetAvailableFunds();
            GetAvailableFunds();
            Balance();

        }

        private void Bt_ClearTrans_Click(object sender, RoutedEventArgs e)
        {
            ClearTrans();
        }

        /// <summary>
        /// Metoda wyswitlajaca szczegoly transakcji
        /// </summary>
        private void ListViewTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewTransaction.SelectedIndex >= 0)
            {
                var item = (ListBox)sender;
                var trans = (Transaction)item.SelectedItem;
                MessageBox.Show("Nazwa: " + trans.Name + " Kwota: " + trans.Amount + " Data: " + trans.Date.ToString("d") + " Opis: " + trans.Description);
            }

        }




        private void Bt_AddPR_Click(object sender, RoutedEventArgs e)
        {
            AddPR();
            ShowPayablesReceivable();
            ClearPR();
        }


        private void Bt_Remove_Click(object sender, RoutedEventArgs e)
        {
            RemovePR();
        }



        private void Bt_ClearPR_Click(object sender, RoutedEventArgs e)
        {
            ClearPR();
        }

        private void Bt_Calculate_Click(object sender, RoutedEventArgs e)
        {
            double amount=0;
            int time=0;
            double interest=0;

            if (String.IsNullOrWhiteSpace(TxB_DepositAmount.Text) || String.IsNullOrWhiteSpace(TxB_RateOfInterest.Text) || TxB_DepositPeriod.Text=="0")
            {
                MessageBox.Show("Podaj dane!");
            }
            else
            {
                Error1.Visibility = Error2.Visibility = Error3.Visibility = Visibility.Collapsed;
                try
                {
                    amount = double.Parse(TxB_DepositAmount.Text);
                }
                catch (Exception ex)
                {
                    Error1.Visibility = Visibility.Visible;
                    Error1.Content = "Nieprawidłowy format";
                    //throw;
                }
                try
                {
                    time = int.Parse(TxB_DepositPeriod.Text);
                }
                catch (Exception ex)
                {
                    Error2.Visibility = Visibility.Visible;
                    Error2.Content = ex.Message;
                    //throw;
                }

                try
                {
                    interest = double.Parse(TxB_RateOfInterest.Text);
                }
                catch (Exception ex)
                {
                    Error3.Visibility = Visibility.Visible;
                    Error3.Content = "Nieprawidłowy format";
                    //throw;
                }

                Investment(amount, time, interest);
            }
            
        }












        /////////////////////////////   METODY  ///////////////////////////// 




        /// <summary>
        /// Metoda wyświetlająca transakcje zalogowanego użytkownika w listview w transakcjach
        /// </summary>
        public void ShowTransListView()
        {
            using (FinanseEntities db = new FinanseEntities())
            {
                ListViewTransaction.ItemsSource = db.Transactions.Where(s=>s.UserID==User_ID).ToList();
                transactionsList= db.Transactions.Where(s => s.UserID == User_ID).ToList();
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


        ///<summary>
        /// Metoda czyszczaca pola w oknie transakcji
        /// </summary>
        public void ClearTrans()
        {
            TxB_Name.Text = TxB_Amount.Text = TxB_Desc.Text = null;
            RadioB_Expense.IsChecked = false;
            RadioB_Income.IsChecked = false;
        }


        /// <summary>
        /// Metoda wyswietlajaca dostepne srodki
        /// </summary>
        public void GetAvailableFunds()
        {
            try
            {
                using(FinanseEntities db=new FinanseEntities())
                {
                    user = db.Users.Where(i => i.ID_User == User_ID).FirstOrDefault();
                    Lb_AvFunds.Content = user.AvailableFunds.ToString();
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

        

        ///<summary>
        /// Metoda wyswietlajca bilans
        /// </summary>
        public void Balance()
        {
            double wydatki=0;
            double przychody=0;
            double bilans = 0;
            foreach (var item in transactionsList)
            {
                if(item.Type=="wydatek")
                {
                    wydatki += item.Amount;
                }
                else
                {
                    przychody += item.Amount;
                }
            }

            Lb_wydatki.Content = wydatki;
            Lb_przychody.Content = przychody;
            bilans = przychody - wydatki;
            Lb_bilans.Content = bilans;
        }


        ///<summary>
        ///Metoda dodająca należności i zobowiązania
        /// </summary>
        public void AddPR()
        {
            bool NameCheck = false;
            bool AmountCheck = false;
            bool TypeCheck = false;

            if (String.IsNullOrWhiteSpace(TxB_PR_Name.Text))
            {
                Lb_PRNameError.Content = "Podaj nazwe transakcji";
                Lb_PRNameError.Visibility = Visibility.Visible;
                NameCheck = false;
            }
            else
            {
                Lb_PRNameError.Visibility = Visibility.Collapsed;
                payablesReceivable.Name = TxB_PR_Name.Text.Trim();
                NameCheck = true;
            }


            if (String.IsNullOrWhiteSpace(TxB_PR_Amount.Text))
            {
                Lb_PRAmountError.Content = "Podaj kwotę transakcji";
                Lb_PRAmountError.Visibility = Visibility.Visible;
                AmountCheck = false;
            }
            else
            {
                Lb_PRAmountError.Visibility = Visibility.Collapsed;
                try
                {
                    payablesReceivable.Amount = double.Parse(TxB_PR_Amount.Text);
                    AmountCheck = true;
                }
                catch (Exception ex)
                {
                    Lb_PRAmountError.Content = ex.Message;
                    Lb_PRAmountError.Visibility = Visibility.Visible;
                    AmountCheck = false;
                    //throw;
                }

            }


            if (RadioB_PR_Receivable.IsChecked == true)
            {
                Lb_PRTypeError.Visibility = Visibility.Collapsed;
                payablesReceivable.Type = "należność";
                TypeCheck = true;
            }
            else if (RadioB_PR_Payables.IsChecked == true)
            {
                Lb_PRTypeError.Visibility = Visibility.Collapsed;
                payablesReceivable.Type = "zobowiązanie";
                TypeCheck = true;
            }
            else
            {
                Lb_PRTypeError.Content = "Dokonaj wyboru";
                Lb_PRTypeError.Visibility = Visibility.Visible;
                TypeCheck = false;
            }


            payablesReceivable.Description = TxB_Desc.Text;
            payablesReceivable.UserID = User_ID;
            payablesReceivable.Date = DateTime.Now;


            if (NameCheck && AmountCheck && TypeCheck == true)
            {

                try
                {
                    using (FinanseEntities db = new FinanseEntities())
                    {
                        db.PayablesReceivables.Add(payablesReceivable);
                        db.SaveChanges();
                    }
                    MessageBox.Show("Dodano poprawnie");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //throw;
                }

            }

        }

        ///<summary>
        ///Metoda czyszczaca pola w oknie naleznosci i zaobowiazan
        /// </summary>
        public void ClearPR()
        {
            TxB_PR_Name.Text = TxB_PR_Amount.Text = TxB_PR_Desc.Text = null;
            RadioB_PR_Payables.IsChecked = false;
            RadioB_PR_Receivable.IsChecked = false;
        }


        ///<summary>
        ///Metoda wyswietlajaca naleznosci i zobowiazania w listview
        /// </summary>
        public void ShowPayablesReceivable()
        {
            using (FinanseEntities db = new FinanseEntities())
            {
                ListViewPR.ItemsSource = db.PayablesReceivables.Where(s => s.UserID == User_ID).ToList();
                                
            }
        }


        public void RemovePR()
        {

            //if (ListViewTransaction.SelectedIndex >= 0)
            //{
            //    var item = (ListBox)sender;
            //    var trans = (Transaction)item.SelectedItem;
            //    MessageBox.Show("Nazwa: " + trans.Name + " Kwota: " + trans.Amount + " Data: " + trans.Date.ToString("d") + " Opis: " + trans.Description);
            //}

            if (ListViewPR.SelectedIndex>=0)
            {
                //Bt_Remove.IsEnabled =true;
                var pR = (PayablesReceivable)ListViewPR.SelectedItem;
                MessageBox.Show(pR.Name);
            }
            

            //ListViewPR.Items.Remove(ListViewPR.SelectedItem);
        }









        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="time"></param>
        /// <param name="rateOfInterest"></param>
        public void Investment(double amount, int time, double rateOfInterest)
        {
            double finalAmount = 0;
            double tax = 0;
            double interest = 0;
            finalAmount = amount+ ((amount * (rateOfInterest/100)) / 12) * time;
            interest = finalAmount - amount;
            tax=interest*0.19;
            finalAmount -= tax;


            TxB_SumPaid.Text = Math.Round(finalAmount,2).ToString();
            TxB_AmountInterest.Text = Math.Round(interest, 2).ToString();
            TxB_AmountTax.Text = Math.Round(tax, 2).ToString();

        }

        
    }
}
