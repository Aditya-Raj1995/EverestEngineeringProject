namespace EverestEngineeringProject.Model
{
    public record Offer(string Code, decimal DiscountPercentage, int MinDistance, int MaxDistance, int MinWeight, int MaxWeight)
    {
        public bool IsEligible(int weight, int distance) =>
            weight >= MinWeight && weight <= MaxWeight &&
            distance >= MinDistance && distance <= MaxDistance;
    }
}
