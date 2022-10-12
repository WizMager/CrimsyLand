using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private Button createGame;
        [SerializeField] private Button findGame;
        [SerializeField] private Button exit;

        private void Awake()
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            createGame.onClick.AddListener(OnCreateGameHandler);
            findGame.onClick.AddListener(OnFindGameHandler);
            exit.onClick.AddListener(OnExitHandler);
            createGame.interactable = false;
            findGame.interactable = false;
            statusText.text = "Connecting...";
        }

        #region ButtonsCallback

        private void OnCreateGameHandler()
        {
            statusText.text = "Creating room...";
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
        }

        private void OnFindGameHandler()
        {
            statusText.text = "Finding room...";
            PhotonNetwork.JoinRandomRoom();
        }

        private void OnExitHandler()
        {
            Application.Quit();
        }

        #endregion

        #region PhotonCallbacks

        public override void OnConnectedToMaster()
        {
            statusText.text = "Connected.";
            createGame.interactable = true;
            findGame.interactable = true;
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(1);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            statusText.text = message;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            statusText.text = message;
        }

        #endregion
       
       
    }
}