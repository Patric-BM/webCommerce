
using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public List<Product> products { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
}