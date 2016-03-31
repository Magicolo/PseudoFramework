using UnityEngine;
using System.Collections;

namespace Pseudo
{
	public static class AudioClipExtensions
	{
		public static AudioClip Concat(this AudioClip audioClip, AudioClip otherClip)
		{
			int length = audioClip.samples >= otherClip.samples ? audioClip.samples : otherClip.samples;
			AudioClip clipSum = AudioClip.Create(audioClip.name + " + " + otherClip.name, length, audioClip.channels, audioClip.frequency, false);

			float[] dataSum;
			float[] otherData;

			if (audioClip.samples >= otherClip.samples)
			{
				dataSum = new float[audioClip.samples];
				audioClip.GetData(dataSum, 0);
				otherData = new float[otherClip.samples];
				otherClip.GetData(otherData, 0);
			}
			else
			{
				dataSum = new float[otherClip.samples];
				otherClip.GetData(dataSum, 0);
				otherData = new float[audioClip.samples];
				audioClip.GetData(otherData, 0);
			}

			for (int i = 0; i < otherData.Length; i++)
				dataSum[i] += otherData[i];

			clipSum.SetData(dataSum, 0);

			return clipSum;
		}

		public static void GetUntangledData(this AudioClip audioClip, out float[] dataLeft, out float[] dataRight, int offsetSamples, int amountOfValues)
		{
			float[] data = new float[amountOfValues];
			audioClip.GetData(data, offsetSamples);

			if (audioClip.channels > 1)
			{
				dataLeft = new float[amountOfValues / 2];
				dataRight = new float[amountOfValues / 2];

				for (int i = 0, j = 0; i < amountOfValues - 1; i += 2, j += 1)
				{
					dataLeft[j] = data[i];
					dataRight[j] = data[i + 1];
				}
			}
			else
			{
				dataLeft = data;
				dataRight = new float[0];
			}

			data = null;
		}

		public static void GetUntangledData(this AudioClip audioClip, out float[] dataLeft, out float[] dataRight)
		{
			audioClip.GetUntangledData(out dataLeft, out dataRight, 0, audioClip.samples * audioClip.channels);
		}
	}
}
