using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using VFi.NetDevPack.Messaging;
using MediatR;

namespace VFi.Application.PIM.Events;

public class AddProductTopicEvent : Event
{
    public AddProductTopicEvent()
    {
        base.MessageType = GetType().Name;
    }
    public Guid Id { get; set; }
    public string Topic { get; set; } 
}