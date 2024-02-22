using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPF5.Models;
using WPF5.Windows;

namespace WPF5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            loginInput.Focus();
            timer.Tick += Timer_Tick;
        }
        private DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1),
            IsEnabled = false
        };

        private int blockTime = 10;
        string password_outputText = string.Empty;
        private bool isVisiblePassword = false;
        private void blockWindow()
        {
            IsEnabled = false;
            blockTime = 10;
            timer.Start();
        }

        private void unblockWindow()
        {
            timer.Stop();
            IsEnabled = true;
            buttonLogin.Content = "Вход в систему";
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (blockTime == 0)
            {
                unblockWindow();
                return;
            }

            buttonLogin.Content = blockTime.ToString();
            blockTime--;
        }
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            var context = new Fabric2Context();

            string currentCaptcha = captchaFirstBlock.Text + captchaSecondBlock.Text;


            if (captchaInput.Text.Trim() != currentCaptcha)
            {
                MessageBox.Show("Символы с картинки введены неверно! Окно будет заблокировано",
                    "Ошибка входа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                updateCaptcha();
                blockWindow();
                return;
            }

            if (!isVisiblePassword) textBox_passwordOutput.Text = passwordBox_passwordInput.Password;
            else passwordBox_passwordInput.Password = textBox_passwordOutput.Text;
           
            var user = context
              .Users
              .FirstOrDefault(u => u.UserLogin == loginInput.Text && u.UserPassword == textBox_passwordOutput.Text && u.UserPassword == passwordBox_passwordInput.Password);
            
            if (user is null)
            {
                MessageBox.Show("Неверный логин или пароль. Проверьте введенные данные",
                    "Ошибка входа",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                if (captchaContainer.Visibility != Visibility.Hidden)
                {
                    blockWindow();
                }
                updateFields();
                updateCaptcha();
                return;
            }
            else
            {
                var userStatus = user.UserRole;
                if (userStatus == 1)
                {
                    ProductsWindow window = new ProductsWindow(true);
                    window.Show();
                    Close();
                }
                else
                {
                    ProductsWindow window = new ProductsWindow(false);
                    window.Show();
                    Close();
                }
            }
           
        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            passwordBox_passwordInput.Visibility = Visibility.Collapsed;
            textBox_passwordOutput.Visibility = Visibility.Visible;
            textBox_passwordOutput.Text = passwordBox_passwordInput.Password;
            isVisiblePassword = true;
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordBox_passwordInput.Visibility = Visibility.Visible;
            textBox_passwordOutput.Visibility = Visibility.Collapsed;
            passwordBox_passwordInput.Password = textBox_passwordOutput.Text;
            isVisiblePassword = false;
        }

        private void updateFields()
        {
            loginInput.Text = string.Empty;
            passwordBox_passwordInput.Password = string.Empty;
            captchaInput.Text = string.Empty;
            textBox_passwordOutput.Text = string.Empty;
        }

        private void updateCaptcha()
        {
            const string alphabet = "qwertyuiopasdfghjklzxcvbnm1234567890";

            var rng = new Random();

            captchaFirstBlock.Text = $"{alphabet[rng.Next(alphabet.Length)]}{alphabet[rng.Next(alphabet.Length)]}";
            captchaSecondBlock.Text = $"{alphabet[rng.Next(alphabet.Length)]}{alphabet[rng.Next(alphabet.Length)]}";
            captchaSecondBlock.Margin = new Thickness(0, rng.Next(-10, 10), rng.Next(-10, 10), 0);
            captchaContainer.Visibility = Visibility.Visible;
        }

        private void button_passwordShow_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox_passwordInput.Password.ToString();
            MessageBox.Show($"{password}");
        }

        private void buttonLoginGuest_Click(object sender, RoutedEventArgs e)
        {
            ProductsWindow window = new ProductsWindow(false);
            window.Show();
            Close();
        }
    }
}