namespace Game.Interfaces
{
    public interface IPlayer : IHealth
    {
        public void ReceiveBonus(BonusType bonusType);
    }
}