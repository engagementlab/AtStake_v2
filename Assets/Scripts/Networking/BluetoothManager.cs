﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BluetoothManager : MonoBehaviour, IConnectionManager {

	public ConnectionStatus Status {
		get { return status; }
	}

	ConnectionStatus status;

	public void Host (string gameInstanceName) {

	}

	public void Join (string hostName, string gameInstanceName) {

	}

	public List<string> UpdateHosts () {
		return null;
	}

	public void ConnectClient (string gameInstanceName) {
		
	}

	public void DisconnectClient (string gameInstanceName) {

	}

	public void Disconnect () {

	}
}
