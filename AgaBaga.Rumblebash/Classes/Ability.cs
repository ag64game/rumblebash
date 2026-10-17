namespace AgaBaga.Rumblebash.Classes
{
    public abstract class Ability
    {
        public string desc;
        public bool canAttack;
        public bool mustTarget;
        public string chargeTxt;
        public Ability(string desc, bool canAttack = true, bool mustTarget = false, string chargeTxt = null)
        {
            this.desc = desc;
            this.canAttack = canAttack;
            this.mustTarget = mustTarget;
            this.chargeTxt = chargeTxt;
        }
        public int[] GetPositionInArray(SlotView toCheck)
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++) if (svCardSlot[i, j] == toCheck) return new int[2] { i, j };
            return null;
        }

        //returns true if the checked card does not have AbilStone
        public static bool CheckForAbilStone(SlotView toCheck) => toCheck.slotCard.cAbility == null || !(toCheck.slotCard.cAbility is AbilStone);

        //Conditions under which abilities are activated
        public virtual async Task OnStrike(SlotView bearer, SlotView target = null, int dmg = 0) { }

        public virtual async Task OnHit(SlotView bearer, SlotView attacker = null, int dmg = 0) { }

        public virtual async Task OnKill(SlotView bearer, SlotView target = null, int dmg = 0) { }

        public virtual async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0) { }

        public virtual async Task OnStartOfTurn(SlotView bearer) { }

        public virtual async Task OnPlace(SlotView bearer) { }

        public virtual async Task OnDrawnFromDeck(Card bearer) { }

        public virtual void OnUpdateHP(SlotView bearer, int amount, SlotView changer) { }

        public virtual void OnUpdateAT(SlotView bearer, int amount, SlotView changer) { }

        public virtual void OnUpdateSP(SlotView bearer, int amount, SlotView changer) { }
    }
}
