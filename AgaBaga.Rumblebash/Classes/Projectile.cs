using AgaBaga.Rumblebash.CustomComponenets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgaBaga.Rumblebash.Classes
{
    public class Projectile
    {
        public int turnsLeft;
        public int dmg;
        public SlotView targetSlot, sender;
        public Ability hitEffect = new AbilNone();

        public Projectile(int turnCount, int dmg, SlotView sender, SlotView targetSlot, Ability hitEffect = null)
        {
            this.turnsLeft = turnCount;
            this.dmg = dmg;
            this.sender = sender;
            this.targetSlot = targetSlot;
            if (hitEffect != null) this.hitEffect = hitEffect;
        }

        public async Task hit()
        {
            targetSlot.CardHit(dmg, sender);
        }
    }
}
