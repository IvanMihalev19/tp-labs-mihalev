namespace Lab4.Core;

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
            throw new ArgumentException("Название не может быть пустым", nameof(name));
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
        => $"{GetType().Name}: {Name} | {Price:C} | {PowerConsumption:F1} Вт | {GetSpecification()}";
}

public class Processor : Component
{
    public int Cores { get; }
    public double FrequencyGHz { get; }

    public Processor(string name, decimal price, double powerWatts, int cores, double frequencyGHz)
        : base(name, price, powerWatts)
    {
        if (cores <= 0)
            throw new ArgumentOutOfRangeException(nameof(cores));
        if (frequencyGHz <= 0)
            throw new ArgumentOutOfRangeException(nameof(frequencyGHz));

        Cores = cores;
        FrequencyGHz = frequencyGHz;
    }

    public override string GetSpecification()
        => $"{Cores} ядер @ {FrequencyGHz:F1} ГГц";
}

public class Memory : Component
{
    public int CapacityGb { get; }
    public string Type { get; }
    public int SpeedMhz { get; }

    public Memory(string name, decimal price, double powerWatts, int capacityGb, string type, int speedMhz)
        : base(name, price, powerWatts)
    {
        if (capacityGb <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacityGb));
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Тип памяти не может быть пустым", nameof(type));
        if (speedMhz <= 0)
            throw new ArgumentOutOfRangeException(nameof(speedMhz));

        CapacityGb = capacityGb;
        Type = type.Trim().ToUpperInvariant();
        SpeedMhz = speedMhz;
    }

    public override string GetSpecification()
        => $"{CapacityGb} ГБ {Type}-{SpeedMhz}";
}

public class Storage : Component
{
    public int CapacityGb { get; }
    public string DriveType { get; }
    public int ReadSpeedMbs { get; }

    public Storage(string name, decimal price, double powerWatts, int capacityGb, string driveType, int readSpeedMbs)
        : base(name, price, powerWatts)
    {
        if (capacityGb <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacityGb));
        if (string.IsNullOrWhiteSpace(driveType))
            throw new ArgumentException("Тип накопителя не может быть пустым", nameof(driveType));
        if (readSpeedMbs <= 0)
            throw new ArgumentOutOfRangeException(nameof(readSpeedMbs));

        CapacityGb = capacityGb;
        DriveType = driveType.Trim().ToUpperInvariant();
        ReadSpeedMbs = readSpeedMbs;
    }

    public override string GetSpecification()
        => $"{CapacityGb} ГБ {DriveType}, чтение {ReadSpeedMbs} МБ/с";
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
        => $"Сборка «{_name}»: {_components.Count} компонентов, стоимость {TotalPrice:C}, энергопотребление {TotalPowerConsumption:F0} Вт";
}