namespace Game.Interfaces
{
    public interface IBullet
    {
        public float MoveSpeed { get;}
        public float MaxFlyDistance { get;}
        public float Damage { get;}

        public void Fly(float deltaTime);
    }
}