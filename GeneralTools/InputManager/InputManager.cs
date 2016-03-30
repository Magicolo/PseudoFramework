using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Input;
using UnityEngine.Assertions;

namespace Pseudo.Internal
{
	public class InputManager : MonoBehaviour, IInputManager
	{
		public PlayerInput[] Inputs = new PlayerInput[0];

		readonly Dictionary<string, PlayerInput> unassignedInputs = new Dictionary<string, PlayerInput>();
		readonly Dictionary<Players, PlayerInput> assignedInputs = new Dictionary<Players, PlayerInput>(PEqualityComparer<Players>.Default);

		void Awake()
		{
			for (int i = 0; i < Inputs.Length; i++)
			{
				var playerInput = Instantiate(Inputs[i]);
				playerInput.transform.parent = transform;
				AddInput(playerInput);

				if (playerInput.Player != Players.None)
					AssignInput(playerInput.Player, playerInput);
			}
		}

		public PlayerInput GetInput(string inputName)
		{
			PlayerInput playerInput;

			if (!unassignedInputs.TryGetValue(inputName, out playerInput))
				Debug.LogError(string.Format("PlayerInput named {0} was not found.", inputName));

			return playerInput;
		}

		public PlayerInput GetAssignedInput(Players player)
		{
			PlayerInput playerInput;

			if (!assignedInputs.TryGetValue(player, out playerInput))
				Debug.LogError(string.Format("No PlayerInput has been assigned to {0}.", player));

			return playerInput;
		}

		public void AssignInput(Players player, string inputName)
		{
			AssignInput(player, GetInput(inputName));
		}

		public void AssignInput(Players player, PlayerInput input)
		{
			Assert.IsNotNull(input);
			input.Player = player;
			assignedInputs[player] = input;
		}

		public void UnassignInput(Players player)
		{
			PlayerInput playerInput;

			if (assignedInputs.Pop(player, out playerInput))
				playerInput.Player = Players.None;
		}

		public bool IsAssigned(Players player)
		{
			return assignedInputs.ContainsKey(player);
		}

		public void AddInput(PlayerInput input)
		{
			Assert.IsNotNull(input);
			unassignedInputs[input.name] = input;
		}

		public bool GetKeyDown(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKeyDown();
		}

		public bool GetKeyUp(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKeyUp();
		}

		public bool GetKey(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKey();
		}

		public float GetAxis(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetAxis();
		}

		public bool GetKeyDown(Players player, string actionName, Vector2 relativeScreenPosition)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKeyDown(relativeScreenPosition);
		}

		public bool GetKeyUp(Players player, string actionName, Vector2 relativeScreenPosition)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKeyUp(relativeScreenPosition);
		}

		public bool GetKey(Players player, string actionName, Vector2 relativeScreenPosition)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKey(relativeScreenPosition);
		}

		public float GetAxis(Players player, string actionName, Vector2 relativeScreenPosition)
		{
			return GetAssignedInput(player).GetAction(actionName).GetAxis(relativeScreenPosition);
		}
	}
}