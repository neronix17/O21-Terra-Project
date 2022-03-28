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
	public class WorldLayer_Mountains : WorldLayer
	{
		private static readonly IntVec2 TexturesInAtlas = new IntVec2(7, 2);

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
				if (tile.hilliness != Hilliness.Impassable && tile.biome != BiomeDefOf.CaveOasis && tile.biome != BiomeDefOf.CaveEntrance && tile.biome != BiomeDefOf.TunnelworldCave)
				{
					continue;
				}
				Material hillinessImpassable = TerraCore.WorldMaterials.HillinessImpassable;
				int atlasX = 0;
				int atlasZ = 0;
				int rotDir = 0;
				grid.GetTileGraphicDataFromNeighbors(i, out atlasX, out atlasZ, out rotDir, (Tile tileFrom, Tile neighbor) => neighbor.hilliness == Hilliness.Impassable || neighbor.biome == BiomeDefOf.CaveOasis || neighbor.biome == BiomeDefOf.CaveEntrance || neighbor.biome == BiomeDefOf.TunnelworldCave);
				TerraCore.WorldRendererUtility.DrawTileTangentialToPlanetWithRodation(grid, GetSubMesh(hillinessImpassable), i, atlasX, atlasZ, TexturesInAtlas, rotDir);
				if (tile.hilliness != Hilliness.Impassable)
				{
					hillinessImpassable = TerraCore.WorldMaterials.CaveSystem;
					grid.GetTileGraphicDataFromNeighbors(i, out atlasX, out atlasZ, out rotDir, (Tile tileFrom, Tile neighbor) => neighbor.biome == BiomeDefOf.CaveOasis || neighbor.biome == BiomeDefOf.CaveEntrance || neighbor.biome == BiomeDefOf.TunnelworldCave);
					TerraCore.WorldRendererUtility.DrawTileTangentialToPlanetWithRodation(grid, GetSubMesh(hillinessImpassable), i, atlasX, atlasZ, TexturesInAtlas, rotDir);
				}
			}
			Rand.PopState();
			FinalizeMesh(MeshParts.All);
		}
	}
}
