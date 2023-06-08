using System;
using System.Collections.Generic;

namespace DemoFBOS.Entity;

public partial class Meal
{
    public int MealId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
}
