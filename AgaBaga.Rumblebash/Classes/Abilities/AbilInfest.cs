namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilInfest : Ability
    {
        int storeHP = 2, storeAT = 0, storeSP = 5;
        public AbilInfest() : base("Swaps stats with target on start of turn. Can't attack", canAttack: false, mustTarget: true) { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                Animation aLoadedAnimation;
                if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation0);
                else aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation1);

                storeHP = bearer.slotCard.cHP;
                storeAT = bearer.slotCard.cAT;
                storeSP = bearer.slotCard.cSP;

                bearer.SetHP(bearer.targetSlot.slotCard.cHP, bearer);
                bearer.SetAT(bearer.targetSlot.slotCard.cAT, bearer);
                bearer.SetSP(bearer.targetSlot.slotCard.cSP, bearer);

                bearer.targetSlot.SetHP(storeHP, bearer);
                bearer.targetSlot.SetAT(storeAT, bearer);
                bearer.targetSlot.SetSP(storeSP, bearer);
            }
        }
    }
}
