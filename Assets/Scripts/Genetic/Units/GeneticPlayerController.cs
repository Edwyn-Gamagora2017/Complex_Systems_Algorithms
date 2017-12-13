using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticPlayerController : MonoBehaviour {
	
	private float playerMovementTimer = 0;	// Timer for enemy's movement
	public const int moveTimeInSec = 1;	// Interval in which characther's movement is executed
	private GeneticPlayer model;

	GeneticSceneController gController;		// Genetic controller used to find the path
	List<int> targetOrder;					// list of targets to follow
	int currentTarget;						// index in targetOrder list
	List<PathVertexInfo> currentPath;		// list of tiles to follow

	public GeneticPlayer Model {
		set {
			model = value;
		}
		get {
			return model;
		}
	}
		
	// Use this for initialization
	void Start () {
		// Path
		targetOrder = this.model.GeneticController.getSolution().Path;

		this.initialPositioning();

		this.playerMovementTimer = moveTimeInSec * (this.model.positionInTheMapCost()+1);
	}

	// Update is called once per frame
	void Update (){
		if( playerMovementTimer <= 0 ){
			executeMovement();
			// TODO Consider edge cost
			playerMovementTimer = Character_Controller.moveTimeInSec * (this.model.positionInTheMapCost()+1);
		}
		else{
			playerMovementTimer -= Time.deltaTime;
		}
	}

	void initialPositioning(){
		currentTarget = 0;

		// poitioning the player at the first target
		int vertexIndex = this.model.Map.getCharacterGraphPosition( this.model.Map.Targets[ this.targetOrder[ currentTarget ] ] );
		MapGenetic.TileInfo vertex = this.model.Map.getTile( vertexIndex );
		this.model.setPos( new Vector2( vertex.x, vertex.y ));
		this.transform.position = new Vector3(this.model.getPosX(),this.model.getPosY(),-0.1f);

		// setting path
		int nextvertexIndex = this.model.Map.getCharacterGraphPosition( this.model.Map.Targets[ this.targetOrder[ (currentTarget+1)%this.targetOrder.Count ] ] );
		currentPath = this.model.Map.Graph.pathToFloydWarshall( vertexIndex, nextvertexIndex );
	}

	void executeMovement(){
		// Move
		if( currentTarget < this.targetOrder.Count ){
			// Current path is finished
			if( currentPath.Count > 0 ){
				MapGenetic.TileInfo v = ((MapGenetic.TileInfo)currentPath[ currentPath.Count-1 ].Vertex);
				this.model.move( v.x, v.y );
				this.transform.position = new Vector3(this.model.getPosX(),this.model.getPosY(),-0.1f);
				// Removing the last vertex of the path because it was used by the character
				currentPath.RemoveAt( currentPath.Count-1 );
			}
			else{
				currentTarget++;
				int vertexIndex = this.model.Map.getCharacterGraphPosition( this.model.Map.Targets[ this.targetOrder[ currentTarget ] ] );
				int nextvertexIndex = this.model.Map.getCharacterGraphPosition( this.model.Map.Targets[ this.targetOrder[ (currentTarget+1)%this.targetOrder.Count ] ] );
				currentPath = this.model.Map.Graph.pathToFloydWarshall( vertexIndex, nextvertexIndex );
			}
		}
		// TODO ti;e doubled on target
		// TODO finish tour
	}
}
