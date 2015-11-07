using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class AudioManagerTests : PMonoBehaviour
{
	public AudioSettingsBase Settings;

	[Button]
	public bool play;
	void Play()
	{
		var item = AudioManager.Instance.CreateItem(Settings);
		item.OnUpdate += i => { if (pause) i.Pause(); if (resume) i.Resume(); if (@break) i.Break(); if (stop) i.Stop(); if (stopImmediate) i.StopImmediate(); };
		item.Play();
	}

	[Button]
	public bool pause;
	[Button]
	public bool resume;
	[Button]
	public bool @break;
	[Button]
	public bool stop;
	[Button]
	public bool stopImmediate;
}
