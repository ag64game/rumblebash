namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilMeteor : Ability
    {
        const int REQUIRED_KILLS = 3;
        int charge = 0;
        public AbilMeteor() : base("Board wipes after " + REQUIRED_KILLS + " kills", chargeTxt: "Kills left: " + REQUIRED_KILLS) { }
        public override async Task OnKill(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            charge++;
            if (charge >= REQUIRED_KILLS)
            {
                for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++)
                    if (svCardSlot[i, j].slotCard != null && svCardSlot[i, j] != bearer && CheckForAbilStone(svCardSlot[i, j]))
                        svCardSlot[i, j].ChangeHP(-999, bearer, false);

                PlaySFX(Resource.Raw.WarlockMeteorSFX, 115, 115);
                charge = 0;
            }
            chargeTxt = "Kills left: " + (REQUIRED_KILLS - charge);
        }
    }
}
