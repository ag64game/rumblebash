namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilPoison : Ability
    {
        public AbilPoison() : base("Damages monster in front every round") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            Animation aPowerjump = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aPowerjump);
            int[] position = GetPositionInArray(bearer);
            if (svCardSlot[1 - position[0], position[1]] != null  && CheckForAbilStone(svCardSlot[1 - position[0], position[1]])) 
                svCardSlot[1 - position[0], position[1]].ChangeHP(-bearer.slotCard.cAT, bearer);
        }
    }
}
