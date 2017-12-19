using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesmanCity{
	int indexCity;
	int indexVertex;

	public SalesmanCity(int indexCity, int indexVertex){
		this.indexCity = indexCity;
		this.indexVertex = indexVertex;
	}

	public int IndexCity {
		get {
			return indexCity;
		}
	}
	public int IndexVertex {
		get {
			return indexVertex;
		}
	}
}
