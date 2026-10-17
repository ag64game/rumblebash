namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilLeech : Ability
    {
        public AbilLeech() : base("+2 HP on kill") { }
        public override async Task OnKill(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            Animation aPowerjump = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_AbilJump);
            bearer.StartAnimation(aPowerjump);
            bearer.slotCard.cHP += 2;
            bearer.tvDisplayStats[1].Text = Convert.ToString(bearer.slotCard.cHP);
        }
    }
}
