using EverestEngineeringProject.Model;

namespace EverestEngineeringProject
{
    public interface ICostEstimation
    {
        (decimal Discount, decimal TotalCost) Calculate(Package package, decimal baseCost);
    }
}
