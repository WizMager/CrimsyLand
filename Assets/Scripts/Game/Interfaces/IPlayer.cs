namespace Game.Interfaces
{
    public interface IPlayer : IDamageable
    {
        public void ReceiveBonus(BonusType bonusType);
    }
}