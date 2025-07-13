using HarmonyLib;
using RimWorld;
using Verse;

namespace BetterPawnControl
{
    [HarmonyPatch(typeof(Pawn_GuestTracker), nameof(Pawn_GuestTracker.SetGuestStatus))]
    static class Pawn_GuestTracker_SetGuestStatus
    {
        static void Postfix(Pawn pawn)
        {
            if (pawn == null) return;
            //became a new free colonist 
            if (pawn.IsFreeColonist 
                && !AssignManager.links.Exists(assignLink => assignLink?.colonist?.Equals(pawn) ?? false))
                AssignManager.SetDefaultsForFreeColonist(pawn);
            //former free colonist become prisoner
            if (pawn.IsPrisoner) AssignManager.SetDefaultsForPrisoner(pawn);
            //former free colonist become a slave
            if (pawn.IsSlave) AssignManager.SetDefaultsForSlave(pawn);
        }
    }

    [HarmonyPatch(typeof(Faction), nameof(Faction.Notify_PawnJoined))]
    static class Faction_Notify_PawnJoined
    {
        static void Postfix(Pawn pawn)
        {
            if (pawn == null) return;
            if (pawn.IsFreeColonist) AssignManager.SetDefaultsForFreeColonist(pawn);
            if (pawn.IsPrisoner) AssignManager.SetDefaultsForPrisoner(pawn);
            if (pawn.IsSlave) AssignManager.SetDefaultsForSlave(pawn);
        }
    }
}