using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Wcf_chat
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceChat" в коде и файле конфигурации.


    //реализация интерфейса IserviceChat
    //один сервис для всех
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ServiceChat : IServiceChat
    {

        //список объектов (пользователей) типа ServerUser
        List<ServerUser> users = new List<ServerUser>();

        //переменная для генерации id пользователя
        int nextId = 1;

        //--------------------------------------------------------------------------------------------------------------------------

        //подключение пользователя к серверу
        public int Connect(string name, out List<ServerUser> userList)
        {
            ServerUser user = new ServerUser() // создаём нового пользователя и инициализируем его поля
            {
                id = nextId, // присваиваем новый id, новому пользователю
                Name = name, // присваиванием имени
                operationContext = OperationContext.Current // информация берется из данных подключения пользователя
            };
            //nextId++;
            SendMessage(" <" + user.Name + "> " + "подключился к чату", 0); // сообщение всем пользователям о том, что появился новый пользователь чата
            users.Add(user); //добавляем в список пользователей, нового пользователя
            Console.WriteLine("Пользователь: " + user.id + "; " + user.Name + "; " + user.operationContext + " подключился");
            showOnline(nextId);
            nextId++;
            userList = users;
            return user.id; // возвращаем уже сгенерированный id пользователя
        }

        //--------------------------------------------------------------------------------------------------------------------------

        //отключение пользователя
        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.id == id); // поиск id в списке users, который передаётся как параметр метода disconnect 
            if (user != null)
            {
                users.Remove(user);
                SendMessage(" <" + user.Name + "> " + "покинул чат", 0);
            }
            showOnline(id);
            Console.WriteLine("Пользователь: " + user.id + "; " + user.Name + "; " + user.operationContext + " отключился");
        }

        //--------------------------------------------------------------------------------------------------------------------------

        //личное сообщение
        public void PrivateSendMessage(string msg, int id, int needId)
        {
            foreach (var item in users)
            {
                if (item.id == needId)
                {
                    string answer = "[" + DateTime.Now.ToShortTimeString() + "]";
                    var user = users.FirstOrDefault(i => i.id == id);
                    if (user != null)
                    {
                        answer += " : <" + user.Name + "> ";
                    }
                    answer += msg;
                    item.operationContext.GetCallbackChannel<IServerChatCallBack>().PrivateMessageCallBack(answer);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------

        //формирование сообщения от сервера, всем пользователям 
        public void SendMessage(string msg, int id)
        {
            //string consoleTestStr = "";
            foreach (var item in users)
            {
                string answer = "[" + DateTime.Now.ToShortTimeString() + "]"; // время отправки сообщения

                var user = users.FirstOrDefault(i => i.id == id); // поиск id в списке users, который передаётся как параметр метода disconnect 
                if (user != null)
                {
                    answer += " : <" + user.Name + "> ";
                }
                answer += msg;
                //consoleTestStr = id.ToString() + " " + answer;
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().MessageCallBack(answer);               
            }
            //Console.WriteLine("Отработал SendMessage: " + consoleTestStr);

        }

        //---------------------------------------------------------------------------------------------------------------------------

        //формирование сообщения от сервера к конкретному пользователю
        /*
        public void SendPrivateMessage(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = "[" + DateTime.Now.ToShortTimeString() + "]"; // время отправки сообщения

                var user = users.FirstOrDefault(i => i.id == id); // поиск id в списке users, который передаётся как параметр метода disconnect 
                if (user != null)
                {
                    answer += " : <" + user.Name + "> ";
                }
                answer += msg;
                //consoleTestStr = id.ToString() + " " + answer;
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().PrivateMessageCallBack(answer);
            }
        }
        */

        //--------------------------------------------------------------------------------------------------------------------------

        //отображение пользователей в онлайне
        public void showOnline(int id)
        {
            /*
            foreach (var item in users)
            {
                item.operationContext.GetCallbackChannel<IServerChatCallBack>().ShowListUsers(users);
            }
            */
            foreach (var item in users)
                if (item.id != id)
                    item.operationContext.GetCallbackChannel<IServerChatCallBack>().ShowListUsers(users);
        }





    }
}
