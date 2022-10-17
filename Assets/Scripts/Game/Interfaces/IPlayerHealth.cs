using Photon.Pun;

namespace Game.Interfaces
{
    public interface IPlayerHealth : IHealth
    {
        public PhotonView PhotonView { get; }
    }
}