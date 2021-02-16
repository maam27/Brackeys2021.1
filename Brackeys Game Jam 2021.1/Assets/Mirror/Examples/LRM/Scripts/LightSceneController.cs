using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using LightReflectiveMirror;
using UnityEngine.SceneManagement;

public class LightSceneController : NetworkBehaviour
{
    public KeyCode changeSceneKey = KeyCode.T;
    private int random;

    public static LightSceneController singleton;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this); }
    }

    // Update is called once per frame
    void Update()
    {
        if(!NetworkServer.localClientActive) return;

        if (Input.GetKeyDown(changeSceneKey))
        {
            random = Random.Range(0, 2);

            if (random == 0)
            {
                LightNetworkManager.singleton.ServerChangeScene("Scene");
                LightNetworkManager.instance.UpdateServerData("Scene");

            }
            else
            {
                LightNetworkManager.singleton.ServerChangeScene("SceneWithBox");
                LightNetworkManager.instance.UpdateServerData("SceneWithBox");
            }
        }
    }
}
