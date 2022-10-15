namespace Game.Interfaces
{
    public interface IDamageable
    {
        public float Health { get;}

        public void ChangeHealthSend(float value);
    }
}