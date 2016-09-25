using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class Generator : MonoBehaviour
	{
		public Transform parent;
		public Transform generatorTarget;
		public ObjectPoolerWorld cubePool;
		public ObjectPoolerWorld phillarPool;
		public List<ParticleSystem> particleSystems;
		public int width = 5;
		public int lenght = 20;
		public int phillarWidth;
		public int phillarDensity = 100;
		public int phillarJitter = 3;
		public float jitter = .5f;
		public bool generate = true;
		public bool generateCubes = true;
		public bool generatePhillars = true;
		public bool generateCeiling = false, generateWalls = false;
		public int maxRows = 25;
		public int loopDistance = 500;

		private int curPos;
		private List<GameObject> _cubes;
		private List<GameObject> _phillars;
		private ParticleSystem.Particle[] _particles;
		private Vector3 _phillarPos;
		private Vector3 _cubePos;
		private int _wallCount = 1;

		void Start ()
		{
			_cubes = new List<GameObject>();
			_phillars = new List<GameObject>();
			if (generateCeiling)
				_wallCount++;
			if (generateWalls)
				_wallCount += 2;
		}
	
		void Update ()
		{
			if(generate)
			{

				//Cleanup distant cubes
				while(_cubes.Count > maxRows * width * _wallCount)
				{
					for (int i = 0; i < width * _wallCount; i++)
					{
						if (_cubes[i] == null)
							continue;
						_cubes[i].SetActive(false);
					}
					_cubes.RemoveRange(0, width * _wallCount);
				}
				//Cleanup distant phillars
				while(_phillars.Count > maxRows * phillarWidth )
				{
					for(int i = 0; i < phillarWidth; i++)
					{
						if (_phillars[i] == null)
							continue;
						_phillars[i].SetActive(false);
					}
					_phillars.RemoveRange(0, phillarWidth);
				}

				//Generate new cubes ahead
				while (curPos < generatorTarget.position.z + lenght)
				{
					//Generate
					if (generateCubes)
					{
						//Floor
						GenerateHorizontalPlane(Vector3.zero);
						//Ceiling
						if(generateCeiling)
							GenerateHorizontalPlane(new Vector3(0, phillarJitter));
						//Left Wall
						if(generateWalls)
							GenerateVerticalPlane(new Vector3(-(phillarWidth+1)/2, 1));
						//Right Wall
						if(generateWalls)
							GenerateVerticalPlane(new Vector3((phillarWidth+1)/2, 1));
					}
					if(generatePhillars)
					{
						for (int x = 0; x < phillarWidth; x++)
						{
							if (Random.Range(0, phillarDensity) == 1)
							{
								_phillarPos = new Vector3(x - (phillarWidth / 2), Random.Range(1, phillarJitter), curPos);
								_phillars.Add(phillarPool.Instantiate(_phillarPos, Quaternion.identity, parent));
							}
							else
								_phillars.Add(null);
						}
					}
					curPos++;
				}
			}
		}

		void GenerateHorizontalPlane(Vector3 offset)
		{
			for (int x = 0; x < width; x++)
			{
				_cubePos = new Vector3(x - (width / 2), Random.Range(-jitter, jitter), curPos);
				_cubePos += offset;
				_cubes.Add(cubePool.Instantiate(_cubePos, Quaternion.identity, parent));
			}
		}
		void GenerateVerticalPlane(Vector3 offset)
		{
			for (int x = 0; x < width; x++)
			{
				_cubePos = new Vector3(Random.Range(-jitter, jitter), x - (width / 2), curPos);
				_cubePos += offset;
				_cubes.Add(cubePool.Instantiate(_cubePos, Quaternion.identity, parent));
			}
		}

		void LateUpdate()
		{
			//Shift everything back to allow for infinite generation
			if (generatorTarget.position.z >= loopDistance)
			{
				//Move Camera
				generatorTarget.position = new Vector3
				{
					x = generatorTarget.position.x,
					y = generatorTarget.position.y,
					z = generatorTarget.position.z - loopDistance
				};

				//Cubes
				foreach (GameObject c in _cubes)
				{
					if (c == null)
						continue;
					c.transform.position = new Vector3
					{
						x = c.transform.position.x,
						y = c.transform.position.y,
						z = c.transform.position.z - loopDistance
					};
				}
				//Phillars
				foreach (GameObject p in _phillars)
				{
					if (p == null)
						continue;
					p.transform.position = new Vector3
					{
						x = p.transform.position.x,
						y = p.transform.position.y,
						z = p.transform.position.z - loopDistance
					};
				}

				//Particles

				foreach (ParticleSystem p in particleSystems)
				{
					if (_particles == null || _particles.Length < p.maxParticles)
						_particles = new ParticleSystem.Particle[p.maxParticles];
					int pCount = p.GetParticles(_particles);
					for (int i = 0; i < pCount; i++)
					{
						_particles[i].position = new Vector3
						{
							x = _particles[i].position.x,
							y = _particles[i].position.y,
							z = _particles[i].position.z - loopDistance
						};
					}
					p.SetParticles(_particles, pCount);
				}

				//Reference point
				curPos -= loopDistance;

			}
		}


	}
}

