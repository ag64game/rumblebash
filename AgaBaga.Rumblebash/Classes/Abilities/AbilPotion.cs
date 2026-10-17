namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilPotion : Ability
    {
        public AbilPotion() : base("+1 AT to target when attacking") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            if (CheckForAbilStone(bearer.targetSlot))
            {
                bearer.targetSlot.ChangeAT(1, bearer);
                Animation aPowerjump = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_AbilJump);
                bearer.targetSlot.StartAnimation(aPowerjump);
            }
        }
    }
}
