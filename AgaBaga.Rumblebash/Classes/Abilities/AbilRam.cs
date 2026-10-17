namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilRam : Ability
    {
        public AbilRam() : base("Attacks in front when placed") { }
        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            bearer.targetSlot = svCardSlot[1 - position[0], position[1]];
            if (bearer.targetSlot != null && CheckForAbilStone(bearer.targetSlot))
            {
                bearer.CardStrike();
                await Task.Delay(300);
                bearer.targetSlot = null;
            }
        }
    }
}
