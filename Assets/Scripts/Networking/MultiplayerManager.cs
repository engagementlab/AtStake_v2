﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class MultiplayerManager : GameInstanceBehaviour {

	public delegate void OnLogMessage (string msg);
	public delegate void OnDisconnected ();

	/// <summary>
	/// Returns true if this is the game's host
	/// </summary>
	public bool Hosting { get; private set; }

	/// <summary>
	/// Returns true if connected (always true if hosting)
	/// </summary>
	public bool Connected {
		get { return Hosting || connected; }
		private set { connected = value; }
	}

	/// <summary>
	/// Gets/sets name of the game's host
	/// </summary>
	public string Host { get; private set; }

	/// <summary>
	/// If hosting, gets a list of client names connected to this host
	/// </summary>
	public List<string> Clients {
		get { return clients; }
	}

	/// <summary>
	/// Gets a list of host names populated from the master server
	/// </summary>
	public List<string> Hosts {
		get { return new List<string> (hosts.Keys); }
	}

	/// <summary>
	/// Returns true if the host was disconnected with an error. This flag is used to display an error message to the user.
	/// </summary>
	public bool DisconnectedWithError { get; private set; }

	public NetworkMasterServer server;
	public NetworkMasterClient client;
	public OnLogMessage onLogMessage;
	public OnDisconnected onDisconnected;

	Dictionary<string, string> hosts;
	List<string> clients = new List<string> ();
	// System.Action<string> clientRegisterResponse;
	bool connected = false;
	AvatarsManager avatars;

	NetManager net = new NetManager ();
	NetManager2 net2;

	void OnEnable () {

		net2 = new NetManager2 (gameObject.AddComponent<SocketIOComponent> ());
		
		// MasterServer
		// debugging messages
		/*server.onServerMessage += SendLogMessage;
		client.onClientMessage += SendLogMessage;*/

		// events
		/*client.callbacks.AddListener ("disconnected", OnDisconnect);
		client.onRegisteredClient += OnRegisteredClient;
		client.onUnregisteredClient += OnUnregisteredClient;
		client.onReceiveMessageFromHost += ReceiveMessageFromHost;
		client.onReceiveMessageFromClient += ReceiveMessageFromClient;*/
	}

	public void HostGame () {

		// MasterServer
		/*#if SINGLE_SCREEN
		if (UnityEngine.Networking.NetworkServer.active) {
			Debug.LogWarning ("Only one host is allowed in Single Screen mode.");
			return;
		}
		#endif*/

		// Set this player as the host
		Host = Game.Name;
		Hosting = true;
		DisconnectedWithError = false;

		// Initialize avatar manager and set the host's avatar
		avatars = new AvatarsManager ();
		avatars.AddPlayer (Host);
		Game.Manager.AddHost (avatars[Host]);

		// NetManager
		/*net.StartAsHost (Host, (bool connected) => {
			// TODO: don't connect if name_taken
			Debug.Log ("Connected ? " + connected);
			if (connected) {
				net.ListenForClients (OnUpdateClients);
				net.ReceiveMessage (ReceiveMessageFromClient);
			}
		});*/

		// MasterServer
		// Start the server
		// server.Initialize ();

		// Start the client and connect to the server as the host
		// Use the discovery service to broadcast this game
		/*client.StartAsHost (Host, () => {
			DiscoveryService.StartBroadcasting (Host, client.IpAddress, OnBroadcastError);
		});*/
	}

	public void RequestHostList (Action<List<string>> callback) {
		// NetManager
		/*net.RequestRoomList ((Dictionary<string, string> hosts) => {
			this.hosts = hosts;
			callback (new List<string> (hosts.Keys));
		});*/

		// MasterServer
		/*DiscoveryService.StartListening (this, (Dictionary<string, string> hosts) => {
			this.hosts = hosts;
			callback (new List<string> (hosts.Keys));
		});*/
	}

	public void JoinGame (string hostName, Action<string> response) {

		// Set the host
		Host = hostName;

		// NetManager
		/*net.StartAsClient (hosts[Host], Game.Name, (string res) => {
			if (res != "room_full" && res != "name_taken") {
				Connected = true;
				net.ReceiveMessage (ReceiveMessageFromHost);
			}
			response (res);
		});*/

		// MasterServer
		// Setup response callback
		// clientRegisterResponse += response;

		// Start the client and request to join the host's game
		// Stop the discovery service from listening for games to join
		/*client.StartAsClient (Game.Name, hosts[Host], () => {
			DiscoveryService.StopListening (this);
		});*/
	}

	public void JoinGame (string ipAddress) {
		// MasterServer
		/*client.StartAsClient (Game.Name, ipAddress, () => {
			Debug.Log ("joined");
		});*/
	}

	public void GameStarted () {
		// MasterServer
		// DiscoveryService.StopBroadcasting ();
		// DiscoveryService.StopListening (this);
	}

	// Intentional disconnect (player chose to terminate their connection)
	// This will stop the discovery service
	// If a connection has been established, the OnDisconnect event will also fire
	public void Disconnect () {

		// NetManager
		/*net.Stop ();
		OnDisconnect ();*/

		// MasterServer
		/*if (Hosting) {
			DiscoveryService.StopBroadcasting ();
			client.UnregisterHost (Host, () => {
				Co.WaitForFixedUpdate (() => {
					server.Reset ();
				});
			});
		} else {
			DiscoveryService.StopListening (this);
			client.UnregisterClient ();
			OnDisconnect ();
		}*/
	}

	// -- Client handling

	// Only the host uses these methods
	// Adds/removes clients to the list of clients as they join and leave the room
	// Enables/disables the discovery broadcaster based on whether or not there are available slots in the room
	// Sends a message whenever the client list is updated so that all players know who's in the game

	void AddClient (string clientName) {
		Clients.Add (clientName);
		avatars.AddPlayer (clientName);
		// UpdateBroadcast ();
		UpdatePlayers ();
	}

	void RemoveClient (string clientName) {
		Clients.Remove (clientName);
		avatars.RemovePlayer (clientName);
		// UpdateBroadcast ();
		UpdatePlayers ();
	}

	void UpdatePlayers () {
		Game.Dispatcher.ScheduleMessage ("UpdatePlayers", avatars.GetPlayers ());
	}

	// MasterServer
	/*void UpdateBroadcast () {
		int maxPlayerCount = DataManager.GetSettings ().PlayerCountRange[1];
		if (Clients.Count+1 < maxPlayerCount) {
			DiscoveryService.StartBroadcasting (Host, client.IpAddress, OnBroadcastError);
		} else {
			DiscoveryService.StopBroadcasting ();
		}
	}*/

	// -- Messaging

	public void SendMessageToHost (MasterMsgTypes.GenericMessage msg) {

		// Client sends message to host so that host can relay it to all clients
		// MasterServer
		// client.SendMessageToHost (msg);

		// NetManager
		// net.SendMessage (msg);
	}

	public void ReceiveMessageFromClient (MasterMsgTypes.GenericMessage msg) {

		// Host receives message from client so that it can relay it to all other clients
		Game.Dispatcher.ReceiveMessageFromClient (msg);
	}

	public void SendMessageToClients (MasterMsgTypes.GenericMessage msg) {

		// Host sends message to all clients
		// MasterServer
		// client.SendMessageToClients (msg);

		// NetManager
		// net.SendMessage (msg);
	}

	public void ReceiveMessageFromHost (MasterMsgTypes.GenericMessage msg) {

		// Clients receive message from host
		if (!Hosting)
			Game.Dispatcher.ReceiveMessageFromHost (msg);
	}

	// -- Events

	// Intentional & unintentional disconnect
	void OnDisconnect () {
		if (Hosting) {
			// DiscoveryService.StopBroadcasting ();
			Clients.Clear ();
			Hosting = false;
		} else {
			// DiscoveryService.StopListening (this);
		}
		Host = "";
		Connected = false;
		if (onDisconnected != null)
			onDisconnected ();
	}

	// MasterServer
	// deprecate
	void OnRegisteredClient (int resultCode, string clientName) {
		
		bool thisClient = clientName == Game.Name;
		
		string keyword;
		switch (resultCode) {
			case -2: keyword = "room_full"; break; // (technically this should never happen because the host will stop broadcasting when the room is maxed out)
			case -1: keyword = "name_taken"; break;
			default: 
				keyword = "registered"; 
				if (Hosting) {
					AddClient (clientName);
				} else if (thisClient) {
					Connected = true;
				}
				break;
		}

		if (thisClient) {
			// SendClientRegisterResponse (keyword);
		}
	}

	// MasterServer
	// deprecate
	void OnUnregisteredClient (string clientName) {
		if (Hosting) {
			RemoveClient (clientName);
		}
	}

	// NetManager
	void OnUpdateClients (string[] regClients) {

		// Add new clients
		foreach (string client in regClients) {
			if (!Clients.Contains (client))
				AddClient (client);
		}

		// Remove old clients
		List<string> tempClients = new List<string> (Clients);
		foreach (string client in tempClients) {
			if (Array.FindIndex (regClients, (string c) => { return c.Equals (client); }) == -1)
				RemoveClient (client);
		}
	}

	/*void SendClientRegisterResponse (string response) {
		if (clientRegisterResponse != null) {
			clientRegisterResponse (response);
			clientRegisterResponse = null;
		}
	}*/

	// MasterServer
	void OnBroadcastError () {
		DisconnectedWithError = true;
		Disconnect ();
	}

	// -- Debugging

	void SendLogMessage (string msg) {
		if (onLogMessage != null)
			onLogMessage (msg);
	}
}
