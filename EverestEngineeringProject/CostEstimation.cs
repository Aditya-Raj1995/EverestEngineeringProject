using EverestEngineeringProject.Model;

namespace EverestEngineeringProject
{
    public class CostEstimation : ICostEstimation
    {
        private readonly Dictionary<string, Offer> _offers;

        public CostEstimation()
        {
            _offers = new List<Offer>
        {
            new("OFR001", 10, 0, 199, 70, 200),
            new("OFR002", 7, 50, 150, 100, 250),
            new("OFR003", 5, 50, 250, 10, 150)
        }.ToDictionary(o => o.Code);
        }

        public (decimal Discount, decimal TotalCost) Calculate(Package package, decimal baseCost)
        {
            decimal deliveryCost = baseCost + (package.Weight * 10) + (package.Distance * 5);
            decimal discountAmount = 0;
            if (_offers.TryGetValue(package.OfferCode, out var offer))
            {
                if (offer.IsEligible(package.Weight, package.Distance))
                {
                    discountAmount = deliveryCost * (offer.DiscountPercentage / 100);
                }
            }
            return (Math.Round(discountAmount, 2), Math.Round(deliveryCost - discountAmount, 2));
        }
    }
}

