using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject loginContainer;
        [SerializeField] private GameObject gameContainer;
        [SerializeField] private TMP_Text nickText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_InputField nickInputField;
        [SerializeField] private Button login;
        [SerializeField] private Button createGame;
        [SerializeField] private Button findGame;
        [SerializeField] private Button exit;
        private const string NicknamePrefsKey = "NicknameKey";

        private void Awake()
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
            login.onClick.AddListener(OnLoginHandler);
            createGame.onClick.AddListener(OnCreateGameHandler);
            findGame.onClick.AddListener(OnFindGameHandler);
            exit.onClick.AddListener(OnExitHandler);
            statusText.text = "Connecting...";
        }

        private void OnDestroy()
        {
            login.onClick.RemoveListener(OnLoginHandler);
            createGame.onClick.RemoveListener(OnCreateGameHandler);
            findGame.onClick.RemoveListener(OnFindGameHandler);
            exit.onClick.RemoveListener(OnExitHandler);
        }

        #region ButtonsCallback

        private void OnLoginHandler()
        {
            var inputText = nickInputField.text;
            if (string.IsNullOrWhiteSpace(inputText) || inputText.Length > 11 || inputText.Length < 3)
            {
                statusText.text = $"Wrong nickname {inputText}";
                return;
            }
            PlayerPrefs.SetString(NicknamePrefsKey, inputText);
            PhotonNetwork.NickName = inputText;
            nickText.text = $"Hello {inputText}";
            loginContainer.SetActive(false);
            gameContainer.SetActive(true);
            statusText.text = "Login success.";
        }
        
        private void OnCreateGameHandler()
        {
            statusText.text = "Creating room...";
            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = 4});
            createGame.interactable = false;
            findGame.interactable = false;
        }

        private void OnFindGameHandler()
        {
            statusText.text = "Finding room...";
            PhotonNetwork.JoinRandomRoom();
            createGame.interactable = false;
            findGame.interactable = false;
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
            loginContainer.SetActive(true);
            if (PlayerPrefs.HasKey(NicknamePrefsKey))
            {
                nickInputField.text = PlayerPrefs.GetString(NicknamePrefsKey);
            }
        }

        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene(1);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            statusText.text = message;
            createGame.interactable = true;
            findGame.interactable = true;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            statusText.text = message;
        }

        #endregion
       
       
    }
}