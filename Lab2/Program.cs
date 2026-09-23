using System;
using System.Collections.Generic;
using System.Linq;

 namespace Lab2;
 public abstract class Component
{
    private readonly string _name;
    private readonly decimal _price;
    private readonly double _powerWatts;

    public string Name => _name;
    public decimal Price => _price;
    public double PowerConsumption => _powerWatts;

    protected Component(string name, decimal price, double powerWatts)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException("Название компонента не может быть пустым", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Цена не может быть отрицательной");
        if (powerWatts < 0)
            throw new ArgumentOutOfRangeException(nameof(powerWatts), "Потребление не может быть отрицательным");

        _name = name;
        _price = price;
        _powerWatts = powerWatts;
    }

    public abstract string GetSpecification();
    public override string ToString()
    {
        return $"{GetType().Name}: {Name} | {Price:C} | {PowerConsumption:F1} Вт | {GetSpecification()}";
    }


}