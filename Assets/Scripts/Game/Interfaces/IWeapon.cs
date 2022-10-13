namespace Game.Interfaces
{
    public interface IWeapon
    {
        public int Damage { get; set; }
        public float ShootCooldown { get; set; }

        public void Shoot();
    }
}