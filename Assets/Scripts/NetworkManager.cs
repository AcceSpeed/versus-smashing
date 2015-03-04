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

	private const string STR_GAME_NAME = "VersusSmashingNetwork";

	private string strRoomComment;

	private GameObject playerPrefab;

	private HostData[] hostList;

	public void StartServer()
	{
		Network.InitializeServer (2, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (STR_GAME_NAME, MainController.strPlayerName);
	}

	public void RefreshHostList()
	{
		MasterServer.RequestHostList(STR_GAME_NAME);
	}

	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}

	void OnServerInitialized()
	{

	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	void OnConnectedToServer()
	{

	}
}
