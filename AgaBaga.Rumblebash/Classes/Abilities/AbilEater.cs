namespace AgaBaga.Rumblebash.Classes.Abilities
{
    public class AbilEater : Ability
    {
        public AbilEater() : base("Only attacks once every other turn") { }
        public override async Task OnStartOfTurn(SlotView bearer)
        {
            if (canAttack)
            {
                canAttack = false;
                chargeTxt = "Resting";
                bearer.slotCard.aIdleAnimation.Duration *= 10;
            }
            else
            {
                bearer.slotCard.aIdleAnimation.Duration /= 10;
                canAttack = true;
                chargeTxt = null;
            }
        }
    }
}
