namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilWither : Ability
    {
        public AbilWither() : base("Loses 1 HP at the start of a turn") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if (bearer.slotCard.cHP > 1)
            {
                Animation aNotifyUpdate = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_MummyFade);
                bearer.StartAnimation(aNotifyUpdate);
            }
            bearer.ChangeHP(-1, bearer);
        }
    }
}
