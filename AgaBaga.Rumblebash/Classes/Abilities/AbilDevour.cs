namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilDevour : Ability
    {
        public AbilDevour() : base("Attacks nearby allies at the start of a turn") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            PlaySFX(Resource.Raw.CardPickSFX);
            Animation stampede = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_cockatriceStampede);
            bearer.StartAnimation(stampede);
            await Task.Delay(195);

            int[] position = GetPositionInArray(bearer);
            if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] - 1]))
                svCardSlot[position[0], position[1] - 1].CardHit(bearer.slotCard.cAT, bearer);

            if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] + 1]))
                svCardSlot[position[0], position[1] + 1].CardHit(bearer.slotCard.cAT, bearer);
        }
    }
}
