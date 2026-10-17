namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilSteal : Ability
    {
        public AbilSteal() : base("Copies the ability in front when placed") { }
        public override async Task OnPlace(SlotView bearer)
        {

            int[] position = GetPositionInArray(bearer);

            if (svCardSlot[1 - position[0], position[1]] != null && !(svCardSlot[1 - position[0], position[1]].slotCard.cAbility is AbilSteal) && CheckForAbilStone(svCardSlot[1 - position[0], position[1]]))
            {
                Animation aLoadedAnimation;
                if (bearer.rotation == 0) aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation0);
                else aLoadedAnimation = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_StrikeRotation1);
                bearer.StartAnimation(aLoadedAnimation);
                bearer.slotCard.cAbility = (Ability)Activator.CreateInstance(svCardSlot[1 - position[0], position[1]].slotCard.cAbility.GetType());

                bearer.slotCard.cAbility.OnPlace(bearer);
            }
            else bearer.slotCard.cAbility = new AbilNone();
        }
    }
}
