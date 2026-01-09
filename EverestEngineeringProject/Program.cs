using EverestEngineeringProject.Model;
using EverestEngineeringProject;

class Program
{
    static void Main()
    {
        Console.WriteLine("COURIER SERVICE\n");
        Console.WriteLine("Choose Problem:");
        Console.WriteLine("1. Calculate Delivery Cost with Offers");
        Console.WriteLine("2. Estimate Delivery Time");
        Console.Write("\nEnter choice (1 or 2): ");

        var choice = Console.ReadLine();
        if (choice == "1")
        {
            DeliveryCostCalculation();
        }
        else if (choice == "2")
        {
            DeliveryTimeEstimation();
        }
        else
        {
            Console.WriteLine("Invalid choice!");
        }
    }
    static void DeliveryCostCalculation()
    {
        Console.WriteLine("Enter Base Delivery Cost and Number of Packages (e.g., 100 3):");
        var input = Console.ReadLine()?.Split(' ');
        if (input == null || input.Length < 2) return;

        decimal baseCost = decimal.Parse(input[0]);
        int packageCount = int.Parse(input[1]);

        var calculator = new CostEstimation();
        var packages = new List<(Package Pkg, decimal Discount, decimal Total)>();

        for (int i = 0; i < packageCount; i++)
        {
            Console.WriteLine($"Enter Package {i + 1} details (ID Weight Distance OfferCode):");
            var pData = Console.ReadLine()?.Split(' ');

            var pkg = new Package
            {
                Id = pData[0],
                Weight = int.Parse(pData[1]),
                Distance = int.Parse(pData[2]),
                OfferCode = pData.Length > 3 ? pData[3] : ""
            };

            var (discount, total) = calculator.Calculate(pkg, baseCost);
            packages.Add((pkg, discount, total));
        }

        Console.WriteLine("\nOUTPUT");
        foreach (var item in packages)
        {
            Console.WriteLine($"{item.Pkg.Id} {item.Discount:F2} {item.Total:F2}");
        }
    }
    static void DeliveryTimeEstimation()
    {
        Console.WriteLine("Enter Base Cost and Number of Packages (e.g., 100 5):");
        var input1 = Console.ReadLine()?.Split(' ');
        if (input1 == null || input1.Length < 2) return;

        decimal baseCost = decimal.Parse(input1[0]);
        int packageCount = int.Parse(input1[1]);

        var packages = new List<Package>();
        var calculator = new CostEstimation();

        for (int i = 0; i < packageCount; i++)
        {
            Console.WriteLine($"Enter Package {i + 1} (ID Weight Distance OfferCode):");
            var pData = Console.ReadLine()?.Split(' ');

            packages.Add(new Package
            {
                Id = pData[0],
                Weight = int.Parse(pData[1]),
                Distance = int.Parse(pData[2]),
                OfferCode = pData.Length > 3 ? pData[3] : ""
            });
        }

        Console.WriteLine("Enter Number of Vehicles, Max Speed, Max Weight (e.g., 2 70 200):");
        var input2 = Console.ReadLine()?.Split(' ');
        if (input2 == null || input2.Length < 3) return;

        int numVehicles = int.Parse(input2[0]);
        int maxSpeed = int.Parse(input2[1]);
        int maxWeight = int.Parse(input2[2]);

        var results = new List<(string Id, decimal Discount, decimal Total, decimal DeliveryTime)>();

        foreach (var pkg in packages)
        {
            var (discount, total) = calculator.Calculate(pkg, baseCost);
            results.Add((pkg.Id, discount, total, 0));
        }

        var estimator = new DeliveryTimeCalculator(numVehicles, maxSpeed, maxWeight);
        var deliveryTimes = estimator.CalculateDeliveryTimes(packages);

        results = results.Select(r =>
            (r.Id, r.Discount, r.Total, deliveryTimes[r.Id])
        ).ToList();

        Console.WriteLine("\nOUTPUT");
        foreach (var result in results)
        {
            Console.WriteLine($"{result.Id} {result.Discount:F2} {result.Total:F2} {result.DeliveryTime:F2}");
        }
    }
}
