namespace Game.Interfaces
{
    public interface IMovable
    {
        public float MoveSpeed { get;}

        public void Move(float deltaTime);
    }
}