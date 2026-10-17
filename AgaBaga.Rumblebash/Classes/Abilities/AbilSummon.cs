namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilSummon : Ability
    {
        public AbilSummon() : base("Grants extra cards every turn per AT + 1. Can't attack", canAttack: false) { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            LoadCards(1 + bearer.slotCard.cAT);
        }
    }
}
