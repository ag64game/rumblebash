namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilChain : Ability
    {
        public AbilChain() : base("Attacks hurt the monster in front of the target") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer.targetSlot);
            int bearerAT = bearer.slotCard.cAT;

            if (svCardSlot[1 - position[0], position[1]].slotCard != null && CheckForAbilStone(svCardSlot[1 - position[0], position[1]]))
            {
                if (svCardSlot[1 - position[0], position[1]] == bearer) bearer.targetSlot.CardHit(bearer.slotCard.cAT, bearer);

                svCardSlot[1 - position[0], position[1]].ChangeHP(-bearerAT, bearer, false);

                if (svCardSlot[1 - position[0], position[1]].slotCard.cAbility != null)
                    svCardSlot[1 - position[0], position[1]].slotCard.cAbility.OnHit(svCardSlot[1 - position[0], position[1]], bearer);
            }
        }
    }
}
