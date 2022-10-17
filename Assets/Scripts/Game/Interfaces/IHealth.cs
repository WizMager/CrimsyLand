namespace Game.Interfaces
{
    public interface IHealth
    {
        public float Health { get;}

        public void ChangeHealth(float value);
    }
}