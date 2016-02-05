using Pseudo.Internal;
using UnityEngine;

namespace Pseudo
{
	public interface IInputManager
	{
		void AddInput(PlayerInput input);
		void AssignInput(InputManager.Players player, string inputName);
		void AssignInput(InputManager.Players player, PlayerInput input);
		PlayerInput GetAssignedInput(InputManager.Players player);
		float GetAxis(InputManager.Players player, string actionName);
		float GetAxis(InputManager.Players player, string actionName, Vector2 relativeScreenPosition);
		PlayerInput GetInput(string inputName);
		bool GetKey(InputManager.Players player, string actionName);
		bool GetKey(InputManager.Players player, string actionName, Vector2 relativeScreenPosition);
		bool GetKeyDown(InputManager.Players player, string actionName);
		bool GetKeyDown(InputManager.Players player, string actionName, Vector2 relativeScreenPosition);
		bool GetKeyUp(InputManager.Players player, string actionName);
		bool GetKeyUp(InputManager.Players player, string actionName, Vector2 relativeScreenPosition);
		bool IsAssigned(InputManager.Players player);
		void UnassignInput(InputManager.Players player);
	}
}