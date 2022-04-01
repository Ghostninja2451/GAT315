using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	public Vector2 gravity = new Vector2(0, -9.8f);


	public List<Body> bodies = new List<Body>();
	Camera activeCamera;

	private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
        foreach(var b in bodies)
        {
			b.Step(Time.deltaTime);
			Intergrator.SemiImplicitEuler(b, Time.deltaTime);
        }
		foreach(var b in bodies)
        {
			b.force = Vector2.zero;
        }
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}
}
