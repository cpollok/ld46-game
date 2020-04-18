using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Rail : MonoBehaviour {

    public PathCreator path_creator;

    public MeshFilter   mesh_filter;
    public MeshRenderer mesh_renderer;
    public Mesh mesh;

    public float rail_dist  = 0.8f;
    public float rail_width = 0.05f;
    public float rail_height = 0.1f;
    public float tie_width  = 1.0f;
    public float tie_depth  = 0.1f;
    public float tie_dist   = 0.2f;
    public float tie_height = 0.03f;

    // Start is called before the first frame update
    void Start() {
        mesh_filter = gameObject.GetComponent<MeshFilter>();
        if (mesh_filter == null) {
            mesh_filter = gameObject.AddComponent<MeshFilter>();
        }

        mesh_renderer = gameObject.GetComponent<MeshRenderer>();
        if (mesh_renderer == null) {
            mesh_renderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (mesh == null) {
            mesh = new Mesh();
        }

        mesh_filter.sharedMesh = mesh;
        GenerateMesh();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void GenerateMesh() {
        int num_tie = (int)(path_creator.path.length / (tie_depth + tie_dist));
        int num_rail_verts = path_creator.path.NumPoints * 8;
        Vector3[] verts    = new Vector3[num_rail_verts + num_tie * 8];
        Vector2[] uvs      = new Vector2[verts.Length];
        Vector3[] normals  = new Vector3[verts.Length];

        int[] rail_tris = new int[(path_creator.path.NumPoints - 1) * 12 * 3];
        int[] tie_tris  = new int[num_tie * 10 * 3];

        float side_uv = rail_height / (2 * rail_height + rail_width);
        float top_uv  = (rail_height + rail_width) / (2 * rail_height + rail_width);

        //     0 1     2 3
        //    /\/     /\/
        //  -8 -7   -6 -5

        int[] top_surf   = {-8,  0, -7,  1, -7,  0, -6, -5,  2,  3,  2, -5};
        int[] side_surfs = { 4,  0, -8, -4,  4, -8, -7,  1, -3,  5, -3,  1,
                             2,  6, -6,  6, -2, -6,  7,  3, -1, -5, -1,  3};

        for (int i = 0; i < path_creator.path.NumPoints; i++) {
            Vector3 up = transform.InverseTransformVector(Vector3.up);
            Vector3 right = transform.InverseTransformVector(path_creator.path.GetNormal(i));

            Vector3 pos = transform.InverseTransformPoint(path_creator.path.GetPoint(i));

            //    0 1       2 3
            //    4 5       6 7

            int vi = i * 8;

            verts[vi + 0] = pos - 0.5f * rail_dist * right + up * (rail_height + tie_height);
            verts[vi + 1] = pos - (0.5f * rail_dist - rail_width) * right + up * (rail_height + tie_height);

            verts[vi + 2] = pos + 0.5f * rail_dist * right + up * (rail_height + tie_height);
            verts[vi + 3] = pos + (0.5f * rail_dist - rail_width) * right + up * (rail_height + tie_height);

            verts[vi + 4] = pos - 0.5f * rail_dist * right + up * (tie_height);
            verts[vi + 5] = pos - (0.5f * rail_dist - rail_width) * right + up * (tie_height);

            verts[vi + 6] = pos + 0.5f * rail_dist * right + up * (tie_height);
            verts[vi + 7] = pos + (0.5f * rail_dist - rail_width) * right + up * (tie_height);

            uvs[vi + 0] = new Vector2(side_uv, path_creator.path.times[i]);
            uvs[vi + 1] = new Vector2(top_uv,  path_creator.path.times[i]);
            uvs[vi + 2] = new Vector2(side_uv, path_creator.path.times[i]);
            uvs[vi + 3] = new Vector2(top_uv,  path_creator.path.times[i]);
            uvs[vi + 4] = new Vector2(0,    path_creator.path.times[i]);
            uvs[vi + 5] = new Vector2(1.0f, path_creator.path.times[i]);
            uvs[vi + 6] = new Vector2(0,    path_creator.path.times[i]);
            uvs[vi + 7] = new Vector2(1.0f, path_creator.path.times[i]);

            normals[vi + 0] = (up - right).normalized;
            normals[vi + 1] = (up + right).normalized;
            normals[vi + 2] = (up - right).normalized;
            normals[vi + 3] = (up + right).normalized;
            normals[vi + 4] = -right;
            normals[vi + 5] =  right;
            normals[vi + 6] = -right;
            normals[vi + 7] =  right;

            if (i > 0) {
                int tris_idx = (i - 1) * 12 * 3;
                for (int k = 0; k < side_surfs.Length; k++)
                    rail_tris[tris_idx + 0 + k] = vi + 0 + side_surfs[k];
                for (int k = 0; k < top_surf.Length; k++)
                    rail_tris[tris_idx + side_surfs.Length + k] = vi + 0 + top_surf[k];
            }
        }

        // 
        //        0 ----------------- 1  
        //       /|                  /|
        //      2-4                 3-5
        //      |/                  |/
        //      6 ----------------- 7


        float uv_vert_1 = tie_height / (2 * tie_height + tie_depth);
        float uv_vert_2 = (tie_height + tie_depth) / (2 * tie_height + tie_depth);

        int[] tie_tris_indices = {1, 0, 5, 0, 4, 5,
                                  2, 0, 1, 2, 1, 3,
                                  6, 2, 3, 6, 3, 7,
                                  4, 0, 2, 6, 4, 2,
                                  3, 1, 5, 7, 3, 5};

        for (int i = 0; i < num_tie; i++) {
            float t = i / (float)num_tie; 
            Vector3 pos = transform.InverseTransformPoint(path_creator.path.GetPointAtTime(t));
            Vector3 up = transform.InverseTransformVector(Vector3.up);
            Vector3 right = transform.InverseTransformVector(path_creator.path.GetNormal(t));
            Vector3 fwd   = transform.InverseTransformVector(path_creator.path.GetDirection(t));

            int vi = num_rail_verts + i * 8;
            verts[vi + 0] = pos - 0.5f * tie_width * right + tie_height * up + 0.5f * tie_depth * fwd;
            verts[vi + 1] = pos + 0.5f * tie_width * right + tie_height * up + 0.5f * tie_depth * fwd;
            verts[vi + 2] = pos - 0.5f * tie_width * right + tie_height * up - 0.5f * tie_depth * fwd;
            verts[vi + 3] = pos + 0.5f * tie_width * right + tie_height * up - 0.5f * tie_depth * fwd;
            verts[vi + 4] = pos - 0.5f * tie_width * right + 0.5f * tie_depth * fwd;
            verts[vi + 5] = pos + 0.5f * tie_width * right + 0.5f * tie_depth * fwd;
            verts[vi + 6] = pos - 0.5f * tie_width * right - 0.5f * tie_depth * fwd;
            verts[vi + 7] = pos + 0.5f * tie_width * right - 0.5f * tie_depth * fwd;

            normals[vi + 0] = (up - right + fwd).normalized;
            normals[vi + 1] = (up + right + fwd).normalized;
            normals[vi + 2] = (up - right - fwd).normalized;
            normals[vi + 3] = (up + right - fwd).normalized;
            normals[vi + 4] = -(right + fwd).normalized;
            normals[vi + 5] =  (right + fwd).normalized;
            normals[vi + 6] = -(right - fwd).normalized;
            normals[vi + 7] =  (right - fwd).normalized;

            uvs[vi + 0] = new Vector2(0, uv_vert_1);
            uvs[vi + 1] = new Vector2(1, uv_vert_1);
            uvs[vi + 2] = new Vector2(0, uv_vert_2);
            uvs[vi + 3] = new Vector2(1, uv_vert_2);
            uvs[vi + 4] = new Vector2(0, 0);
            uvs[vi + 5] = new Vector2(1, 0);
            uvs[vi + 6] = new Vector2(0, 1);
            uvs[vi + 7] = new Vector2(1, 1);

            int ti = i * 10 * 3;
            for (int k = 0; k < tie_tris_indices.Length; k++)
                tie_tris[ti + k] = vi +  tie_tris_indices[k];
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.subMeshCount = 2;
        mesh.SetTriangles(rail_tris, 0);
        mesh.SetTriangles(tie_tris,  1);
        mesh.RecalculateBounds();
    }
}
