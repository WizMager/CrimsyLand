using Photon.Pun;

namespace Game.Interfaces
{
    public interface IEnemy : IMovable
    {
        public PhotonView PhotonView { get; }
        public void ChangeHealth(float value, int id);
    }
}