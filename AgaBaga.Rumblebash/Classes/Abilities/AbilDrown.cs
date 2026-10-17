namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilDrown : Ability
    {
        public AbilDrown() : base("Drags monster in front to death when dying") { }
        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);

            Task.Delay(45);
            if (svCardSlot[1 - position[0], position[1]] != null && svCardSlot[1 - position[0], position[1]].slotCard.cHP > 0 
                && CheckForAbilStone(svCardSlot[1 - position[0], position[1]]))
            {
                svCardSlot[1 - position[0], position[1]].ChangeHP(-999, bearer, false);
                PlaySFX(Resource.Raw.HorseDieSFX, 130, 130);
            }
        }
    }
}
