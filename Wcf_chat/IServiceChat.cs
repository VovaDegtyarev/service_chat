
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Wcf_chat
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IServiceChat" в коде и файле конфигурации.

    //основной функционал сервиса
     
    [ServiceContract(CallbackContract = typeof(IServerChatCallBack))]
    public interface IServiceChat
    {
        //метод с атрибутом OperationContract виден со стороны клиента; 
        [OperationContract]
        int Connect(string name, out List<ServerUser> userList); // - метод для подключения к сервису, результат работы метода это получение id пользователя


        [OperationContract(IsOneWay = true)]
        void Disconnect(int id); // - метод для отключения от сервиса(чата) пользователем


        //для того, чтобы не дожидаться ответа от сервера isOneWay = true
        [OperationContract(IsOneWay = true)]
        void SendMessage(string msg, int id); // - метод для отправки сообщения серверу *всем

        
        [OperationContract(IsOneWay = true)]
        void PrivateSendMessage(string msg, int id, int needId); // - метод для отправки сообщения серверу *лично пользователю
        
    }

    public interface IServerChatCallBack
    {
        //метод для обработки сообщения от сервиса, реализация будет сделана в клиенте (т.е. каким образов клиент будет получать сообщение от сервера)
        //вызов метода на стороне клиента со стороны сервера
        [OperationContract(IsOneWay = true)]
        void MessageCallBack(string msg); // метод рассылает сообщения всем клиентам

        //метод передаёт список пользователей в онлайне
        [OperationContract(IsOneWay = true)]
        void ShowListUsers(List<ServerUser> UserList);

        //личное сообщение выбранному пользователю, реализация для стороны клиента       
        [OperationContract(IsOneWay = true)]
        void PrivateMessageCallBack(string msg);
        
    }

}
