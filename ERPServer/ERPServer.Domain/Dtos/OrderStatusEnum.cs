using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPServer.Domain.Dtos;

public sealed class OrderStatusEnum : SmartEnum<OrderStatusEnum>
{
    public static readonly OrderStatusEnum Pending = new OrderStatusEnum("Bekliyor", 1);
    public static readonly OrderStatusEnum RequirementsPlanWorked = new OrderStatusEnum("İhtiyaç planı çalışıldı", 2);
    public static readonly OrderStatusEnum Completed = new OrderStatusEnum("Tamamlandı", 3);

    public OrderStatusEnum(string name, int value) : base(name, value)
    {
    }
}
