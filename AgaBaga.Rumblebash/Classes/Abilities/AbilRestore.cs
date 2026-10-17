using System;
using System.Collections.Generic;
using System.Text;

namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilRestore : Ability
    {
        public AbilRestore() : base("Heals nearby allies when attacking") { }
        public override async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0)
        {
            int[] position = GetPositionInArray(bearer);
            if (position[1] > 0 && svCardSlot[position[0], position[1] - 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] - 1]))
                svCardSlot[position[0], position[1] - 1].ChangeHP(bearer.slotCard.cAT, bearer);

            if (position[1] < 2 && svCardSlot[position[0], position[1] + 1].slotCard != null && CheckForAbilStone(svCardSlot[position[0], position[1] + 1]))
                svCardSlot[position[0], position[1] + 1].ChangeHP(bearer.slotCard.cAT, bearer);
        }
    }
}
