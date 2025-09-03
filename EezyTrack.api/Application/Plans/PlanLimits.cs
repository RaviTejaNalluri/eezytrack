using EezyTrack.api.Domain.Enums;

namespace EezyTrack.api.Application.Plans;

public static class PlanLimits
{
    public static int MaxMembers(PlanTier plan) => plan == PlanTier.Free ? 3 : 10;
    public static int MaxProjects(PlanTier plan) => plan == PlanTier.Free ? 1 : int.MaxValue;
}


