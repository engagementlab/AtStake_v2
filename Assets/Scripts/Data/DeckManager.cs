﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Models;

//// <summary>
/// Handles anything having to do with decks
/// This includes selecting a deck from a list of decks,
/// getting roles, and handling agenda items
/// </summary>
public class DeckManager : GameInstanceBehaviour {

	public List<string> Names {
		get { return Decks.ConvertAll (x => x.Name); }
	}

	public Deck Deck { get; private set; }

	public List<Role> Roles {
		get { 
			List<Role> roles = new List<Role> (Deck.Roles); 
			roles.Add (new Role () { Title = "Decider" });
			return roles;
		}
	}

	List<Deck> decks;
	List<Deck> Decks {
		get {
			if (decks == null) {
				decks = new List<Deck> (DataManager.GetDecks ());
			}
			return decks;
		}
	}

	public void Init () {
		Game.Dispatcher.AddListener ("SetDeck", SetDeck);
	}

	public void Reset () {
		Deck = null;
	}

	public void SetDeck (MasterMsgTypes.GenericMessage msg) {
		Deck = Decks.Find (x => x.Name == msg.str1);
	}
}
