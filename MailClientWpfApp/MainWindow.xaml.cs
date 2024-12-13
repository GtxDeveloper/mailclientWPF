using System.Net;
using System.Net.Mail;
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

namespace MailClientWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush _alertBrush;
        private Thickness _alertThickness;
        public MainWindow()
        {
            InitializeComponent();
            _alertThickness = new Thickness(2, 2, 2, 2);
            _alertBrush = new SolidColorBrush(Color.FromArgb( 200, (byte)227, (byte)79, (byte)57));
        }
        private void ShowAlertBox(TextBox box)
        {
            box.BorderThickness = _alertThickness;
            box.BorderBrush = new SolidColorBrush(Color.FromArgb(120, (byte)255, (byte)0, (byte)0)); ;
            box.Background = _alertBrush;
        }
        private void HideAlertBox(TextBox box)
        {
            box.BorderThickness = new Thickness(0);
            box.BorderBrush = null;
            box.Background = null;
        }
        private void Button_SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if(TextBox_Reciever.Text.Length <= 8)
            {
                ShowAlertBox(TextBox_Reciever);
                StatusBarItem_Info.Content = "Reciever email is to short";
                return;
            }
            HideAlertBox(TextBox_Reciever);
            // валидация!!!!
            try
            {
                // Провести валидацию полей
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(TextBox_Repeater.Text);
                mailMessage.To.Add(new MailAddress(TextBox_Reciever.Text));
                mailMessage.Subject = TextBox_Theme.Text;
                mailMessage.Body = TextBox_Message.Text;
                //mailMessage.Attachments
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.Port = Int32.Parse(TextBox_Port.Text);
                smtpClient.Credentials = new NetworkCredential(TextBox_Repeater.Text, PasswordBox_Repeater.Password);
                smtpClient.SendCompleted += SmtpClient_SendCompleted;
                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {
                StatusBarItem_Info.Content = ex.Message;
            }

            /*
                1. Реализовать возможность групповой рассылки
                2. Реализовать возможность отправки вложенных файлов(в теле письма)
                3*. Доделать программу чтобы все было хорошо.
             */
        }

        private void SmtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Email sended", "Cool", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}