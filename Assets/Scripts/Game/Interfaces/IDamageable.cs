namespace Game.Interfaces
{
    public interface IDamageable
    {
        public float Health { get; set; }

        public void ReceiveDamage(float damage);
    }
}