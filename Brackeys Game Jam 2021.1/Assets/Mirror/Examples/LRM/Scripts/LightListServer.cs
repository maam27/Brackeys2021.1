using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using LightReflectiveMirror;

public class LightListServer : MonoBehaviour
{
    [Header("Transport's Info")]
    public LightReflectiveMirrorTransport lightReflectiveMirrorTransport;

    [Header("UI")]
    public GameObject mainPanel;
    public Transform content;
    public Text statusText;
    public UILightServerStatusSlot slotPrefab;
    public Button serverAndPlayButton;
    public Button serverOnlyButton;
    public GameObject connectingPanel;
    public Text connectingText;
    public Button connectingCancelButton;

    bool RelayConnected() => lightReflectiveMirrorTransport.IsAuthenticated();
    bool IsConnecting() => NetworkClient.active && !ClientScene.ready;
    bool FullyConnected() => NetworkClient.active && ClientScene.ready;

    [Header("Refresh Settings")]
    public KeyCode refreshKey;
    public bool refreshOnStart = true;
    public int refreshInterval = 10; //0 To Disable

    private NetworkManagerHUD networkManagerHUD;

    // Start is called before the first frame update
    void Start()
    {
        networkManagerHUD = GetComponent<NetworkManagerHUD>();

        serverAndPlayButton.onClick.RemoveAllListeners();
        serverAndPlayButton.onClick.AddListener(() =>
        {
            NetworkManager.singleton.StartHost();
        });

        serverOnlyButton.onClick.RemoveAllListeners();
        serverOnlyButton.onClick.AddListener(() =>
        {
            NetworkManager.singleton.StartServer();
        });

        if (refreshOnStart)
        {
            RequestServersList();
        }

        if (refreshInterval > 0)
        {
            InvokeRepeating(nameof(RequestServersList), Time.realtimeSinceStartup + refreshInterval, refreshInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RelayConnected() == false)
        {
            connectingText.text = "No Relay Server Running";

            connectingCancelButton.onClick.RemoveAllListeners();
            connectingCancelButton.onClick.AddListener(() => connectingPanel.SetActive(false));
            return;
        }

        if (Input.GetKeyDown(refreshKey))
        {
            RequestServersList();
        }

        OnUI();
    }

    public void RequestServersList()
    {
        // You can remove these 2 statements it acts as a loading stuff
        connectingPanel.SetActive(true);
        connectingText.text = "Loading Server's Info from Relay Server";

        lightReflectiveMirrorTransport.RequestServerList();
    }

    public void RefreshServersList()
    {
        connectingPanel.SetActive(false);
        connectingText.text = "";
    }

    // instantiate/remove enough prefabs to match amount
    public void BalancePrefabs(GameObject prefab, int amount, Transform parent)
    {
        // instantiate until amount
        for (int i = parent.childCount; i < amount; ++i)
        {
            Instantiate(prefab, parent, false);
        }

        // delete everything that's too much
        // (backwards loop because Destroy changes childCount)
        for (int i = parent.childCount - 1; i >= amount; --i)
            Destroy(parent.GetChild(i).gameObject);
    }

    void OnUI()
    {
        // only show while client not connected and server not started
        if (!NetworkManager.singleton.isNetworkActive || IsConnecting())
        {
            mainPanel.SetActive(true);

            // instantiate/destroy enough slots
            BalancePrefabs(slotPrefab.gameObject, lightReflectiveMirrorTransport.relayServerList.Count, content);

            // refresh all members
            for (int i = 0; i < lightReflectiveMirrorTransport.relayServerList.Count; ++i)
            {
                UILightServerStatusSlot uiLightServerStatusSlot = content.GetChild(i).GetComponent<UILightServerStatusSlot>();

                if (uiLightServerStatusSlot != null)
                {
                    // use this for the title of server
                    // uIDarkServerStatusSlot.titleText.text = darkReflectiveMirrorTransport.relayServerList[i].serverName;

                    // i use this to combine the extra variable stuff,[serverData] is extra Variable
                    uiLightServerStatusSlot.titleText.text = lightReflectiveMirrorTransport.relayServerList[i].serverName +
                       " " + lightReflectiveMirrorTransport.relayServerList[i].serverData;

                    uiLightServerStatusSlot.playersText.text = lightReflectiveMirrorTransport.relayServerList[i].currentPlayers
                        + "/" + lightReflectiveMirrorTransport.relayServerList[i].maxPlayers;

       ///             Debug.Log(lightReflectiveMirrorTransport.relayServerList[i].serverData);

                    //u can hide it
                    uiLightServerStatusSlot.addressText.text = lightReflectiveMirrorTransport.relayServerList[i].serverId.ToString();

                    int serverID = lightReflectiveMirrorTransport.relayServerList[i].serverId;

                    uiLightServerStatusSlot.joinButton.onClick.RemoveAllListeners();
                    uiLightServerStatusSlot.joinButton.onClick.AddListener(() =>
                    {
                        NetworkManager.singleton.networkAddress = serverID.ToString();
                        NetworkManager.singleton.StartClient();
                    });
                }
            }
        }
        else mainPanel.SetActive(false);

        if (IsConnecting())
        {
            connectingPanel.SetActive(true);
            connectingText.text = "Connecting";

            // cancel button
            connectingCancelButton.onClick.RemoveAllListeners();
            connectingCancelButton.onClick.AddListener(NetworkManager.singleton.StopClient);
        }
        else connectingPanel.SetActive(false);

        if (FullyConnected())
        {
            networkManagerHUD.showGUI = true;
        }
        else { networkManagerHUD.showGUI = false; }

        if (RelayConnected())
        {
            statusText.text = "Connected";
        }
        else { statusText.text = "Status"; }
    }

    void OnApplicationQuit()
    {
        if (lightReflectiveMirrorTransport.ServerActive()) 
        {
            lightReflectiveMirrorTransport.ClientDisconnect();
        }

        if (lightReflectiveMirrorTransport.ClientConnected()) 
        {
            lightReflectiveMirrorTransport.ClientDisconnect();
        }
    }
}
