namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilSnowstorm : Ability
    {
        const int REDUCTION_COUNT = 2;
        public AbilSnowstorm() : base("-" + REDUCTION_COUNT + " SP to all enemies when placed") { }

        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);

            for (int i = 0; i < 3; i++) if (svCardSlot[1 - position[0], i].slotCard != null) svCardSlot[1 - position[0], i].ChangeSP(-REDUCTION_COUNT, bearer);
        }
    }
}
