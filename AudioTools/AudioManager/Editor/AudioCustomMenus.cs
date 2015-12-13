using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;
using Pseudo.Internal.Audio;

namespace Pseudo.Internal.Editor
{
	public static class AudioCustomMenus
	{
		[MenuItem("Pseudo/Create/Audio Settings/Source", validate = true, priority = 9)]
		[MenuItem("Assets/Create/Audio Settings/Source", validate = true, priority = 9)]
		static bool CreateAudioSourceSettingsValid()
		{
			return Array.Exists(Selection.objects, obj => obj is AudioClip);
		}

		[MenuItem("Pseudo/Create/Audio Settings/Source", priority = 9)]
		[MenuItem("Assets/Create/Audio Settings/Source", priority = 9)]
		static void CreateAudioSourceSettings()
		{
			for (int i = 0; i < Selection.objects.Length; i++)
			{
				AudioClip clip = Selection.objects[i] as AudioClip;

				if (clip == null)
					continue;

				AudioSourceSettings settings = CreateAudioContainerSettings<AudioSourceSettings>(clip.name, AssetDatabase.GetAssetPath(clip));
				settings.name = clip.name;
				settings.Clip = clip;
			}
		}

		[MenuItem("Pseudo/Create/Audio Settings/Container/Mix", priority = 10)]
		[MenuItem("Assets/Create/Audio Settings/Container/Mix", priority = 10)]
		static void CreateAudioMixContainerSettings()
		{
			CreateAudioContainerSettings<AudioMixContainerSettings>("Mix Container");
		}

		[MenuItem("Pseudo/Create/Audio Settings/Container/Random", priority = 11)]
		[MenuItem("Assets/Create/Audio Settings/Container/Random", priority = 11)]
		static void CreateAudioRandomContainerSettings()
		{
			CreateAudioContainerSettings<AudioRandomContainerSettings>("Random Container");
		}

		[MenuItem("Pseudo/Create/Audio Settings/Container/Enumerator", priority = 12)]
		[MenuItem("Assets/Create/Audio Settings/Container/Enumerator", priority = 12)]
		static void CreateAudioEnumeratorContainerSettings()
		{
			CreateAudioContainerSettings<AudioEnumeratorContainerSettings>("Enumerator Container");
		}

		[MenuItem("Pseudo/Create/Audio Settings/Container/Switch", priority = 13)]
		[MenuItem("Assets/Create/Audio Settings/Container/Switch", priority = 13)]
		static void CreateAudioSwitchContainerSettings()
		{
			CreateAudioContainerSettings<AudioSwitchContainerSettings>("Switch Container");
		}

		[MenuItem("Pseudo/Create/Audio Settings/Container/Sequence", priority = 14)]
		[MenuItem("Assets/Create/Audio Settings/Container/Sequence", priority = 14)]
		static void CreateAudioSequenceContainerSettings()
		{
			CreateAudioContainerSettings<AudioSequenceContainerSettings>("Sequence Container");
		}

		static T CreateAudioContainerSettings<T>(string name, string settingsPath = "") where T : AudioSettingsBase
		{
			T settings = ScriptableObject.CreateInstance<T>();
            string path = AssetDatabaseUtility.GenerateUniqueAssetPath(name, settingsPath);
			AssetDatabase.CreateAsset(settings, path);

			return settings;
		}
	}
}
