namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilGnome : Ability
    {
        SlotView prevTarget = null;
        bool alreadyAttacked = false;
        public AbilGnome() : base("Attacks both currently and previously targeted slots when striking") { }

        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            if (!alreadyAttacked)
            {
                SlotView curTarget = bearer.targetSlot;

                try
                {
                    await Task.Delay(15);
                    bearer.targetSlot = prevTarget;
                    alreadyAttacked = true;
                    bearer.CardStrike();
                    await Task.Delay(300);
                    if (curTarget.slotCard != null) bearer.targetSlot = curTarget; else bearer.targetSlot = null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(errTxt);
                    Console.WriteLine(e.ToString());
                }

                prevTarget = curTarget;
                chargeTxt = "Targeting slot No. " + (GetPositionInArray(prevTarget)[1] + 1);
                alreadyAttacked = false;
            }
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (bearer.Animation != null) bearer.ClearAnimation();
        }
    }
}
