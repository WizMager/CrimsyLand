using Photon.Pun;

namespace Game.Interfaces
{
    public interface IBonus
    {
        public PhotonView PhotonView { get;}

        public void ReceiveBonus(BonusType bonusType, int id);
    }
}