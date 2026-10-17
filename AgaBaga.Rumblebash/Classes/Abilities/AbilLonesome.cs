namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilLonesome : Ability
    {
        private int addedAT = 0, oldAT = 0;

        public AbilLonesome() : base("Gets stronger for empty nearby ally slots") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);
            addedAT = 0;
            if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard == null) addedAT++;
            if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard == null) addedAT++;
            bearer.ChangeAT(addedAT - oldAT, bearer);
            oldAT = addedAT;
        }
    }
}
