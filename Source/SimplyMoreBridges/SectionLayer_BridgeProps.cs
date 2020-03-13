using System;
using UnityEngine;
using Verse;

namespace SimplyMoreBridges
{
	// Token: 0x02000005 RID: 5
	[StaticConstructorOnStartup]
	public abstract class SectionLayer_BridgeProps : SectionLayer
	{
		// Token: 0x06000004 RID: 4 RVA: 0x00002089 File Offset: 0x00000289
		public SectionLayer_BridgeProps(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlag.Terrain;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5
		protected abstract Material PropsLoopMat { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6
		protected abstract Material PropsRightMat { get; }

		// Token: 0x06000007 RID: 7
		protected abstract bool IsTerrainThisBridge(TerrainDef terrain);

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020A0 File Offset: 0x000002A0
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawTerrain;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000020B8 File Offset: 0x000002B8
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			TerrainGrid terrainGrid = base.Map.terrainGrid;
			CellRect cellRect = this.section.CellRect;
			float num = AltitudeLayer.TerrainScatter.AltitudeFor();
			CellRect.CellRectIterator iterator = cellRect.GetIterator();
			while (!iterator.Done())
			{
				IntVec3 intVec = iterator.Current;
				if (this.ShouldDrawPropsBelow(intVec, terrainGrid))
				{
					IntVec3 c = intVec;
					c.x++;
					Material material;
					if (c.InBounds(base.Map) && this.ShouldDrawPropsBelow(c, terrainGrid))
					{
						material = this.PropsLoopMat;
					}
					else
					{
						material = this.PropsRightMat;
					}
					LayerSubMesh subMesh = base.GetSubMesh(material);
					int count = subMesh.verts.Count;
					subMesh.verts.Add(new Vector3((float)intVec.x, num, (float)(intVec.z - 1)));
					subMesh.verts.Add(new Vector3((float)intVec.x, num, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), num, (float)intVec.z));
					subMesh.verts.Add(new Vector3((float)(intVec.x + 1), num, (float)(intVec.z - 1)));
					subMesh.uvs.Add(new Vector2(0f, 0f));
					subMesh.uvs.Add(new Vector2(0f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 1f));
					subMesh.uvs.Add(new Vector2(1f, 0f));
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 1);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count);
					subMesh.tris.Add(count + 2);
					subMesh.tris.Add(count + 3);
				}
				iterator.MoveNext();
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002320 File Offset: 0x00000520
		private bool ShouldDrawPropsBelow(IntVec3 c, TerrainGrid terrGrid)
		{
			TerrainDef terrainDef = terrGrid.TerrainAt(c);
			bool result;
			if (terrainDef == null || !this.IsTerrainThisBridge(terrainDef))
			{
				result = false;
			}
			else
			{
				IntVec3 c2 = c;
				c2.z--;
				if (!c2.InBounds(base.Map))
				{
					result = false;
				}
				else
				{
					TerrainDef terrain = terrGrid.TerrainAt(c2);
					result = (!this.IsTerrainThisBridge(terrain) && (c2.SupportsStructureType(base.Map, TerrainAffordanceDefOf.Bridgeable) || c2.SupportsStructureType(base.Map, TerrainAffordanceDefOf.BridgeableDeep)));
				}
			}
			return result;
		}
	}
}
