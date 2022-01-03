
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CableComponentUdon : UdonSharpBehaviour
{
    #region Class members

    [Header("Wire Settings")]
	[SerializeField] private Transform endPoint;
	[SerializeField] private Material cableMaterial;

	// Cable config
	[SerializeField] private float cableLength = 0.5f;
	[SerializeField] private int totalSegments = 5;
	[SerializeField] private float segmentsPerUnit = 2f;
	private int segments = 0;
	
	[SerializeField] private float cableWidth = 0.1f;

	// Solver config
	[SerializeField] private int verletIterations = 1;
	[SerializeField] private int solverIterations = 1;
    
    // Needed GameObjects
    [Header("Required GameObjects")]
    public GameObject cableParticleUdon;

	private LineRenderer line;
	private CableParticleUdon[] points;

	#endregion


	#region Initial setup

	void Start()
	{
		InitCableParticleUdon();
		InitLineRenderer();
	}

	/**
	 * Init cable particles
	 * 
	 * Creates the cable particles along the cable length
	 * and binds the start and end tips to their respective game objects.
	 */
	void InitCableParticleUdon()
	{
		// Calculate segments to use
		if (totalSegments > 0)
			segments = totalSegments;
		else
			segments = Mathf.CeilToInt (cableLength * segmentsPerUnit);

		Vector3 cableDirection = (endPoint.position - transform.position).normalized;
		float initialSegmentLength = cableLength / segments;
		points = new CableParticleUdon[segments + 1];

		// Foreach point
		for (int pointIdx = 0; pointIdx <= segments; pointIdx++) {
			// Initial position
			Vector3 initialPosition = transform.position + (cableDirection * (initialSegmentLength * pointIdx));
			CableParticleUdon particle = transform.GetChild(pointIdx).GetComponent<CableParticleUdon>();
			particle.CreateCableParticle(initialPosition);
			points[pointIdx] = particle;
		}

		// Bind start and end particles with their respective gameobjects
		CableParticleUdon start = points[0];
		CableParticleUdon end = points[segments];
		start.Bind(this.transform);
		end.Bind(endPoint.transform);
	}

	/**
	 * Initialized the line renderer
	 */
	void InitLineRenderer()
	{
		line = GetComponent<LineRenderer>();
		line.startWidth = cableWidth;
		line.endWidth = cableWidth;
		line.positionCount = segments + 1;
		line.material = cableMaterial;
		line.GetComponent<Renderer>().enabled = true;
	}

	#endregion


	#region Render Pass

	void Update()
	{
		RenderCable();
	}

	/**
	 * Render Cable
	 * 
	 * Update every particle position in the line renderer.
	 */
	void RenderCable()
	{
		for (int pointIdx = 0; pointIdx < segments + 1; pointIdx++) 
		{
		  line.SetPosition(pointIdx, points [pointIdx].Position);
		}
	}

	#endregion


	#region Verlet integration & solver pass

	void FixedUpdate()
	{
		for (int verletIdx = 0; verletIdx < verletIterations; verletIdx++) 
		{
			VerletIntegrate();
			SolveConstraints();
		}
	}

	/**
	 * Verler integration pass
	 * 
	 * In this step every particle updates its position and speed.
	 */
	void VerletIntegrate()
	{
		Vector3 gravityDisplacement = Time.fixedDeltaTime * Time.fixedDeltaTime * Physics.gravity;
		foreach (CableParticleUdon particle in points) 
		{
			particle.UpdateVerlet(gravityDisplacement);
		}
	}

	/**
	 * Constrains solver pass
	 * 
	 * In this step every constraint is addressed in sequence
	 */
	void SolveConstraints()
	{
		// For each solver iteration..
		for (int iterationIdx = 0; iterationIdx < solverIterations; iterationIdx++) 
		{
			SolveDistanceConstraint();
		}
	}

	#endregion


	#region Solver Constraints

	/**
	 * Distance constraint for each segment / pair of particles
	 **/
	void SolveDistanceConstraint()
	{
		float segmentLength = cableLength / segments;
		for (int SegIdx = 0; SegIdx < segments; SegIdx++) 
		{
			CableParticleUdon particleA = points[SegIdx];
			CableParticleUdon particleB = points[SegIdx + 1];

			// Solve for this pair of particles
			SolveDistanceConstraintValues(particleA, particleB, segmentLength);
		}
	}
		
	/**
	 * Distance Constraint 
	 * 
	 * This is the main constrains that keeps the cable particles "tied" together.
	 */
	void SolveDistanceConstraintValues(CableParticleUdon particleA, CableParticleUdon particleB, float segmentLength)
	{
		// Find current vector between particles
		Vector3 delta = particleB.Position - particleA.Position;
		// 
		float currentDistance = delta.magnitude;
		float errorFactor = (currentDistance - segmentLength) / currentDistance;
		
		// Only move free particles to satisfy constraints
		if (particleA.IsFree() && particleB.IsFree()) 
		{
			particleA.Position += errorFactor * 0.5f * delta;
			particleB.Position -= errorFactor * 0.5f * delta;
		} 
		else if (particleA.IsFree()) 
		{
			particleA.Position += errorFactor * delta;
		} 
		else if (particleB.IsFree()) 
		{
			particleB.Position -= errorFactor * delta;
		}
	}

	#endregion
}
