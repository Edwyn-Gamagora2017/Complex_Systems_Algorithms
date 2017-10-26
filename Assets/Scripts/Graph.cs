using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {
	bool bidirectional;
	float[] vertices;
	float[][] adjacency;

	public Graph( int nVertices, bool bidirectional )
	{
		this.bidirectional = bidirectional;
		// Setting Vertices
		this.vertices = new float[nVertices];
		for(int i=0; i<nVertices; i++) {
			this.vertices[i] = 0;
		}
		// Setting Vertices Adjacency
		this.adjacency = new float[nVertices][];
		for(int j=0; j<nVertices; j++) {
			this.adjacency[j] = new float[ nVertices ];
			for(int i=0; i<nVertices; i++) {
				this.adjacency[j][i] = 0;
			}
		}
	}

	public void setVertex( int index, float value )
	{
		this.vertices [index] = value;
	}

	public void setAdjacency( int indexA, int indexB, float value )
	{
		this.adjacency [indexA][indexB] = value;
		if (this.bidirectional) {
			this.adjacency [indexB][indexA] = value;
		}
	}
}
