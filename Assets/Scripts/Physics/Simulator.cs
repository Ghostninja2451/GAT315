using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
	[SerializeField] BoolData simulate;
	[SerializeField] List<Force> forces;
	[SerializeField] IntData fixedFPS;
	[SerializeField] StringData fps;
	public List<Body> bodies  { get; set; } = new List<Body>();
	public float fixedDeltaTime { get => 1.0f/fixedFPS.value; }
	
	Camera activeCamera;
	float timeAccumulator = 0;

    private void Start()
	{
		activeCamera = Camera.main;
	}

    private void Update()
    {
		//get fps
		fps.value = (1.0f / Time.deltaTime).ToString("F2");


		if (!simulate.value) return; 

		//add current delta time to time accumulator
		timeAccumulator += Time.deltaTime;

		//apply force to bodies
		forces.ForEach(force => force.ApplyForce(bodies));

		while (timeAccumulator >= fixedDeltaTime)
		{
			//bodies.ForEach(body => body.shape.color = Color.white);
			Collision.CreateContact(bodies, out var contacts);
			//contacts.ForEach(contact =>
			//{
			//	contact.bodyA.shape.color = Color.green;
			//	contact.bodyB.shape.color = Color.blue;

			//});
			Collision.SeparateContacts(contacts);
			Collision.ApplyImpulses(contacts);

			bodies.ForEach(body =>
			{
				Intergrator.SemiImplicitEuler(body, fixedDeltaTime);
				body.position = body.position.Wrap(-GetScreenSize() * 0.5f, GetScreenSize() * 0.5f);
			});
			timeAccumulator -= fixedDeltaTime;
		}
		foreach (var b in bodies)
        {
			b.acceleration = Vector2.zero;

        }
	}

    public Body GetScreenToBody(Vector3 screen)
    {
		Body body = null;

		Ray ray = activeCamera.ScreenPointToRay(screen);
		RaycastHit2D hit =  Physics2D.GetRayIntersection(ray);
		if(hit.collider)
        {
			hit.collider.gameObject.TryGetComponent<Body>(out body);
        }


		return body;
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
	{
		Vector3 world = activeCamera.ScreenToWorldPoint(screen);
		return new Vector3(world.x, world.y, 0);
	}
	
	public Vector2 GetScreenSize()
    {
		return activeCamera.ViewportToWorldPoint(Vector2.one) * 2;
    }
	public void Clear()
    {
		bodies.ForEach(body => Destroy(body.gameObject));
		bodies.Clear();
    }

}
