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
    public class Storage : Component
    {
        private readonly int _capacityGb;
        private readonly string _driveType;
        private readonly int _readSpeedMbs;

        public int CapacityGb => _capacityGb;
        public string DriveType => _driveType;
        public int ReadSpeedMbs => _readSpeedMbs;

        public Storage(string name, decimal price, double powerWatts, int capacityGb, string driveType, int readSpeedMbs)
            : base(name, price, powerWatts)
        {
            if (capacityGb <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacityGb), "Объём накопителя должен быть положительным");
            if (string.IsNullOrWhiteSpace(driveType))
                throw new ArgumentException("Тип накопителя не может быть пустым", nameof(driveType));
            if (readSpeedMbs <= 0)
                throw new ArgumentOutOfRangeException(nameof(readSpeedMbs), "Скорость чтения должна быть положительной");

            _capacityGb = capacityGb;
            _driveType = driveType.Trim().ToUpperInvariant();
            _readSpeedMbs = readSpeedMbs;
        }

        public override string GetSpecification()
            => $"{CapacityGb} ГБ {_driveType}, чтение {ReadSpeedMbs} МБ/с";
    }
    public class Computer
    {
        private readonly List<Component> _components = new();
        private readonly string _name;

        public string Name => _name;
        public IReadOnlyList<Component> Components => _components.AsReadOnly();

        public Computer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название сборки не может быть пустым", nameof(name));
            _name = name.Trim();
        }

        public void AddComponent(Component component)
        {
            if (component is null)
                throw new ArgumentNullException(nameof(component));
            _components.Add(component);
        }
        public decimal TotalPrice => _components.Sum(c => c.Price);

        public double TotalPowerConsumption => _components.Sum(c => c.PowerConsumption);

        public override string ToString()
        {
            return $"Сборка «{_name}»: {_components.Count} компонентов, " +
                   $"стоимость {TotalPrice:C}, энергопотребление {TotalPowerConsumption:F0} Вт";
        }
    }
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var cpu1 = new Processor("AMD Ryzen 7 7800X3D", 34_990m, 120, 8, 4.2);
            var cpu2 = new Processor("Intel Core i5-14600K", 27_500m, 125, 14, 3.5);

            var ram1 = new Memory("Kingston Fury Beast", 8_990m, 5, 32, "DDR5", 6000);
            var ram2 = new Memory("Corsair Vengeance", 5_490m, 4, 16, "DDR4", 3200);

            var ssd = new Storage("Samsung 990 PRO", 12_490m, 6.5, 2000, "SSD", 7450);
            var hdd = new Storage("Seagate Barracuda", 4_990m, 8, 4000, "HDD", 190);

            List<Component> allComponents = [cpu1, cpu2, ram1, ram2, ssd, hdd];

            Console.WriteLine("=== Все компоненты (полиморфный вывод) ===");
            foreach (Component c in allComponents)
            {
                Console.WriteLine(c);
            }

            var pc1 = new Computer("Игровой ПК");
            pc1.AddComponent(cpu1);
            pc1.AddComponent(ram1);
            pc1.AddComponent(ssd);

            var pc2 = new Computer("Офисный ПК");
            pc2.AddComponent(cpu2);
            pc2.AddComponent(ram2);
            pc2.AddComponent(hdd);

            Console.WriteLine("\n=== Сборки ===");
            Console.WriteLine(pc1);
            Console.WriteLine(pc2);

            Console.WriteLine("\n=== Детализация сборки «Игровой ПК» ===");
            foreach (Component c in pc1.Components)
                Console.WriteLine($"  • {c}");

            Console.WriteLine($"\nИтоговая стоимость игрового ПК: {pc1.TotalPrice:C}");
            Console.WriteLine($"Суммарное энергопотребление: {pc1.TotalPowerConsumption:F0} Вт");

            Console.WriteLine("\n=== Проверка инкапсуляции (ожидаются исключения) ===");
            try
            {
                var bad = new Processor("Bad CPU", -1000, 100, 8, 3.5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Поймано: {ex.Message}");
            }

            try
            {
                var bad2 = new Memory("Bad RAM", 5000, 5, 0, "DDR5", 6000);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Поймано: {ex.Message}");
            }

            Console.WriteLine("\nГотово.");
        }
    }
}
