using System.Collections.Generic;
using UnityEngine;

public class ScaleGUI
{
	public static bool scaleScreen;

	public static float WIDTH;

	public static float HEIGHT;

	internal static List<Matrix4x4> stack = new List<Matrix4x4>();

	public static void initScaleGUI()
	{
		Cout.println("Init Scale GUI: Screen.w=" + Screen.width + " Screen.h=" + Screen.height);
		WIDTH = Screen.width;
		HEIGHT = Screen.height;
		scaleScreen = false;
		if (Screen.width <= 1200)
			;
	}

	public static void BeginGUI()
	{
		if (scaleScreen)
		{
			stack.Add(GUI.matrix);
			Matrix4x4 matrix4x = default(Matrix4x4);
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = 1f;
			Vector3 zero = Vector3.zero;
			num2 = ((!(num < WIDTH / HEIGHT)) ? ((float)Screen.height / HEIGHT) : ((float)Screen.width / WIDTH));
			matrix4x.SetTRS(zero, Quaternion.identity, Vector3.one * num2);
			GUI.matrix *= matrix4x;
		}
	}

	public static void EndGUI()
	{
		if (scaleScreen)
		{
			GUI.matrix = stack[stack.Count - 1];
			stack.RemoveAt(stack.Count - 1);
		}
	}

	public static float scaleX(float x)
	{
		if (!scaleScreen)
			return x;
		x = x * WIDTH / (float)Screen.width;
		return x;
	}

	public static float scaleY(float y)
	{
		if (!scaleScreen)
			return y;
		y = y * HEIGHT / (float)Screen.height;
		return y;
	}
}
