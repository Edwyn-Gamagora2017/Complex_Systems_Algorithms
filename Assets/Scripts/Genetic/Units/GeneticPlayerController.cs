using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticPlayerController : MonoBehaviour {
	
	private float playerMovementTimer = 0;	// Timer for enemy's movement
	public const float tourSeconds = 15;
	public static float moveTimeInSec = 1;	// Interval in which characther's movement is executed
	private GeneticPlayer model;

	GeneticSceneController gController;		// Genetic controller used to find the path
	List<int> targetOrder;					// list of targets to follow
	int currentTarget;						// index in targetOrder list
	List<PathVertexInfo> currentPath;		// list of tiles to follow

	float begin;

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
		ChromosomeSalesman solution = this.model.GeneticController.getSolution();
		targetOrder = solution.Path;
		// Adjust move time to tour time
		Debug.Log( solution.fitness() );
		GeneticPlayerController.moveTimeInSec = tourSeconds/solution.fitness();

		this.initialPositioning();

		this.playerMovementTimer = moveTimeInSec * (this.model.positionInTheMapCost());
		begin = Time.time;
	}

	// Update is called once per frame
	void Update (){
		if( playerMovementTimer <= 0 ){
			executeMovement();
			// TODO Consider edge cost
			playerMovementTimer = moveTimeInSec * (this.model.positionInTheMapCost());
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
		this.transform.position = new Vector3(this.model.getPosX(),this.model.getPosY(),-0.2f);

		// setting path
		this.setCurrentPath();
	}

	void executeMovement(){
		// Move
		if( currentTarget < this.targetOrder.Count ){
			// Current path is finished
			if( currentPath.Count == 0 ){
				currentTarget++;
				if( currentTarget < this.targetOrder.Count ){
					setCurrentPath();
				}
				else{
					// Tour is over
Debug.Log( "Time to finish tour : "+ (Time.time - begin) );
					return;
				}
			}
			MapGenetic.TileInfo v = ((MapGenetic.TileInfo)currentPath[ currentPath.Count-1 ].Vertex);
//Debug.Log("Vertex : "+v.VertexIndex);
			this.model.move( v.x, v.y );
			this.transform.position = new Vector3(this.model.getPosX(),this.model.getPosY(),-0.2f);
			// Removing the last vertex of the path because it was used by the character
			currentPath.RemoveAt( currentPath.Count-1 );
		}
	}

	public void setCurrentPath(){
		int vertexIndex = this.model.Map.getCharacterGraphPosition( this.model.Map.Targets[ this.targetOrder[ currentTarget ] ] );
		int nextvertexIndex = this.model.Map.getCharacterGraphPosition( this.model.Map.Targets[ this.targetOrder[ (currentTarget+1)%this.targetOrder.Count ] ] );
		currentPath = this.model.Map.Graph.pathToFloydWarshall( vertexIndex, nextvertexIndex );

/*Debug.Log( "Path "+currentTarget+"("+vertexIndex+","+nextvertexIndex+") : "+this.model.Map.Graph.getFloydWarshallDistance( vertexIndex, nextvertexIndex ) );
string p = "";
foreach( PathVertexInfo v in currentPath ){
	p+=v.VertexIndex+"("+v.VertexCost+")-";
}
Debug.Log(p);*/

		// Removing the first vertex, which is the current one
		currentPath.RemoveAt( currentPath.Count-1 );
	}
}
