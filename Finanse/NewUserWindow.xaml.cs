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
using System.Windows.Shapes;


namespace Finanse
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    /// 

    

    public partial class NewUserWindow : Window
    {
        public NewUserWindow()
        {
            InitializeComponent();
            
        }

        User user = new User();
        List<User> usersList;


        
        /// <summary>
        /// Funkcja tworząca nowego użytkownika
        /// </summary>
        public void CreateUser()
        {
            user.FirstName = TxB_FirstName.Text.Trim();
            user.LastName = TxB_LastName.Text.Trim();

            bool LoginCheck = false;
            bool PassCheck = false;
            bool AvFundsCheck = false;

            if (String.IsNullOrWhiteSpace(TxB_AvailableFunds.Text))
            {
                TxBl_AvFunds.Text = "Wprowadź dostępne środki";
                TxBl_AvFunds.Visibility = Visibility.Visible;
                AvFundsCheck = false;
            }
            else
            {
                TxBl_AvFunds.Visibility = Visibility.Collapsed;
                try
                {
                    user.AvailableFunds = double.Parse(TxB_AvailableFunds.Text.Trim());
                    AvFundsCheck = true;
                }
                catch (Exception ex)
                {
                    TxBl_AvFunds.Text = ex.Message;
                    TxBl_AvFunds.Visibility = Visibility.Visible;
                    //throw ex;
                }
                
            }

            if (String.IsNullOrWhiteSpace(TxB_Login.Text))
            {
                TxBl_Login.Text = "Podaj login";
                TxBl_Login.Visibility = Visibility.Visible;
                LoginCheck = false;
            }
            else
            {   /////////////

                foreach (var item in usersList)
                {
                    if (item.Login == TxB_Login.Text)
                    {
                        TxBl_Login.Text = "Podany login jest zajęty";
                        TxBl_Login.Visibility = Visibility.Visible;
                        LoginCheck = false;
                        break;
                    }
                    else
                    {
                        TxBl_Login.Visibility = Visibility.Collapsed;
                        user.Login = TxB_Login.Text.Trim();
                        LoginCheck = true;
                    }



                }
                               
            }

            if (String.IsNullOrWhiteSpace(PassB_Password.Password))
            {
                TxBl_Password.Text = "Podaj hasło";
                TxBl_Password.Visibility = Visibility.Visible;
                PassCheck = false;
            }
            else
            {
                TxBl_Password.Visibility = Visibility.Collapsed;
                user.Password = PassB_Password.Password.Trim();
                PassCheck = true;
            }

            if(LoginCheck && PassCheck && AvFundsCheck == true)
            {
                using(FinanseEntities db=new FinanseEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                MessageBox.Show("Utworzono użytkownika");
                Clear();
            }
            

        }

        


        public void Clear()
        {
            TxB_FirstName.Text = TxB_LastName.Text = TxB_Login.Text = TxB_AvailableFunds.Text = PassB_Password.Password = null;
            TxBl_Login.Visibility = TxBl_Password.Visibility = TxBl_AvFunds.Visibility = Visibility.Collapsed;
           
        }

        private void Bt_Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Bt_Create_Click(object sender, RoutedEventArgs e)
        {
            CreateUser();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using(FinanseEntities db=new FinanseEntities())
            {
                usersList = db.Users.ToList();
            }
        }
    }
}
