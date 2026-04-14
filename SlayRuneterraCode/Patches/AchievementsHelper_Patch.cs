using HarmonyLib;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Models;

namespace SlayRuneterra.Patches;


// Canys patch
[HarmonyPatch(typeof(AchievementsHelper), nameof(AchievementsHelper.CheckForDefeatedAllEnemiesAchievement))]
public class SkipModdedActAchievementPatch
{
    public static bool Prefix(ActModel act)
    {
        return act is not Content.Acts.Demacia;
    }
}