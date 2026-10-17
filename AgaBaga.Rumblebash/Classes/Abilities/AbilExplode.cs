namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilExplode : Ability
    {
        private bool exploded = false;
        public AbilExplode() : base("Deals 50% of taken DMG to adjacent allies on hit/death") { }

        public override async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);

            if (!exploded)
            {
                exploded = true;

                Animation aNotifyExplosion = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_LandmineSquash);
                aNotifyExplosion.Interpolator = new OvershootInterpolator();
                bearer.StartAnimation(aNotifyExplosion);

                int aoe;
                if (bearer.slotCard.cAT % 2 != 0) aoe = (dmg + 1) / 2; else aoe = (dmg) / 2;
                if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] - 1]))
                    svCardSlot[position[0], position[1] - 1].CardHit(aoe, attacker);

                if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] + 1]))
                    svCardSlot[position[0], position[1] + 1].CardHit(aoe, attacker);
            }

            exploded = false;
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);

            if (!exploded)
            {
                exploded = true;

                Animation aNotifyExplosion = AnimationUtils.LoadAnimation(main, Resource.Animation.anim_LandmineSquash);
                aNotifyExplosion.Interpolator = new OvershootInterpolator();
                bearer.StartAnimation(aNotifyExplosion);

                int aoe;
                if (bearer.slotCard.cAT % 2 != 0) aoe = (dmg + 1) / 2; else aoe = (dmg) / 2;
                if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] - 1]))
                    svCardSlot[position[0], position[1] - 1].CardHit(aoe, attacker);

                if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] + 1]))
                    svCardSlot[position[0], position[1] + 1].CardHit(aoe, attacker);
            }

            exploded = false;
        }
    }
}
