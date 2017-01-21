using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateQuad : MonoBehaviour {

	public Transform[] p;
	public float width;

	MeshFilter mf;
	Mesh mesh;
	float halfWidth;
	Vector3 normal;
	int quads;
	int numVerts;
	int numTris;

	Vector3[] vertices;
	Vector3[] normals;
	Vector2[] uv;
	int[] tri;

	void Awake() {		

		quads = p.Length - 1;
		numVerts = 2 + (quads * 2);
		numTris = 2 * quads;

		mf = GetComponent<MeshFilter>();
		mesh = new Mesh ();
		mf.mesh = mesh;
		halfWidth = width * 0.5f;


		InitVertices ();
		InitNormals ();
		InitUvs ();
		InitIndices ();

		mesh.vertices = vertices;
		mesh.triangles = tri;
		mesh.normals = normals;
		mesh.uv = uv;
	}

	void InitVertices() {
		vertices = new Vector3[numVerts];
	}

	void InitNormals() {
		normals = new Vector3[numVerts];
		for(int i = 0; i < numVerts; ++i){
			normals [i] = -Vector3.forward;
		}
	}

	void InitUvs() {
		uv = new Vector2[numVerts];
		for (int i = 0; i < numVerts; i += 2) {
			//float v = i / (float)(numVerts-2);
			float v = 0.5f;
			uv[i] = new Vector2(0, v);
			uv[i+1] = new Vector2(1, v);
		}
	}

	void InitIndices() {
		tri = new int[numTris * 3];
		for (int i = 0; i < numTris * 3; i+=6) {
			int n = i / 6;

			tri [i] = 2 * n;
			tri [i+1] = 2 * n + 2;
			tri [i+2] = 2 * n + 1;

			tri [i+3] = 2 * n + 2;
			tri [i+4] = 2 * n + 3;
			tri [i+5] = 2 * n + 1;
		}
	}

	void Update() {

		for (int i = 0; i < p.Length; ++i) {
			Vector3 prev = i == 0 ? p [0].position - (p [1].position- p[0].position) : p [i - 1].position;
			Vector3 curr = p [i].position;
			Vector3 next = i == p.Length - 1 ? p [p.Length-1].position + (p [p.Length-1].position - p [p.Length-2].position) : p [i + 1].position;

			GenerateVerts (i * 2, prev, curr, next);
		}

		mesh.vertices = vertices;
	}

	void GenerateVerts(int i, Vector3 prev, Vector3 curr, Vector3 next){
		var prevDirection = curr - prev;
		var nextDirection = next - curr;

		var bisect = (prevDirection + nextDirection) / 2;
		normal.x = bisect.y;
		normal.y = -bisect.x;
		normal.Normalize ();
		normal *= halfWidth;

		vertices[i] = curr - normal;
		vertices[i+1] = curr + normal;
	}
}
