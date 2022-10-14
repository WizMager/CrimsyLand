namespace Game.Interfaces
{
    public interface IDamageable
    {
        public float Health { get;}

        public void ChangeHealth(float value);
    }
}