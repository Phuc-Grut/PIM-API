using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Events
{
    public class PublishProductTopicQueueEvent : QueueEvent
    {
        public PublishProductTopicQueueEvent()
        {
            base.MessageType = GetType().Name;
        }

        public Guid Id { get; set; } 
    }
}
