namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilMirror : Ability
    {
        int tAT = 0;
        public AbilMirror() : base("+1 AT to monster in front and copies new AT on place") { }
        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            if (svCardSlot[1 - position[0], position[1]] != null && CheckForAbilStone(svCardSlot[1 - position[0], position[1]]))
            {
                svCardSlot[1 - position[0], position[1]].ChangeAT(1, bearer);
                tAT = svCardSlot[1 - position[0], position[1]].slotCard.cAT;
            }
            bearer.SetAT(tAT, bearer);

        }
    }
}
