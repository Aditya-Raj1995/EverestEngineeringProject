using EverestEngineeringProject.Model;

namespace EverestEngineeringProject
{
    public class DeliveryTimeCalculator
    {
        private readonly int _numberOfVehicles;
        private readonly int _maxSpeed;
        private readonly int _maxWeight; 

        public DeliveryTimeCalculator(int numberOfVehicles, int maxSpeed, int maxWeight)
        {
            _numberOfVehicles = numberOfVehicles;
            _maxSpeed = maxSpeed;
            _maxWeight = maxWeight;
        }

        public Dictionary<string, decimal> CalculateDeliveryTimes(List<Package> packages)
        {
            // Step 1: Create optimal shipments (maximize packages per trip)
            var shipments = CreateOptimalShipments(packages);

            // Step 2: Schedule shipments and calculate delivery times
            return ScheduleShipments(shipments);
        }

        // Create shipments by grouping packages efficiently
        private List<Shipment> CreateOptimalShipments(List<Package> packages)
        {
            var shipments = new List<Shipment>();
            var remainingPackages = new List<Package>(packages);

            // Sort packages: heaviest first, then by distance (shortest first for ties)
            remainingPackages = remainingPackages
                .OrderByDescending(p => p.Weight)
                .ThenBy(p => p.Distance)
                .ToList();

            while (remainingPackages.Any())
            {
                var shipment = new Shipment();
                var currentPackage = remainingPackages[0];
                shipment.Packages.Add(currentPackage);
                remainingPackages.RemoveAt(0);

                // Try to add more packages to this shipment
                for (int i = remainingPackages.Count - 1; i >= 0; i--)
                {
                    if (shipment.TotalWeight + remainingPackages[i].Weight <= _maxWeight)
                    {
                        shipment.Packages.Add(remainingPackages[i]);
                        remainingPackages.RemoveAt(i);
                    }
                }
                shipments.Add(shipment);
            }
            return shipments;
        }

        // Schedule shipments across vehicles and calculate delivery times
        private Dictionary<string, decimal> ScheduleShipments(List<Shipment> shipments)
        {
            var deliveryTimes = new Dictionary<string, decimal>();
            var vehicleAvailableTimes = new decimal[_numberOfVehicles]; // When each vehicle becomes available

            foreach (var shipment in shipments)
            {
                // Find the vehicle that will be available first
                int vehicleIndex = 0;
                decimal earliestTime = vehicleAvailableTimes[0];

                for (int i = 1; i < _numberOfVehicles; i++)
                {
                    if (vehicleAvailableTimes[i] < earliestTime)
                    {
                        earliestTime = vehicleAvailableTimes[i];
                        vehicleIndex = i;
                    }
                }

                // Calculate delivery time for this shipment
                decimal deliveryTime = (decimal)shipment.MaxDistance / _maxSpeed;
                decimal totalTripTime = deliveryTime * 2; // Round trip

                // Assign delivery times to all packages in this shipment
                foreach (var package in shipment.Packages)
                {
                    deliveryTimes[package.Id] = Math.Round(earliestTime + deliveryTime, 2);
                }

                // Update vehicle availability
                vehicleAvailableTimes[vehicleIndex] = earliestTime + totalTripTime;
            }
            return deliveryTimes;
        }
    }
}

