using System.Collections.Generic;
using ExitGames.Client.Photon;
using Game;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private GameObject loginContainer;
        [SerializeField] private GameObject gameContainer;
        [SerializeField] private GameObject roomContainer;
        [SerializeField] private TMP_Text nickText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_InputField nickInputField;
        [SerializeField] private Button login;
        [SerializeField] private Button createRoom;
        [SerializeField] private Button findRoom;
        [SerializeField] private Button exit;
        [SerializeField] private Button ready;
        [SerializeField] private Button startGame;
        [SerializeField] private Image[] readyIndicators;
        private const string NicknamePrefsKey = "NicknameKey";
        private readonly Dictionary<int, int> _playersIndicatorsList = new Dictionary<int, int>();
        private int _activeIndicators;
        private readonly Dictionary<int, bool> _indicatorsStatus = new Dictionary<int, bool>();

        #region MonoBeh

        private void Awake()
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            statusText.text = "Connecting...";
        }

        private void Start()
        {
            login.onClick.AddListener(OnLoginHandler);
            createRoom.onClick.AddListener(OnCreateGameHandler);
            findRoom.onClick.AddListener(OnFindGameHandler);
            ready.onClick.AddListener(OnReadyHandler);
            exit.onClick.AddListener(OnExitHandler);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        private void OnDestroy()
        {
            login.onClick.RemoveListener(OnLoginHandler);
            createRoom.onClick.RemoveListener(OnCreateGameHandler);
            findRoom.onClick.RemoveListener(OnFindGameHandler);
            ready.onClick.RemoveListener(OnReadyHandler);
            exit.onClick.RemoveListener(OnExitHandler);
            if (!PhotonNetwork.IsMasterClient)return;
            startGame.onClick.RemoveListener(OnStartGameHandler);
        }

        #endregion
        
       

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
            createRoom.interactable = false;
            findRoom.interactable = false;
        }

        private void OnFindGameHandler()
        {
            statusText.text = "Finding room...";
            PhotonNetwork.JoinRandomRoom();
            createRoom.interactable = false;
            findRoom.interactable = false;
        }

        private void OnStartGameHandler()
        {
            foreach (var statusValue in _indicatorsStatus.Values)
            {
                if (statusValue) continue;
                return;
            }
            PhotonNetwork.LoadLevel(1);
        }

        private void OnReadyHandler()
        {
            PhotonNetwork.RaiseEvent(EventCodePhoton.ReadyStatusChange, PhotonNetwork.LocalPlayer.ActorNumber,
                new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient}, SendOptions.SendReliable);
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
            gameContainer.SetActive(false);
            loginContainer.SetActive(false);
            statusText.gameObject.SetActive(false);
           roomContainer.SetActive(true);
           if (!PhotonNetwork.IsMasterClient)return;
           startGame.onClick.AddListener(OnStartGameHandler);
           startGame.gameObject.SetActive(true);
           _playersIndicatorsList.Add(PhotonNetwork.LocalPlayer.ActorNumber, _activeIndicators);
           _indicatorsStatus.Add(_activeIndicators, false);
           readyIndicators[0].enabled = true;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            if (!PhotonNetwork.IsMasterClient)return;
            _activeIndicators++;
            _playersIndicatorsList.Add(newPlayer.ActorNumber, _activeIndicators);
            _indicatorsStatus.Add(_activeIndicators, false);
            PhotonNetwork.RaiseEvent(EventCodePhoton.NewPlayerEnterRoom, _indicatorsStatus,
                new RaiseEventOptions {Receivers = ReceiverGroup.All}, SendOptions.SendReliable);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            statusText.text = message;
            createRoom.interactable = true;
            findRoom.interactable = true;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            statusText.text = message;
        }

        #endregion

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
               case EventCodePhoton.NewPlayerEnterRoom:
                   AddNewIndicator((Dictionary<int, bool>)photonEvent.CustomData);
                   break;
               case EventCodePhoton.ReadyStatusChange:
                   ChangeIndicatorStatus((int)photonEvent.CustomData);
                   PhotonNetwork.RaiseEvent(EventCodePhoton.ReadyStatusRefreshSend, _indicatorsStatus,
                       new RaiseEventOptions {Receivers = ReceiverGroup.All}, SendOptions.SendReliable);
                   break;
               case EventCodePhoton.ReadyStatusRefreshSend:
                   IndicatorRefresh((Dictionary<int, bool>)photonEvent.CustomData);
                   break;
            }
        }

        private void AddNewIndicator(Dictionary<int, bool> indicatorsActivity)
        {
            for (int i = 0; i < indicatorsActivity.Count; i++)
            {
                readyIndicators[i].enabled = true;
            }
            IndicatorRefresh(indicatorsActivity);
        }
        
        private void ChangeIndicatorStatus(int actorNumber)
        {
            var indicatorIndex = _playersIndicatorsList[actorNumber];
            _indicatorsStatus[indicatorIndex] = !_indicatorsStatus[indicatorIndex];
        }
        
        private void IndicatorRefresh(Dictionary<int, bool> indicatorsActivity)
        {
            for (int i = 0; i < indicatorsActivity.Count; i++)
            {
                readyIndicators[i].color = indicatorsActivity[i] ? Color.green : Color.red;
            }
        }
    }
}