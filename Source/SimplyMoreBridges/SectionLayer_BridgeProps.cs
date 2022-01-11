using UnityEngine;
using Verse;

namespace SimplyMoreBridges;

[StaticConstructorOnStartup]
public abstract class SectionLayer_BridgeProps : SectionLayer
{
    protected SectionLayer_BridgeProps(Section section)
        : base(section)
    {
        relevantChangeTypes = MapMeshFlag.Terrain;
    }

    public override bool Visible => DebugViewSettings.drawTerrain;

    protected abstract Material PropsLoopMat { get; }

    protected abstract Material PropsRightMat { get; }

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

    protected abstract bool IsTerrainThisBridge(TerrainDef terrain);

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
                result = !IsTerrainThisBridge(terrain)
                         && (c2.SupportsStructureType(Map, TerrainAffordanceDefOf.Bridgeable)
                             || c2.SupportsStructureType(Map, TerrainAffordanceDefOf.BridgeableDeep));
            }
        }

        return result;
    }
}