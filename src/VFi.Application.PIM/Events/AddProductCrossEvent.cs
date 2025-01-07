using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using VFi.NetDevPack.Messaging;
using MediatR;

namespace VFi.Application.PIM.Events;

public class AddProductCrossEvent : Event
{
    public AddProductCrossEvent()
    {
        base.MessageType = GetType().Name;
    }

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string ProductType { get; set; }

    /// <summary>
    /// New = 0, Refurbished = 10, Used = 20, Damaged = 30
    /// </summary>
    public int Condition { get; set; }
    public string? UnitType { get; set; }
    public string? UnitCode { get; set; }
    public string Name { get; set; } = null!;
    public string SourceLink { get; set; }
    public string SourceCode { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? LimitedToStores { get; set; }


    public string? Channel { get; set; }
    public string? Channel_Category { get; set; }

    public string? Origin { get; set; }
    public string? Brand { get; set; }
    public string? Manufacturer { get; set; }
    public string? ManufacturerNumber { get; set; }
    public string? Image { get; set; }
    public string? Images { get; set; }
    public string? Gtin { get; set; }
    public decimal? ProductCost { get; set; }
    public string? CurrencyCost { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }

    public bool? IsTaxExempt { get; set; }
    public int? Tax { get; set; }


    public int? OrderMinimumQuantity { get; set; }
    public int? OrderMaximumQuantity { get; set; }


    public bool? IsShipEnabled { get; set; } = true;
    public bool? IsFreeShipping { get; set; } = false;



    public string? ProductTag { get; set; }
}
