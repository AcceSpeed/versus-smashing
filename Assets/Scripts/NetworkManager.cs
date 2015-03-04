//*********************************************************
// Societe: ETML
// Auteur : Vincent Mouquin
// Date : 16.02.15
// But : Network control test script
//*********************************************************
// Modifications:
// Date :
// Auteur :
// Raison :
//*********************************************************
// Date :
// Auteur :
// Raison :
//*********************************************************

using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private const string typeName = "VerSmaTESTINGPHASE";
	private const string gameName = "VerSmaRoomOne";
	private const string gameCom = "RoomComment";
	public GameObject playerPrefab;

	private HostData[] hostList;

	
	private void StartServer()
	{
		Network.InitializeServer (2, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (typeName, gameName);
	}

	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}

	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnServerInitialized()
	{
		SpawnPlayer();
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	void OnConnectedToServer()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		Network.Instantiate(playerPrefab, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
