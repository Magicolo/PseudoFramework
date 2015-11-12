﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using Pseudo.Internal;
using Pseudo.Internal.Input;

namespace Pseudo
{
	public class InputManager : Singleton<InputManager>
	{
		public enum ControllerTypes
		{
			Keyboard,
			Joystick
		}

		public enum Joysticks
		{
			Any = 0,
			Joystick1 = 1,
			Joystick2 = 2,
			Joystick3 = 3,
			Joystick4 = 4,
			Joystick5 = 5,
			Joystick6 = 6,
			Joystick7 = 7,
			Joystick8 = 8,
		}

		public enum JoystickButtons
		{
			Cross_A = 0,
			Circle_B = 1,
			Square_X = 2,
			Triangle_Y = 3,
			L1 = 4,
			R1 = 5,
			Select = 6,
			Start = 7,
			LeftStick = 8,
			RigthStick = 9,
			Button10 = 10,
			Button11 = 11,
			Button12 = 12,
			Button13 = 13,
			Button14 = 14,
			Button15 = 15,
			Button16 = 16,
			Button17 = 17,
			Button18 = 18,
			Button19 = 19
		}

		public enum JoystickAxes
		{
			LeftStickX = 0,
			LeftStickY = 1,
			RightStickX = 3,
			RightStickY = 4,
			DirectionalPadX = 5,
			DirectionalPadY = 6,
			LeftTrigger = 7,
			RightTrigger = 8
		}

		public enum Players
		{
			None,
			Player1,
			Player2,
			Player3,
			Player4,
			Player5,
			Player6,
			Player7,
			Player8
		}

		public PlayerInput[] Inputs = new PlayerInput[0];

		protected readonly Dictionary<string, PlayerInput> unassignedInputs = new Dictionary<string, PlayerInput>();
		protected readonly Dictionary<Players, PlayerInput> assignedInputs = new Dictionary<Players, PlayerInput>();

		protected override void Awake()
		{
			base.Awake();

			for (int i = 0; i < Inputs.Length; i++)
			{
				PlayerInput playerInput = Instantiate(Inputs[i]);
				playerInput.CachedTransform.parent = CachedTransform;
				AddInput(playerInput);

				if (playerInput.Player != Players.None)
					AssignInput(playerInput.Player, playerInput);
			}
		}

		public virtual PlayerInput GetInput(string inputName)
		{
			PlayerInput playerInput;

			if (!unassignedInputs.TryGetValue(inputName, out playerInput))
				Debug.LogError(string.Format("PlayerInput named {0} was not found.", inputName));

			return playerInput;
		}

		public virtual PlayerInput GetAssignedInput(Players player)
		{
			PlayerInput playerInput;

			if (!assignedInputs.TryGetValue(player, out playerInput))
				Debug.LogError(string.Format("No PlayerInput has been assigned to {0}.", player));

			return playerInput;
		}

		public virtual void AssignInput(Players player, string inputName)
		{
			AssignInput(player, GetInput(inputName));
		}

		public virtual void AssignInput(Players player, PlayerInput input)
		{
			input.Player = player;
			assignedInputs[player] = input;
		}

		public virtual void UnassignInput(Players player)
		{
			PlayerInput playerInput;

			if (assignedInputs.Pop(player, out playerInput))
				playerInput.Player = Players.None;
		}

		public virtual bool IsAssigned(Players player)
		{
			return assignedInputs.ContainsKey(player);
		}

		public virtual void AddInput(PlayerInput inputPrefab)
		{
			PlayerInput input = Instantiate(inputPrefab);
			input.CachedTransform.parent = CachedTransform;
			unassignedInputs[inputPrefab.name] = inputPrefab;
		}

		public virtual bool GetKeyDown(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKeyDown();
		}

		public virtual bool GetKeyUp(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKeyUp();
		}

		public virtual bool GetKey(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetKey();
		}

		public virtual float GetAxis(Players player, string actionName)
		{
			return GetAssignedInput(player).GetAction(actionName).GetAxis();
		}
	}
}