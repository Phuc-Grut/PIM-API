
using VFi.Domain.PIM.Events;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces;

public partial interface IEventRepository
{
    Task<bool> AddProductCross(AddProductCrossQueueEvent message);
    Task<bool> AddProductTopic(AddProductTopicQueueEvent message);
    Task<bool> PublishProductTopicItem(PublishProductTopicQueueEvent message);
}
