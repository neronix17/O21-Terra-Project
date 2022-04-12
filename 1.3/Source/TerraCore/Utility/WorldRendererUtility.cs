using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public static class WorldRendererUtility
	{
		private static List<Vector3> tmpVerts = new List<Vector3>();

		public static void DrawTileTangentialToPlanetWithRodation(WorldGrid grid, LayerSubMesh subMesh, int tileID, int atlasX, int atlasZ, IntVec2 texturesInAtlas, int rotDir)
		{
			int count = subMesh.verts.Count;
			grid.GetTileVertices(tileID, tmpVerts);
			int count2 = tmpVerts.Count;
			if (rotDir < 0)
			{
				rotDir += count2;
			}
			for (int i = 0; i < count2; i++)
			{
				int index = (i + rotDir) % count2;
				List<Vector3> verts = subMesh.verts;
				Vector3 val = tmpVerts[index];
				Vector3 val2 = tmpVerts[index];
				verts.Add(val + ((Vector3)(val2)).normalized * 0.012f);
				Vector2 val3 = (GenGeo.RegularPolygonVertexPosition(count2, i) + Vector2.one) / 2f;
				val3.x = (val3.x + (float)atlasX) / (float)texturesInAtlas.x;
				val3.y = (val3.y + (float)atlasZ) / (float)texturesInAtlas.z;
				subMesh.uvs.Add(val3);
				if (i < count2 - 2)
				{
					subMesh.tris.Add(count + i + 2);
					subMesh.tris.Add(count + i + 1);
					subMesh.tris.Add(count);
				}
			}
		}

		public static void PrintQuadTangentialToPlanetWithRodation(Vector3 pos, Vector3 posForTangents, float size, float altOffset, LayerSubMesh subMesh, Vector3 rotVec)
		{
			GetTangentsToPlanetWithRotation(posForTangents, out var first, out var second, rotVec);
			Vector3 normalized = ((Vector3)(posForTangents)).normalized;
			float num = size * 0.5f;
			Vector3 item = pos - first * num - second * num + normalized * altOffset;
			Vector3 item2 = pos - first * num + second * num + normalized * altOffset;
			Vector3 item3 = pos + first * num + second * num + normalized * altOffset;
			Vector3 item4 = pos + first * num - second * num + normalized * altOffset;
			int count = subMesh.verts.Count;
			subMesh.verts.Add(item);
			subMesh.verts.Add(item2);
			subMesh.verts.Add(item3);
			subMesh.verts.Add(item4);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		public static void GetTangentsToPlanetWithRotation(Vector3 pos, out Vector3 first, out Vector3 second, Vector3 rotVec)
		{
			Quaternion val = Quaternion.LookRotation(((Vector3)(pos)).normalized, rotVec);
			first = val * Vector3.up;
			second = val * Vector3.right;
		}
	}
}
