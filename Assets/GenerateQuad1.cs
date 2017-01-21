using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateQuad1 : MonoBehaviour {

	public GameObject begin;
	public GameObject end;
	public float width;
	public int quads;

	MeshFilter mf;
	Mesh mesh;
	float halfWidth;
	Vector3 normal;
	Vector3 direction;
	int numVerts;
	int numTris;

	Vector3[] vertices;
	Vector3[] normals;
	Vector2[] uv;
	int[] tri;

	void Awake() {
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
			float v = i / (float)(numVerts-2);
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

		direction = end.transform.position - begin.transform.position;

		normal.x = direction.y;
		normal.y = -direction.x;
		normal.Normalize ();
		normal *= halfWidth;

		for (int i = 0; i < numVerts; i += 2) {
			float t = i / (float)(numVerts-2);
			vertices[i] = begin.transform.position + (direction*t) - normal;
			vertices[i+1] = begin.transform.position + (direction*t) + normal;
		}
			
		mesh.vertices = vertices;
	}
}
