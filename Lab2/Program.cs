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
    public class Memory : Component
    {
        private readonly int _capacityGb;
        private readonly string _type;
        private readonly int _speedMhz;

        public int CapacityGb => _capacityGb;
        public string Type => _type;
        public int SpeedMhz => _speedMhz;

        public Memory(string name, decimal price, double powerWatts, int capacityGb, string type, int speedMhz)
            : base(name, price, powerWatts)
        {
            if (capacityGb <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacityGb), "Объём памяти должен быть положительным");
            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("Тип памяти не может быть пустым", nameof(type));
            if (speedMhz <= 0)
                throw new ArgumentOutOfRangeException(nameof(speedMhz), "Частота памяти должна быть положительной");

            _capacityGb = capacityGb;
            _type = type.Trim().ToUpperInvariant();
            _speedMhz = speedMhz;
        }

        public override string GetSpecification()
            => $"{CapacityGb} ГБ {_type}-{SpeedMhz}";
    }
}
