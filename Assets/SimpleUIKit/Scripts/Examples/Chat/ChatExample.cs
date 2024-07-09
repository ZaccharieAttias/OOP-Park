using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Common;
using Assets.Scripts.Common;
using Assets.Scripts.Data;
using Assets.Scripts.Interface;
using Assets.Scripts.Interface.Elements;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.Examples.Chat
{
    public class ChatExample : BaseInterface
    {
        public Image Avatar;
        public Sprite AvatarExample;
        public Text Name;
        public InputField InputText;
        public Transform RowsMessagesList;
        public GameObject MessageIncomingPrefab;
        public GameObject MessageOutgoingPrefab;
        public GameObject HistoryButton;

        public string ServerUrl = "YoursServerUrl";
        public string ServerWriteControllerName = "WriteMessage";
        public string ServerReadControllerName = "ReadLastMessages";

        public static ChatExample Instance;

        private List<MessageRow> _rows = new();
        private Message[] _messages = Array.Empty<Message>();

        private const int MaxLastMessages = 2;

        [Serializable]
        public class ServerResponse
        {
            public bool Success;
            public string Error;
            public string Data;
            public string Meta;
        }

        private DateTime _prevTime;
        private bool _busy;
        private uint _chatId;
        private uint _playerProfileId;

        private int _updateSec = 10;

        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            if (Opened && !_busy && DateTime.Now.Subtract(_prevTime).TotalSeconds > _updateSec)
            {
                GetMessages(true);
            }
        }

        protected override void OnOpen()
        {
            Init(28115576, 28147056);
        }

        protected override void OnClose()
        {
            ResetRows();
            _messages = Array.Empty<Message>();
        }

        public void Init(uint chatId, uint playerProfileId)
        {
            _chatId = chatId;
            _playerProfileId = playerProfileId;
            _busy = false;
            _prevTime = DateTime.Now;

            Avatar.sprite = AvatarExample;
            Name.text = "Example";

            GetMessages();
        }

        public void GetMessages(bool silent = false)
        {
            if (_chatId == 0) return;

            _prevTime = DateTime.Now;
            _busy = true;

            PopupSpin.Instance.Run(() => ReadLastMessages(_chatId, (success, error, data) =>
            {
                if (success)
                {
                    InitTable(data.IsEmpty() ? null : JsonGeneric.FromJson<Message[]>(data), _chatId);
                    PopupSpin.Instance.Close();
                }
                else
                {
                    if (silent)
                    {
                        PopupSpin.Instance.Close();
                    }
                    else
                    {
                        PopupSpin.Instance.Stop(error);
                    }
                }

                _busy = false;
            }), "Loading...", silent);
        }

        private void ReadLastMessages(uint chatId, Action<bool, string, string> callback)
        {
            var form = new WWWForm();

            form.AddField("chatId", chatId.ToString());

            Download($"{ServerUrl}/{ServerReadControllerName}", form, callback);
        }

        private void Download(string url, WWWForm formData, Action<bool, string, string> callback)
        {
            callback(true, null,
                "[{\"TextEncrypted\":\"vU4x3wz8jA18Nw6/4U03Ik5MjuMKEw8i/DUm85IA0eQ=\",\"Time\":\"2024-04-26T18:31:11.7086109Z\",\"FromId\":28147056,\"ToId\":28115576,\"FromToIdHash\":\"523f5229915febfee5aef5c0abe877be\"},{\"TextEncrypted\":\"vU4x3wz8jA18Nw6/4U03ItVSRKcp5dVtxJkHDBy5D2g=\",\"Time\":\"2024-04-26T18:31:36.2678434Z\",\"FromId\":28115576,\"ToId\":28147056,\"FromToIdHash\":\"523f5229915febfee5aef5c0abe877be\"}]"
            );

            return; // STAB!!!

            StartCoroutine(Coroutine(url, formData, webRequest =>
            {
                if (webRequest.error.IsEmpty())
                {
                    var response = JsonGeneric.FromJson<ServerResponse>(webRequest.downloadHandler.text);

                    callback(response.Success, response.Error, response.Data);
                }
                else
                {
                    callback(false, webRequest.error, null);
                }
            }));
        }

        private IEnumerator Coroutine(string url, WWWForm formData, Action<UnityWebRequest> callback)
        {
            using var webRequest = UnityWebRequest.Post(url, formData);

#if UNITY_ANDROID && !UNITY_EDITOR
        if (AndroidNative.GetApiLevel() <= 25)
        {
            webRequest.certificateHandler = new BypassCertificate();
        }
#endif

            yield return webRequest.SendWebRequest();

            callback(webRequest);
        }

        public void SendMail()
        {
            if (InputText.text.Length > 0)
            {
                var encryptedText = EncryptMessage(InputText.text);

                if (_busy)
                {
                    PopupSpin.Instance.Run(() =>
                    {
                        while (_busy)
                        {
                        }

                        PopupSpin.Instance.Close();
                    }, "Loading...");
                }

                _busy = true;

                PopupSpin.Instance.Run(() => SendMail(_chatId, encryptedText, (success, error, data) =>
                {
                    if (!success)
                    {
                        PopupSpin.Instance.Stop(error);
                        return;
                    }

                    InitTable(data.IsEmpty() ? null : JsonGeneric.FromJson<Message[]>(data), _chatId);
                    InputText.text = "";
                    _busy = false;

                    PopupSpin.Instance.Close();
                }), "Loading...");
            }
        }

        public void SendMail(uint chatId, string message, Action<bool, string, string> callback)
        {
            var form = new WWWForm();

            form.AddField("playerProfileId", _playerProfileId.ToString());
            form.AddField("chatId", chatId.ToString());
            form.AddField("encText", message);
            Download($"{ServerUrl}/{ServerWriteControllerName}", form, callback);
        }

        public void LoadHistory()
        {
            if (!PlayerPrefs.HasKey("HistoryMail")) return;

            //var history = PlayerPrefs.GetString("HistoryMail");
            var history =
                "[{\"TextEncrypted\":\"9YH/wkoT/WPSAC+ibJgiSZ6bsjPkTTz9hvJlhxb+M9Y=\",\"Time\":\"2024-04-26T19:05:17.9554375Z\",\"FromId\":28115576,\"ToId\":28147056,\"FromToIdHash\":\"523f5229915febfee5aef5c0abe877be\"},{\"TextEncrypted\":\"9YH/wkoT/WPSAC+ibJgiSegw0C5aQQQRz754HL9OQxU=\",\"Time\":\"2024-04-26T19:05:41.4197633Z\",\"FromId\":28147056,\"ToId\":28115576,\"FromToIdHash\":\"523f5229915febfee5aef5c0abe877be\"},{\"TextEncrypted\":\"vU4x3wz8jA18Nw6/4U03Ik5MjuMKEw8i/DUm85IA0eQ=\",\"Time\":\"2024-04-26T18:31:11.7086109Z\",\"FromId\":28147056,\"ToId\":28115576,\"FromToIdHash\":\"523f5229915febfee5aef5c0abe877be\"},{\"TextEncrypted\":\"vU4x3wz8jA18Nw6/4U03ItVSRKcp5dVtxJkHDBy5D2g=\",\"Time\":\"2024-04-26T18:31:36.2678434Z\",\"FromId\":28115576,\"ToId\":28147056,\"FromToIdHash\":\"523f5229915febfee5aef5c0abe877be\"}]";

            _messages = JsonGeneric.FromJson<Message[]>(history);
            InitMessages(false);
        }

        private void InitTable(Message[] messagesNew, uint friendId)
        {
            HistoryButton.SetActive(PlayerPrefs.HasKey("HistoryMailCount") &&
                                    PlayerPrefs.GetInt("HistoryMailCount") >= MaxLastMessages);

            if (messagesNew == null) return;

            var messages = MergeMessages(_messages, messagesNew);

            if (messages.Length > 0)
            {
                _messages = messages;
                InitMessages();
            }
        }

        private void InitMessages(bool updateHistory = true)
        {
            if (_rows.Count > 0) ResetRows();

            foreach (var message in _messages)
            {
                _rows.Add(MakeMessageRow(DecryptMessage(message.TextEncrypted),
                    message.Time.ToLocalTime().ToString(CultureInfo.InvariantCulture),
                    message.FromId == _playerProfileId ? MessageOutgoingPrefab : MessageIncomingPrefab,
                    RowsMessagesList));
            }

            StartCoroutine(Rerender());

            if (updateHistory) WriteHistory();
        }

        private void WriteHistory()
        {
            if (PlayerPrefs.HasKey("HistoryMail"))
            {
                var messagesOld = JsonGeneric.FromJson<Message[]>(PlayerPrefs.GetString("HistoryMail"));
                var history = MergeMessages(messagesOld, _messages);

                if (history.Length > 0)
                {
                    PlayerPrefs.SetString("HistoryMail", JsonGeneric.ToJson(history));
                    PlayerPrefs.SetInt("HistoryMailCount", history.Length);
                }
            }
            else
            {
                PlayerPrefs.SetString("HistoryMail", JsonGeneric.ToJson(_messages));
                PlayerPrefs.SetInt("HistoryMailCount", _messages.Length);
            }
        }

        private MessageRow MakeMessageRow(string text, string time, GameObject rowPrefab, Transform parent)
        {
            var instance = Instantiate(rowPrefab, parent);

            instance.TryGetComponent<MessageRow>(out var row);

            if (row == null)
            {
                Debug.LogWarning("MakeMessageRow: MessageRow Prefab does not contain 'MessageRow' component!");
            }
            else
            {
                row.Init(text, time);
            }

            return row;
        }

        private string EncryptMessage(string message)
        {
            return AES.Encrypt(message, (_playerProfileId + _chatId).ToString());
        }

        private string DecryptMessage(string message)
        {
            return AES.Decrypt(message, (_playerProfileId + _chatId).ToString());
        }

        private Message[] MergeMessages(Message[] messagesOld, Message[] messagesNew)
        {
            if (messagesNew.Length == messagesOld.Length && messagesNew[0].Equals(messagesOld[0]))
                return Array.Empty<Message>();

            var index = Array.FindIndex(messagesOld, i => i.Equals(messagesNew[0]));

            if (index == -1) return messagesOld.Concat(messagesNew).ToArray();

            var current = messagesOld[index..];

            return current.Length == messagesNew.Length
                ? Array.Empty<Message>()
                : messagesOld[..index].Concat(messagesNew).ToArray();
        }

        private IEnumerator Rerender()
        {
            Canvas.ForceUpdateCanvases();
            RowsMessagesList.transform.GetComponent<ContentSizeFitter>().verticalFit =
                ContentSizeFitter.FitMode.Unconstrained;
            yield return new WaitForSeconds(0.05f);
            RowsMessagesList.transform.GetComponent<ContentSizeFitter>().verticalFit =
                ContentSizeFitter.FitMode.MinSize;

            RowsMessagesList.transform.GetComponent<ContentSizeFitter>().verticalFit =
                ContentSizeFitter.FitMode.Unconstrained;
            yield return new WaitForSeconds(0.05f);
            RowsMessagesList.transform.GetComponent<ContentSizeFitter>().verticalFit =
                ContentSizeFitter.FitMode.MinSize;
        }

        private void ResetRows()
        {
            _rows.ForEach(i => Destroy(i.gameObject));
            _rows.Clear();
        }
    }
}
