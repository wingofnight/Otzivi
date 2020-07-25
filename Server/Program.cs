using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace ChatServer
{
    class Program
    {
        static int port = 8005; // порт для приема входящих запросов
        static void Main(string[] args)
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(5);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных
                    
                        while (true)
                    {
                        builder.Clear();
                        do
                        {
                            bytes = handler.Receive(data);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (handler.Available > 0);

                        string mes = builder.ToString();
                        int index = mes.IndexOf(":");//определяем сколько знаков до символа
                        if (index == -1) continue;
                        string name = mes.Substring(0, index);//записываем кол-во символов из строки
                        mes = mes.Substring(index + 1);

                        index = name.IndexOf(";");
                        string fileName = name.Substring(0, index);
                        if (index == -1) continue;
                        name = name.Substring(index + 1);


                        using (StreamWriter file = new StreamWriter(fileName,true))//тру означает дозапись
                        file.WriteLine(DateTime.Now.ToShortTimeString() + ": " + name + ": " + mes );//записываем в файл

                        Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + name + ": " + mes );

                        // отправляем ответ
                        string message = "ваше сообщение доставлено";
                        data = Encoding.Unicode.GetBytes(message);
                        handler.Send(data);
                        if (mes == "стоп")
                        {
                            break;
                        }
                    }
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
    }
    }
}
