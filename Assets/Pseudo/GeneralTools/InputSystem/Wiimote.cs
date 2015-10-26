using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using System.Runtime.InteropServices;

public static class Wiimote
{
	static Wiimote()
	{
		wiimote_start();
	}

	[DllImport("UniWii")]
	static extern void wiimote_start();
	[DllImport("UniWii")]
	static extern void wiimote_stop();
	[DllImport("UniWii")]
	static extern int wiimote_count();
	[DllImport("UniWii")]
	static extern byte wiimote_getAccX(int which);
	[DllImport("UniWii")]
	static extern byte wiimote_getAccY(int which);
	[DllImport("UniWii")]
	static extern byte wiimote_getAccZ(int which);
	[DllImport("UniWii")]
	static extern float wiimote_getIrX(int which);
	[DllImport("UniWii")]
	static extern float wiimote_getIrY(int which);
	[DllImport("UniWii")]
	static extern float wiimote_getRoll(int which);
	[DllImport("UniWii")]
	static extern float wiimote_getPitch(int which);
	[DllImport("UniWii")]
	static extern float wiimote_getYaw(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonA(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonB(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonUp(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonLeft(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonRight(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonDown(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButton1(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButton2(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonPlus(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonMinus(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonHome(int which);
	[DllImport("UniWii")]
	static extern byte wiimote_getNunchuckStickX(int which);
	[DllImport("UniWii")]
	static extern byte wiimote_getNunchuckStickY(int which);
	[DllImport("UniWii")]
	static extern byte wiimote_getNunchuckAccX(int which);
	[DllImport("UniWii")]
	static extern byte wiimote_getNunchuckAccZ(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonNunchuckC(int which);
	[DllImport("UniWii")]
	static extern bool wiimote_getButtonNunchuckZ(int which);

	public static int GetCount()
	{
		return wiimote_count();
	}

	public static bool GetButtonDown()
	{
		bool pressed = false;

		for (int i = 0; i < 16; i++)
			pressed |= wiimote_getButtonA(i);

		return pressed;
	}
}
