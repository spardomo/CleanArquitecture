using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }

    public Order(int id, string customerName, string productName, int quantity, decimal unitPrice)
    {
        Id = id;
        CustomerName = customerName ?? string.Empty;
        ProductName = productName ?? string.Empty;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public decimal CalculateTotalAndLog()
    {
        return Quantity * UnitPrice; 
    }
}
