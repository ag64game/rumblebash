namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilUnleash : Ability
    {
        public AbilUnleash() : base("Attacks monsters granting it a stat boost") { }
        public override void OnUpdateAT(SlotView bearer, int amount, SlotView changer)
        {
            Task.Delay(200);
            Animation aLoadedAnimation;
            if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation0);
            else aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation1);
            bearer.StartAnimation(aLoadedAnimation);
            Task.Delay(185);
            changer.CardHit(bearer.slotCard.cAT, bearer);
        }

        public override void OnUpdateHP(SlotView bearer, int amount, SlotView changer)
        {
            Task.Delay(200);
            Animation aLoadedAnimation;
            if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation0);
            else aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation1);
            bearer.StartAnimation(aLoadedAnimation);
            Task.Delay(185);
            changer.CardHit(bearer.slotCard.cAT, bearer);
        }

        public override void OnUpdateSP(SlotView bearer, int amount, SlotView changer)
        {
            Task.Delay(200);
            Animation aLoadedAnimation;
            if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation0);
            else aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation1);
            bearer.StartAnimation(aLoadedAnimation);
            Task.Delay(185);
            changer.CardHit(bearer.slotCard.cAT, bearer);
        }
    }
}
