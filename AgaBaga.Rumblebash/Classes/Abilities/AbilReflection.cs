namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilReflection : Ability
    {
        public AbilReflection() : base("Reflects dealt damage to target when hit or dying") { }

        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                bearer.StartAnimation(AnimationUtils.LoadAnimation(main, Resource.Animation.anim_AbilJump));
                bearer.targetSlot.CardHit(dmg, bearer);
            }
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                bearer.StartAnimation(AnimationUtils.LoadAnimation(main, Resource.Animation.anim_AbilJump));
                bearer.targetSlot.CardHit(dmg, bearer);
            }
        }
    }
}
