namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilFish : Ability
    {
        const int MAX_FISH = 6;
        int charge = 6;
        public AbilFish() : base("Respawns in deck after death with more stats. Up to " + MAX_FISH + " times") { }
        public override async Task OnDrawnFromDeck(Card bearer)
        {
            this.charge = MAX_FISH + 1 - bearer.cHP;
            if (bearer.cName == "Fin-Inito") chargeTxt = "Fish Left: " + charge;
            else chargeTxt = bearer.cName + " Left: " + charge;
        }

        public override async Task OnDie(SlotView bearer, SlotView attacker = null, int dmg = 0)
        {
            if (charge > 1)
            {
                int newStat = Math.Abs(MAX_FISH + 2 - charge);
                Card[] newCards = { new Card(bearer.slotCard.cPortrait, bearer.slotCard.cMonster, bearer.slotCard.cName, newStat, newStat, newStat, "AbilFish", bearer.slotCard.aIdleAnimation) };
                LoadCards(content: newCards);
            }
        }
    }
}
