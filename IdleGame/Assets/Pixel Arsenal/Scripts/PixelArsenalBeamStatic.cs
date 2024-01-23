using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelArsenal
{

public class PixelArsenalBeamStatic : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab; //Put a prefab with a line renderer onto here.
    public GameObject beamStartPrefab; //This is a prefab that is put at the start of the beam.
    public GameObject beamEndPrefab; //Prefab put at end of beam.

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;

    [Header("Beam Options")]
    public bool beamCollides = true; //Beam stops at colliders
    public float beamLength = 100; //Ingame beam length
    public float beamEndOffset = 0f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. Example: if texture is 200 pixels in height and 600 in length, set this to 3
	
	[Header("Width Pulse Options")]
	public float widthMultiplier = 1.5f;
	private float customWidth;
	private float originalWidth;
	private float lerpValue = 0.0f;
	public float pulseSpeed = 1.0f;
	private bool pulseExpanding = true;

    void Start()
    {
		SpawnBeam();
		originalWidth = line.startWidth;
		customWidth = originalWidth * widthMultiplier;
    }

    void FixedUpdate()
	{
		if (beam) 
		{
			line.SetPosition(0, transform.position);
			Vector3 end = transform.position + (transform.forward * beamLength);
			RaycastHit hit;
			
			if (beamCollides && Physics.Raycast(transform.position, transform.forward, out hit))
			{
				end = hit.point - (transform.forward * beamEndOffset);
				end = Vector3.Distance(transform.position, end) > beamLength 
					? transform.position + (transform.forward * beamLength) 
					: end;
			}
			else
			{
				end = transform.position + (transform.forward * beamLength);
			}
				
			line.SetPosition(1, end);
			beamStart.transform.position = transform.position;
			beamStart.transform.LookAt(end);
			beamEnd.transform.position = end;
			beamEnd.transform.LookAt(beamStart.transform.position);
			float distance = Vector3.Distance(transform.position, end);
			line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
			line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
		}
		
		// Pulse the width of the beam
		if (pulseExpanding) 
		{
			lerpValue += Time.deltaTime * pulseSpeed;
		} 
		else 
		{
			lerpValue -= Time.deltaTime * pulseSpeed;
		}

		if (lerpValue >= 1.0f) 
		{
			pulseExpanding = false;
			lerpValue = 1.0f;
		} 
		else if (lerpValue <= 0.0f) 
		{
			pulseExpanding = true;
			lerpValue = 0.0f;
		}

		float currentWidth = Mathf.Lerp(originalWidth, customWidth, Mathf.Sin(lerpValue * Mathf.PI));
		
		line.startWidth = currentWidth;
		line.endWidth = currentWidth;
	}

    public void SpawnBeam()
	{
		if (beamLineRendererPrefab)
		{
			beam = Instantiate(beamLineRendererPrefab);
			beam.transform.position = transform.position;
			beam.transform.parent = transform;
			beam.transform.rotation = transform.rotation;

			line = beam.GetComponent<LineRenderer>();
			line.useWorldSpace = true;

			#if UNITY_5_5_OR_NEWER
			line.positionCount = 2;
			#else
			line.SetVertexCount(2); 
			#endif

			beamStart = beamStartPrefab ? Instantiate(beamStartPrefab, beam.transform) : null;
			beamEnd = beamEndPrefab ? Instantiate(beamEndPrefab, beam.transform) : null;
		}
		else
		{
			Debug.LogError("A prefab with a line renderer must be assigned to the `beamLineRendererPrefab` field in the PixelArsenalBeamStatic script on " + gameObject.name);
		}
	}

}
}