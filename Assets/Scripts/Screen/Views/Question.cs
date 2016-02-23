﻿using UnityEngine;
using System.Collections;

namespace Views {

	// Shows the question that players will be debating in this round
	// The Decider is instructed to read a script

	public class Question : View {

		protected override void OnInitDeciderElements () {
			Elements.Add ("question", new TextElement (Game.Decks.GetQuestion ()));
			Elements.Add ("next", new NextButtonElement ("think_instructions"));
		}

		protected override void OnInitPlayerElements () {
			Elements.Add ("question", new TextElement (Game.Decks.GetQuestion ()));
		}
	}
}