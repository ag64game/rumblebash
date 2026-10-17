namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilInvert : Ability
    {
        public AbilInvert() : base("Swaps attackers' HP and AT around when hit") { }
        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (attacker.slotCard.cAT > 0)
            {
                if (attacker != null && CheckForAbilStone(attacker))
                {
                    Animation aSlotSpin = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_invertigoBounce);
                    attacker.StartAnimation(aSlotSpin);
                    int storeHP = attacker.slotCard.cHP;
                    attacker.SetHP(attacker.slotCard.cAT, bearer);
                    attacker.SetAT(storeHP, bearer);
                }
            }
            else attacker.CardDead(attacker, 0);
        }
    }
}
