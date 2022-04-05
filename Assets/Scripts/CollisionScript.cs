using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionScript : MonoBehaviour {
	//This is hardcoded due to limitations of HLSL
	//If you change this value, update the array and variables in the shader as well
	private int maxRipples = 100;
	private int waveNumber = 0;
	private float[] _Amplitudes;
	private float[] _OffsetX;
	private float[] _OffsetZ;
	private float[] _OffsetY;
	[Range(0.1f,2.0f)]
	public float initialRippleSize = 0.7f;
	private List<ParticleCollisionEvent> collisionEvents;
	private Material shaderMaterial;

	void Start () {

		this.shaderMaterial = GetComponent<Renderer>().material;
		this.shaderMaterial.SetInt("_maxRipples", this.maxRipples);

		_Amplitudes = new float[this.maxRipples];
		_OffsetX = new float[this.maxRipples]; 
		_OffsetZ = new float[this.maxRipples];
		_OffsetY = new float[this.maxRipples]; 

        collisionEvents = new List<ParticleCollisionEvent>();

		for (int i=0; i < maxRipples; i++)
		{
			_OffsetX[i] = 0;
			_OffsetZ[i] = 0;
			_OffsetY[i] = 0;
			_Amplitudes[i] = 0;
		}
	}
	
	void Update () {
	
		for (int i=0; i<100; i++)
		{
			if (_Amplitudes[i] > 0)
			{
				_Amplitudes[i] *= 0.98f;
			}
			if (_Amplitudes[i] < 0.01)
			{
				_Amplitudes[i] = 0;
			}
		}

		this.shaderMaterial.SetFloatArray("_Amplitudes", _Amplitudes);
		this.shaderMaterial.SetFloatArray("_XImpacts", _OffsetX);
		this.shaderMaterial.SetFloatArray("_ZImpacts", _OffsetZ);
		this.shaderMaterial.SetFloatArray("_YImpacts", _OffsetY);
	}

	void OnParticleCollision(GameObject other)
	{
		other.GetComponent<ParticleSystem>().GetCollisionEvents(this.gameObject, collisionEvents);

		foreach (ParticleCollisionEvent pce in collisionEvents)
		{
			if (waveNumber == maxRipples){
				waveNumber = 0;
			}

			//this._Amplitudes[waveNumber] = 0;
			this._OffsetX[waveNumber] = ((-this.transform.position.x + pce.intersection.x) / this.transform.localScale.x);
			this._OffsetZ[waveNumber] = ((-this.transform.position.z + pce.intersection.z) / this.transform.localScale.z);
			this._OffsetY[waveNumber] = ((-this.transform.position.y + pce.intersection.y) / this.transform.localScale.y);
			this._Amplitudes[waveNumber] = initialRippleSize;

			waveNumber++;
		}
	}
}