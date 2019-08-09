using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//для OperationContext
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Wcf_chat
{
    [DataContract]
    public class ServerUser
    {
        //св-во поля id, id пользователя
        [DataMember]
        public int id { set; get; }
        
        //св-во поля Name. Имя пользователя
        [DataMember]
        public string Name { set; get; }

        //информация о подключении клиента к сервису
        public OperationContext operationContext { set; get; }
    }
}
