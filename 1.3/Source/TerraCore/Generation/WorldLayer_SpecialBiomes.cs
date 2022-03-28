using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TerraCore
{
	public class WorldLayer_SpecialBiomes : WorldLayer
	{
		public override IEnumerable Regenerate()
		{
			foreach (object item in base.Regenerate())
			{
				yield return item;
			}
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			WorldGrid grid = Find.WorldGrid;
			int tilesCount = grid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile tile = grid[i];
				BiomeWorkerSpecial biomeWorkerSpecial = tile.biome.WorkerSpecial();
				if (biomeWorkerSpecial == null)
				{
					continue;
				}
				WLTileGraphicData wLTileGraphicData = biomeWorkerSpecial.GetWLTileGraphicData(grid, i);
				if (wLTileGraphicData != null)
				{
					LayerSubMesh subMesh = GetSubMesh(wLTileGraphicData.material);
					if (wLTileGraphicData.drawAsQuad)
					{
						Vector3 tileCenter = grid.GetTileCenter(i);
						Vector3 val = tileCenter + Rand.UnitVector3 * wLTileGraphicData.posOffset * grid.averageTileSize;
                        Vector3 pos = ((Vector3)(val)).normalized * ((Vector3)(tileCenter)).magnitude;
						TerraCore.WorldRendererUtility.PrintQuadTangentialToPlanetWithRodation(pos, tileCenter, wLTileGraphicData.sizeFactor * grid.averageTileSize, 0.005f, subMesh, wLTileGraphicData.rotVector);
						RimWorld.Planet.WorldRendererUtility.PrintTextureAtlasUVs(wLTileGraphicData.atlasX, wLTileGraphicData.atlasZ, wLTileGraphicData.texturesInAtlasX, wLTileGraphicData.texturesInAtlasZ, subMesh);
					}
					else
					{
						TerraCore.WorldRendererUtility.DrawTileTangentialToPlanetWithRodation(texturesInAtlas: new IntVec2(wLTileGraphicData.texturesInAtlasX, wLTileGraphicData.texturesInAtlasZ), grid: grid, subMesh: subMesh, tileID: i, atlasX: wLTileGraphicData.atlasX, atlasZ: wLTileGraphicData.atlasZ, rotDir: wLTileGraphicData.rotDir);
					}
				}
			}
			Rand.PopState();
			FinalizeMesh(MeshParts.All);
		}
	}
}
