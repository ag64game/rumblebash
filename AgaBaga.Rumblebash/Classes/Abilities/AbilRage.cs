namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilRage : Ability
    {
        public AbilRage() : base("+2 AT and +1 SP when hit") { }
        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            bearer.ChangeAT(2, bearer);
            bearer.ChangeSP(1, bearer);
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_AbilJump);
            bearer.StartAnimation(aNotifyUpdate);

            bearer.slotCard.aIdleAnimation.Duration /= 2;
        }
    }
}
