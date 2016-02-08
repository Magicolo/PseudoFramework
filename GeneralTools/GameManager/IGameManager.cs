using Pseudo.Internal;
using UnityEngine.SceneManagement;

namespace Pseudo
{
	public enum GameStates
	{
		Playing,
		Loading,
	}

	public interface IGameManager
	{
		GameStates State { get; }
		Scene Scene { get; }

		void LoadScene(string scene);
		void ReloadScene();
	}
}