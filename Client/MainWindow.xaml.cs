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
using System.Net;
using System.Net.Sockets;

namespace ServerChatX
{
    
    public partial class MainWindow : Window
    {
        static int port = 8005; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        static string FORM = "empty";
        static string messageClient = "empty";
        static string email = "empty";
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void Action()
        {
            // адрес и порт сервера, к которому будем подключаться
            
           
               
                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // подключаемся к удаленному хосту
                    socket.Connect(ipPoint);
                    
                  
                        string message = FORM + ";" +  email + ":" + messageClient;                    

                        byte[] data = Encoding.Unicode.GetBytes(message);
                        socket.Send(data);

                        // получаем ответ
                        data = new byte[256]; // буфер для ответа
                        StringBuilder builder = new StringBuilder();
                        int bytes = 0; // количество полученных байт

                        do
                        {
                            bytes = socket.Receive(data, data.Length, 0);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (socket.Available > 0);
                                        
                    // закрываем сокет
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            messageClient = txt_box.Text;
        }

     
        private void btn_good_Click(object sender, RoutedEventArgs e)
        {
            FORM = "GoodFoorm.ini";
            Action();
            Close();
        }

        private void btn_bad_Click(object sender, RoutedEventArgs e)
        {
            FORM = "BadFoorm.ini";
            Action();
            Close();
        }

        private void btn_gutton_Click(object sender, RoutedEventArgs e)
        {
            FORM = "medFoorm.ini";
            Action();
            Close();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            email = txt_email.Text;
        }
    }
}
