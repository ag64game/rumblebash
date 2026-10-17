namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilCauldron : Ability
    {
        const int REQUIRED_TURNS = 2;
        int charge = REQUIRED_TURNS;
        public AbilCauldron() : base("Drops a 10 DMG cauldron on slot in front of it after " + REQUIRED_TURNS + " turns", chargeTxt: "Turns Left: " + REQUIRED_TURNS) { }

        public override async Task OnPlace(SlotView bearer)
        {
            int[] position = GetPositionInArray(bearer);
            SlotView cauldronTarget = svCardSlot[1 - position[0], position[1]];

            if (CheckForAbilStone(cauldronTarget)) lProjectileBank.Add(new Projectile(REQUIRED_TURNS, 10, bearer, cauldronTarget));
            else charge = -1;
        }

        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if (charge > 0)
            {
                charge--;
                if (charge == 0) chargeTxt = null;
                else chargeTxt = "Turns left: " + charge;
            }
        }
    }
}
