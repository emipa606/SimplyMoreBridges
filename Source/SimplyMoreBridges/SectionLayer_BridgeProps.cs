using UnityEngine;
using Verse;

namespace SimplyMoreBridges
{
    // Token: 0x02000005 RID: 5
    [StaticConstructorOnStartup]
    public abstract class SectionLayer_BridgeProps : SectionLayer
    {
        // Token: 0x06000004 RID: 4 RVA: 0x00002089 File Offset: 0x00000289
        protected SectionLayer_BridgeProps(Section section) : base(section)
        {
            relevantChangeTypes = MapMeshFlag.Terrain;
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000005 RID: 5
        protected abstract Material PropsLoopMat { get; }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000006 RID: 6
        protected abstract Material PropsRightMat { get; }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000008 RID: 8 RVA: 0x000020A0 File Offset: 0x000002A0
        public override bool Visible => DebugViewSettings.drawTerrain;

        // Token: 0x06000007 RID: 7
        protected abstract bool IsTerrainThisBridge(TerrainDef terrain);

        // Token: 0x06000009 RID: 9 RVA: 0x000020B8 File Offset: 0x000002B8
        public override void Regenerate()
        {
            ClearSubMeshes(MeshParts.All);
            var terrainGrid = Map.terrainGrid;
            var cellRect = section.CellRect;
            var num = AltitudeLayer.TerrainScatter.AltitudeFor();
            foreach (var intVec in cellRect)
            {
                if (!ShouldDrawPropsBelow(intVec, terrainGrid))
                {
                    continue;
                }

                var c = intVec;
                c.x++;
                Material material;
                if (c.InBounds(Map) && ShouldDrawPropsBelow(c, terrainGrid))
                {
                    material = PropsLoopMat;
                }
                else
                {
                    material = PropsRightMat;
                }

                var subMesh = GetSubMesh(material);
                var count = subMesh.verts.Count;
                subMesh.verts.Add(new Vector3(intVec.x, num, intVec.z - 1));
                subMesh.verts.Add(new Vector3(intVec.x, num, intVec.z));
                subMesh.verts.Add(new Vector3(intVec.x + 1, num, intVec.z));
                subMesh.verts.Add(new Vector3(intVec.x + 1, num, intVec.z - 1));
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

            FinalizeMesh(MeshParts.All);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002320 File Offset: 0x00000520
        private bool ShouldDrawPropsBelow(IntVec3 c, TerrainGrid terrGrid)
        {
            var terrainDef = terrGrid.TerrainAt(c);
            bool result;
            if (terrainDef == null || !IsTerrainThisBridge(terrainDef))
            {
                result = false;
            }
            else
            {
                var c2 = c;
                c2.z--;
                if (!c2.InBounds(Map))
                {
                    result = false;
                }
                else
                {
                    var terrain = terrGrid.TerrainAt(c2);
                    result = !IsTerrainThisBridge(terrain) &&
                             (c2.SupportsStructureType(Map, TerrainAffordanceDefOf.Bridgeable) ||
                              c2.SupportsStructureType(Map, TerrainAffordanceDefOf.BridgeableDeep));
                }
            }

            return result;
        }
    }
}