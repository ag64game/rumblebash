namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilEmpower : Ability
    {
        public AbilEmpower() : base("Nearby allies gain +1 AT") { }
        public override async Task OnPlace(SlotView bearer)
        {
            Animation aPowerjump = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aPowerjump);
            int[] position = GetPositionInArray(bearer);
            if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] - 1])) svCardSlot[position[0], position[1] - 1].ChangeAT(1, bearer);
            if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] + 1])) svCardSlot[position[0], position[1] + 1].ChangeAT(1, bearer);
        }
    }
}
