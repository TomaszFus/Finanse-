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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        User user = new User();

        

        /// <summary>
        /// Funkcja logujaca użytkownika do programu 
        /// </summary>
        public void LogIn()
        {
            using(FinanseEntities db=new FinanseEntities())
            {
                user = db.Users.FirstOrDefault(p => p.Login == TxB_Login.Text);
                if(user is null)
                {
                    TxBl_Login.Text = "Błędny login";
                    TxBl_Login.Visibility = Visibility.Visible;
                }
                else
                {
                    TxBl_Login.Visibility = Visibility.Collapsed;
                    if(user.Password==PassB_Passowrd.Password)
                    {
                        string UserLogin = user.Login;  //zmienna przekazujaca login użytkownika do okna głównego
                        int UserID = user.ID_User;  //zmienna przekazujaca id użytkownika do okna głównego
                        TxBl_Password.Visibility = Visibility.Collapsed;
                        MessageBox.Show("Witaj "+user.FirstName);
                        MainWindow mainWindow = new MainWindow(UserLogin, UserID);
                        mainWindow.Show();
                        Close();
                        
                    }
                    else
                    {
                        TxBl_Password.Text = "Błędne hasło";
                        TxBl_Password.Visibility = Visibility.Visible;
                    }
                }
            }

        }
        public LoginWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// zdarzenie obslugujace klikniecie prezycisku tworzenia nowego konta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_CreateNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow();
            newUserWindow.ShowDialog();
        }

        /// <summary>
        /// zdarzenie obslugujace klikniecie prezycisku logowania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bt_LogIn_Click(object sender, RoutedEventArgs e)
        {
            LogIn();
        }
    }
}
