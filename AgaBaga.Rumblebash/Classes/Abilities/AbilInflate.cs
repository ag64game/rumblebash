namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilInflate : Ability
    {
        int charge = 0;
        public AbilInflate() : base("charges on stat change to release as AoE on death", chargeTxt: "Charge: 0") { }
        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            for (int i = 0; i < 3; i++) if (svCardSlot[1 - bearer.rotation, i].slotCard != null && svCardSlot[1 - bearer.rotation, i].slotCard.cHP > 0 && 
                CheckForAbilStone(svCardSlot[1 - bearer.rotation, i])) svCardSlot[1 - bearer.rotation, i].ChangeHP(-charge, bearer, false);
        }
        public override void OnUpdateAT(SlotView bearer, int amount, SlotView changer)
        {
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_AbilJump);
            bearer.StartAnimation(aNotifyUpdate);
            charge++;
            chargeTxt = "Charge: " + charge;
        }
        public override void OnUpdateHP(SlotView bearer, int amount, SlotView changer)
        {
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aNotifyUpdate);
            charge++;
            chargeTxt = "Charge: " + charge;
        }
        public override void OnUpdateSP(SlotView bearer, int amount, SlotView changer)
        {
            Animation aNotifyUpdate = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_textBounce);
            bearer.StartAnimation(aNotifyUpdate);
            charge++;
            chargeTxt = "Charge: " + charge;
        }
    }
}
