using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public class WLTileGraphicData
	{
		public Material material;

		public int texturesInAtlasX;

		public int texturesInAtlasZ;

		public int atlasX = 0;

		public int atlasZ = 0;

		public float sizeFactor = 1f;

		public float posOffset = 0f;

		public int rotDir = 0;

		public Vector3 rotVector = Vector3.up;

		public bool drawAsQuad = true;

		public WLTileGraphicData()
		{ 
		}


		public WLTileGraphicData(Material material, FloatRange sizeFactor, FloatRange posOffset, bool randomRotation = true)
		{
			this.material = material;
			texturesInAtlasX = material.mainTexture.width / 256;
			texturesInAtlasZ = material.mainTexture.height / 256;
			atlasX = Rand.RangeInclusive(0, texturesInAtlasX - 1);
			atlasZ = Rand.RangeInclusive(0, texturesInAtlasZ - 1);
			this.sizeFactor = sizeFactor.RandomInRange;
			this.posOffset = posOffset.RandomInRange;
			if (randomRotation)
			{
				rotVector = Rand.UnitVector3;
			}
		}

		public WLTileGraphicData(Material material, int atlasX, int atlasZ, int rotDir)
		{
			this.material = material;
			texturesInAtlasX = material.mainTexture.width / 256;
			texturesInAtlasZ = material.mainTexture.height / 256;
			this.atlasX = atlasX;
			this.atlasZ = atlasZ;
			this.rotDir = rotDir;
			drawAsQuad = false;
		}
	}
}
