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
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название компонента не может быть пустым", nameof(name));
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Цена не может быть отрицательной");
        if (powerWatts < 0)
            throw new ArgumentOutOfRangeException(nameof(powerWatts), "Энергопотребление не может быть отрицательным");

        _name = name.Trim();
        _price = price;
        _powerWatts = powerWatts;
    }

    public abstract string GetSpecification();

    public override string ToString()
    {
        return $"{GetType().Name}: {Name} | {Price:C} | {PowerConsumption:F1} Вт | {GetSpecification()}";
    }
    public class Processor : Component
    {
        private readonly int _cores;
        private readonly double _frequencyGHz;

        public int Cores => _cores;
        public double FrequencyGHz => _frequencyGHz;

        public Processor(string name, decimal price, double powerWatts, int cores, double frequencyGHz)
            : base(name, price, powerWatts)
        {
            if (cores <= 0)
                throw new ArgumentOutOfRangeException(nameof(cores), "Количество ядер должно быть положительным");
            if (frequencyGHz <= 0)
                throw new ArgumentOutOfRangeException(nameof(frequencyGHz), "Частота должна быть положительной");

            _cores = cores;
            _frequencyGHz = frequencyGHz;
        }

        public override string GetSpecification()
            => $"{Cores} ядер @ {FrequencyGHz:F1} ГГц";
    }
}
