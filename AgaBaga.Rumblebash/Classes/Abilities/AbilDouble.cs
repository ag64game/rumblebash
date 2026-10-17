namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilDouble : Ability
    {
        public AbilDouble() : base("Attacks strike again at the start of the next turn") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                lProjectileBank.Add(new Projectile(1, bearer.slotCard.cAT, bearer, bearer.targetSlot));
            }
        }
    }
}
