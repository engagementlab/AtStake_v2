﻿using UnityEngine;
using System.Collections;

public class NameTemplate : TemplateContent {

	public override TemplateSettings Settings {
		get {
			return new TemplateSettings () {
				TopBarEnabled = true,
				TopBarColor = Palette.Orange,
				BackgroundColor = Palette.White,
				BackgroundImage = "applause-bg"
			};
		}
	}
}