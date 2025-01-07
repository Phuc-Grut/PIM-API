using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Events
{
    public class AddProductTopicQueueEvent : QueueEvent
    {
        public AddProductTopicQueueEvent()
        {
            base.MessageType = GetType().Name;
        }

        public Guid Id { get; set; }
        public string Topic { get; set; }
    }
}
