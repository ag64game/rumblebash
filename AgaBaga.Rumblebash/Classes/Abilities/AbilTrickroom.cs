namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilTrickroom : Ability
    {
        public AbilTrickroom() : base("On start of turn, Each monster swaps SP with it's opposing monster") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);

            for (int i = 0; i < 3; i++) if (svCardSlot[1 - position[0], i].slotCard != null && svCardSlot[position[0], i].slotCard != null)
            {
                int stored = svCardSlot[position[0], i].slotCard.cSP;
                svCardSlot[position[0], i].SetSP(svCardSlot[1 - position[0], i].slotCard.cSP, bearer);
                svCardSlot[1 - position[0], i].SetSP(stored, bearer);
            }
        }
    }
}
