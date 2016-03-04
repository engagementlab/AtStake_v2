﻿using System.Collections;
using System.Collections.Generic;

public interface IConnectionManager {
	// ConnectionStatus Status { get; }
	void Init (string gameInstanceName, MultiplayerManager multiplayer);
	void Host ();
	void Join (string hostName);
	void RequestHostList (System.Action<List<string>> callback);
	void ConnectClient (string clientName);
	void DisconnectClient ();
	void Disconnect (string hostName);
	void SendMessageToHost (string id, string str1, string str2, int val);
	void ReceiveMessageFromClient (string id, string str1, string str2, int val);
	void SendMessageToClients (string id, string str1, string str2, int val);
	void ReceiveMessageFromHost (string id, string str1, string str2, int val);
}