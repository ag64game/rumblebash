namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilScreech : Ability
    {
        public AbilScreech() : base("50% of AT splash damage") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer.targetSlot);

            try
            {
                int aoe;
                if (bearer.slotCard.cAT % 2 != 0) aoe = (dmg + 1) / 2; else aoe = (dmg) / 2;
                if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] - 1])) svCardSlot[position[0], position[1] - 1].CardHit(aoe, bearer);
                if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] + 1])) svCardSlot[position[0], position[1] + 1].CardHit(aoe, bearer);

                Console.WriteLine(bearer.slotCard.cName); //Ideally, The command should pike up an error if the bearer dies while the ability is active, forcing it to trigger targetSlot's CardHit
                Console.WriteLine(worksTxt + "Ability");
            }
            catch (Exception e)
            {
                svCardSlot[position[0], position[1]].CardHit(dmg, bearer);
            }
        }
    }
}
