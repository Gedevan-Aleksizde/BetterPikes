﻿using TaleWorlds.MountAndBlade;

namespace BetterPikes
{
    public class BetterPikesBehaviorCharge
    {
        // If the percentage of pikemen is above a certain limit, make the formation into a deep shield wall.
        // If the pikemen's enemies are not routing, make the pikemen walk.
        // If the pikemen's closest enemy formation is not cavalry, make the pikemen advance.
        protected static void Postfix(BehaviorCharge __instance, ref MovementOrder ____currentOrder)
        {
            Formation formation = __instance.Formation;
            FormationQuerySystem closestEnemyFormation = formation.QuerySystem.ClosestEnemyFormation;
            bool isPikeFormation = formation.CountOfUnits > 1 && formation.GetCountOfUnitsWithCondition(agent => agent.WieldedWeapon.CurrentUsageItem?.WeaponLength >= 400) >= formation.CountOfUnits * BetterPikesSettings.Instance.MinPikemenPercentInPikeFormation;
            if (isPikeFormation)
            {
                formation.ArrangementOrder = ArrangementOrder.ArrangementOrderShieldWall;
                formation.FormOrder = FormOrder.FormOrderDeep;
                if (closestEnemyFormation != null && !closestEnemyFormation.IsCavalryFormation)
                {
                    ____currentOrder = MovementOrder.MovementOrderAdvance;
                }
            }
            foreach (Agent agent in formation.UnitsWithoutLooseDetachedOnes)
            {
                agent.SetScriptedFlags(isPikeFormation && closestEnemyFormation?.Formation.CountOfUnits > 1 ? agent.GetScriptedFlags() | Agent.AIScriptedFrameFlags.DoNotRun : agent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.DoNotRun);
            }
        }
    }
}
