namespace PlcKafkaProducer.Models;

public class Drink
{
    public string Name { get; set; }
    public double Price { get; set; }

    public override string ToString()
    {
        return Name + " " + Price;
    }
}
