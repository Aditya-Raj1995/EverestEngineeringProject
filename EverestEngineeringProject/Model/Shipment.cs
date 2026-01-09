namespace EverestEngineeringProject.Model
{
    public class Shipment
    {
        public List<Package> Packages { get; set; } = new();
        public int TotalWeight => Packages.Sum(p => p.Weight);
        public int MaxDistance => Packages.Max(p => p.Distance);
    }
}
