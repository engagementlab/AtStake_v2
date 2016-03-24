﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Views {

	// Shows a list of games the player can join

	public class Games : View {

		protected override void OnInitElements () {
			Elements.Add ("back", new BackButtonElement ("hostjoin", () => { Game.Multiplayer.Disconnect (); }));
			Elements.Add ("game_list", new ListElement<ButtonElement> ());
			Elements.Add ("searching", new TextElement ("Searching for games..."));
		}

		protected override void OnShow () {

			ListElement<ButtonElement> list = GetScreenElement<ListElement<ButtonElement>> ("game_list");

			Game.Multiplayer.RequestHostList ((List<string> hosts) => {
				Dictionary<string, ButtonElement> hostButtons = new Dictionary<string, ButtonElement> ();
				for (int i = 0; i < hosts.Count; i ++) {
					string hostId = hosts[i];
					hostButtons.Add (hostId, new ButtonElement (hostId, () => {
						Game.Multiplayer.JoinGame (hostId, (string response) => {
							Game.StartGame ();
							switch (response) {
								case "room_full": OnShow (); break;
								case "name_taken":
									Debug.Log ("NAME TAKEN");
									// todo
									OnShow ();
									break;
								case "registered":
									GotoView ("lobby");
									break;
							}
						});
					}));
				}
				list.Set (hostButtons);
				Elements["searching"].Active = list.Count == 0;
			});
		}
	}
}