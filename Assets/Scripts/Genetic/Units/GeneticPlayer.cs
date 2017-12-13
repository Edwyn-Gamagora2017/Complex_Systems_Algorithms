using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticPlayer : GeneticCharacter {
	
	GeneticSceneController geneticController;

	public GeneticPlayer( Vector2 position, MapGenetic map, GeneticSceneController gController ):base( position, map ){
		this.geneticController = gController;
	}

	public GeneticSceneController GeneticController {
		get {
			return geneticController;
		}
	}
}
