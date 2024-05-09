using Claims.Models;

namespace Claims.Application.Utils;

public class Premium
{
    private const decimal basePremiumPerDay = 1250m;

    public static decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        decimal multiplier = GetMultiplier(coverType);

        var premiumPerDay = basePremiumPerDay * multiplier;
        var totalDays = (endDate - startDate).TotalDays;

        // Calculate days in each period
        int daysInFirstPeriod = Math.Min(30, (int)totalDays);
        int daysInSecondPeriod = Math.Max(0, Math.Min(150, (int)totalDays - daysInFirstPeriod));
        int daysInThirdPeriod = Math.Max(0, (int)totalDays - daysInFirstPeriod - daysInSecondPeriod);

        // Calculate premium for each period
        decimal totalPremium = CalculatePremiumProgressively(coverType, premiumPerDay, daysInFirstPeriod, daysInSecondPeriod, daysInThirdPeriod);

        return totalPremium;
    }

    private static decimal CalculatePremiumProgressively(CoverType coverType, decimal premiumPerDay, int daysInFirstPeriod, int daysInSecondPeriod, int daysInThirdPeriod)
    {
        decimal totalPremium = daysInFirstPeriod * premiumPerDay;

        if (coverType == CoverType.Yacht)
        {
            totalPremium += daysInSecondPeriod * premiumPerDay * 0.95m; // Yacht 5% discount for the second period
            totalPremium += daysInThirdPeriod * premiumPerDay * 0.92m;  // Yacht 8% discount for the third period
        }
        else
        {
            totalPremium += daysInSecondPeriod * premiumPerDay * 0.98m; // Other types 2% discount for the second period
            totalPremium += daysInThirdPeriod * premiumPerDay * 0.97m;  // Other types 3% discount for the third period
        }

        return totalPremium;
    }

    private static decimal GetMultiplier(CoverType coverType)
    {
        var multiplier = coverType switch
        {
            CoverType.Yacht => 1.1m,// 10% more expensive
            CoverType.PassengerShip => 1.2m,// 20% more expensive
            CoverType.Tanker => 1.5m,// 50% more expensive
            _ => 1.3m,// Other types 30% more expensive
        };

        return multiplier;
    }
}

