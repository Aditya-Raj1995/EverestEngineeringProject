using EverestEngineeringProject;
using EverestEngineeringProject.Model;
class Program
{
    static void Main()
    {
        Console.WriteLine("Enter Base Delivery Cost and Number of Packages:");
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
        Console.WriteLine("\n");
        foreach (var item in packages)
        {
            Console.WriteLine($"{item.Pkg.Id} {item.Discount:F2} {item.Total:F2}");
        }
    }
}
