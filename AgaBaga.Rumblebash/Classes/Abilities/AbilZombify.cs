namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilZombify : Ability
    {
        public AbilZombify() : base("Turns attacked targets into Zombies") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            if (bearer.targetSlot.slotCard != null && bearer.targetSlot.slotCard.cHP > bearer.slotCard.cAT && CheckForAbilStone(bearer.targetSlot))
                bearer.targetSlot.PlaceCard(new Card("Zombie", 3, bearer.slotCard.cAT, bearer.slotCard.cSP, "ZombiePortrait", "ZombieMonster", "AbilZombify", "anim_IdleAnimationBreath", 3200));
        }
    }
}
