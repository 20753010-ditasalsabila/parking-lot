using System;
using System.Collections.Generic;
using System.Linq;

class Vehicle
{
    public string PoliceNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string policeNumber, string color, string type)
    {
        PoliceNumber = policeNumber;
        Color = color;
        Type = type;
    }
}

class ParkingSlot
{
    public int Number { get; }
    public bool IsFilled { get { return ParkedVehicle != null; } }
    public Vehicle ParkedVehicle { get; private set; }

    public ParkingSlot(int number)
    {
        Number = number;
    }

    public void Park(Vehicle vehicle)
    {
        ParkedVehicle = vehicle;
    }

    public void Leave()
    {
        ParkedVehicle = null;
    }
}

    class Program
    {
        static void Main(string[] args)
        {
             while (true)
        {
            Console.Write("$ ");
            string input = Console.ReadLine();
            if (input == null || input.Length == 0)
            {
                continue;
            }

            string[] command = input.Split(' ');

            switch (command[0].ToLower())
            {
                case "create_parking_lot":
                    int totalLots = int.Parse(command[1]);
                    parkingLot = new ParkingLot(totalLots);
                    Console.WriteLine($"Created a parking lot with {totalLots} slots");
                    break;

                case "park":
                    string policeNumber = command[1];
                    string color = command[command.Length - 2];
                    string vehicleType = command[command.Length - 1];
                    ParkVehicle(policeNumber, color, vehicleType);
                    break;

                case "leave":
                    int slotNumber = int.Parse(command[1]);
                    LeaveParking(slotNumber);
                    break;

                case "status":
                    StatusParking();
                    break;

                case "type_of_vehicles":
                    string type = command[1];
                    TypeOfVehicle(type);
                    break;

                case "exit":
                    Console.WriteLine("Program End.");
                    return;

                default:
                    Console.WriteLine("Invalid Command.");
                    break;
            }
        }
    }

    static void ParkVehicle(string policeNumber, string color, string vehicleType)
    {
        if (parkingLot == null)
        {
            Console.WriteLine("There is no parking space.");
            return;
        }

        ParkingSlot slot = parkingLot.ParkVehicle(policeNumber, color, vehicleType);
        if (slot != null)
        {
            Console.WriteLine($"Allocated slot number: {slot.Number}");
        }
        else
        {
            Console.WriteLine("Sorry, parking slots are full");
        }
    }

    static void LeaveParking(int slotNumber)
    {
        if (parkingLot == null)
        {
            Console.WriteLine("There is no parking space.");
            return;
        }

        bool success = parkingLot.Leave(slotNumber);
        if (success)
        {
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine($"There is no slot number {slotNumber} or it is already used");
        }
        }
    }

    static void StatusParking()
    {
        if (parkingLot == null)
        {
            Console.WriteLine("There is no parking space.");
            return;
        }

        Console.WriteLine("Slot\tRegistration No\tType\tColour");
        foreach (var slot in parkingLot.SlotFilled())
        {
            var vehicle = slot.ParkedVehicle;
            Console.WriteLine($"{slot.Number}\t{vehicle.PoliceNumber}\t{vehicle.Type}\t{vehicle.Color}");
        }
    }

    static void TypeOfVehicle(string type)
    {
        if (parkingLot == null)
        {
            Console.WriteLine("There is no parking space.");
            return;
        }

        int count = parkingLot.VehicleNumberType(type);
        Console.WriteLine(count);
    }
}

class ParkingLot
{
    private List<ParkingSlot> slots;

    public ParkingLot(int totalSlots)
    {
        slots = new List<ParkingSlot>();
        for (int i = 1; i <= totalSlots; i++)
        {
            slots.Add(new ParkingSlot(i));
        }
    }

    public ParkingSlot ParkVehicle(string policeNumber, string color, string type)
    {
        ParkingSlot slotAvailable = slots.FirstOrDefault(slot => !slot.IsFilled);
        if (slotAvailable != null)
        {
            slotAvailable.Park(new Vehicle(policeNumber, color, type));
        }
        return slotAvailable;
    }

    public bool Leave(int slotNumber)
    {
        ParkingSlot slot = NumberSlot(slotNumber);
        if (slot != null && slot.IsFilled)
        {
            slot.Leave();
            return true;
        }
        return false;
    }

    public IEnumerable<ParkingSlot> SlotFilled()
    {
        return slots.Where(slot => slot.IsFilled);
    }

    public int VehicleNumberType(string type)
    {
        return slots.Count(slot => slot.IsFilled && slot.ParkedVehicle.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
    }

    private ParkingSlot NumberSlot(int slotNumber)
    {
        return slots.FirstOrDefault(slot => slot.Number == slotNumber);
    }
}