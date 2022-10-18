using Photon.Pun;

namespace Game.Interfaces
{
    public interface IBonusReceiver
    {
        public PhotonView PhotonView { get;}

        public void ReceiveBonus(int id, int bonusType);
    }
}