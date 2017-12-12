using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a map is a grid graph composed of tiles with different values
public class MapGenetic {

	// Type of a map tile : it is associated to a value for the tile
	public enum MapTileType{
		Wall, Grass, Water, Rock, Mud, NotDefined
	};

	// Class to be used to store a vertex in the graph
	public class TileInfo : VertexInfo{
		// Map
		public MapTileType type;	// type of the tile
		public int x;				// position of the tile in the map : X component
		public int y;				// position of the tile in the map : Y component

		public TileInfo( int x, int y, MapTileType type, float tileCost, int index ) : base(index, tileCost){
			this.x = x;
			this.y = y;
			this.type = type;
		}

		// Graph
		public override float distanceTo (VertexInfo vertex)
		{
			return Mathf.Sqrt( Mathf.Pow( ((TileInfo)vertex).x-this.x, 2 ) + Mathf.Pow( ((TileInfo)vertex).y-this.y, 2 ) );
		}
		public override string toString ()
		{
			return "{ x : "+this.x.ToString()+"; y : "+this.y.ToString()+"; weight : "+this.vertexCost.ToString()+"}";
		}
	};

	int height;							// Map height
	int width;							// Map width
	private bool neighborhood4;			// the map doesnt consider the diagonal for the neighborhood
	private TileInfo[][] mapTiles;		// the grid of tile
	private Dictionary<MapTileType, float> mapTileTypeCost;	// Indicates the Cost of each TileType
	private	Graph graph;				// the representation of the map as a graph

	GeneticPlayer player;						// Player
	List<GeneticTarget> targets;		// Targets

	public MapGenetic( int height, int width, bool neighborhood4, bool bidirectional ){
		this.height = height;
		this.width = width;
		this.neighborhood4 = neighborhood4;
		this.mapTileTypeCost = new Dictionary<MapTileType, float>();
		this.player = new GeneticPlayer( new Vector2(), this );
		this.targets = new List<GeneticTarget>();

		// Creating empty matrix
		this.mapTiles = new TileInfo[height][];
		for(int y = 0; y < height; y++){
			this.mapTiles[y] = new TileInfo[width];
			for(int x=0; x<width; x++){
				this.mapTiles[y][x] = new TileInfo(x,y,MapTileType.NotDefined,this.getMapTileTypeCost(MapTileType.NotDefined),this.graphIndexFromTile(x,y));
			}
		}

		// Each tile is a vertex in the graph
		this.graph = new Graph(width*height,bidirectional,false);
	}

	/* PROPERTIES */
	public GeneticPlayer Player {
		get {
			return player;
		}
	}
	public List<GeneticTarget> Targets {
		get {
			return targets;
		}
	}
	public int Height {
		get {
			return height;
		}
	}
	public int Width {
		get {
			return width;
		}
	}
	/*GETTERS*/
	public MapTileType getTileType(int x, int y){
		if( this.isValidTilePosition(x,y) ){
			return this.mapTiles[y][x].type;
		}
		else{
			return MapTileType.NotDefined;
		}
	}
	// returns the cost for a given type
	public float getMapTileTypeCost( MapTileType type ){
		if( this.mapTileTypeCost.ContainsKey( type ) ){
			return this.mapTileTypeCost[ type ];
		}
		else{
			return float.MaxValue;
		}
	}
	/*
	 * SETTERS
	 */
	// set the cost for a given type
	public void setMapTileTypeCost( MapTileType type, float cost ){
		if( this.mapTileTypeCost.ContainsKey( type ) && type != MapTileType.Wall && type != MapTileType.NotDefined ){
			this.mapTileTypeCost[ type ] = cost;
		}
		else{
			this.mapTileTypeCost.Add(type,cost);
		}
	}

	// insert the tile in the map matrix and into the graph
	public void addTile( int x, int y, MapTileType type ){
		// Check map bounds
		if( this.isValidTilePosition(x,y) ){
			TileInfo info = new TileInfo( x, y, type, this.getMapTileTypeCost( type ), this.graphIndexFromTile(x,y));
			this.mapTiles[y][x] = info;
			this.graph.setVertex( info.VertexIndex, info );

			// create graph edges
			if( this.isUsefulPosition( x,y ) ){	// avoid wall to create edges
				// neighborhood 4 - applied to everyone
				if( this.isUsefulPosition( x,y-1 ) ){ // UP
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x,y-1),1 );
				}
				if( this.isUsefulPosition( x,y+1 ) ){ // DOWN
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x,y+1),1 );
				}
				if( this.isUsefulPosition( x-1,y ) ){ // LEFT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y),1 );
				}
				if( this.isUsefulPosition( x+1,y ) ){ // RIGHT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y),1 );
				}
				// neighborhood 8
				if( !this.neighborhood4 ){
					float edgeWeight = Mathf.Sqrt( 2 );	// sqrt 1*1+1*1
					if( this.isUsefulPosition( x-1,y-1 ) ){ // UP LEFT
						this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y-1), edgeWeight );
					}
					if( this.isUsefulPosition( x-1,y+1 ) ){ // DOWN LEFT 
						this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y+1), edgeWeight );
					}
					if( this.isUsefulPosition( x+1,y-1 ) ){ // UP RIGHT
						this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y-1), edgeWeight );
					}
					if( this.isUsefulPosition( x+1,y+1 ) ){ // DOWN RIGHT
						this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y+1), edgeWeight );
					}
				}
			}
		}
	}

	public bool isValidTilePosition( int x, int y ){
		return( y >= 0 && y < this.height && x >= 0 && x < this.width );
	}
	public bool isDefinedTile( int x, int y ){
		return this.isValidTilePosition( x,y ) && this.mapTiles[y][x].type != MapTileType.NotDefined;
	}
	public bool isUsefulPosition( int x, int y ){
		return this.isDefinedTile( x,y ) && this.mapTiles[y][x].type != MapTileType.Wall;
	}
	private int graphIndexFromTile( int x, int y ){
		return y*this.width + x;
	}

	public static MapTileType typeIndexToType( int typeIndex )
	{
		string[] names = System.Enum.GetNames(typeof(MapTileType));
		if( typeIndex < names.Length ){
			return (MapTileType)typeIndex;
		}
		else{
			return MapTileType.NotDefined;
		}
	}
	public static int typeToTypeIndex( MapTileType type ){
		return (int)type;
	}

	public int getCharacterPositionX( GeneticCharacter c ){
		return Mathf.RoundToInt( c.getPosX() );
	}
	public int getCharacterPositionY( GeneticCharacter c ){
		return Mathf.RoundToInt( c.getPosY() );
	}
	private bool checkCharacterPosition( GeneticCharacter c ){
		return this.isUsefulPosition( getCharacterPositionX(c), getCharacterPositionY(c) );
	}
	public float getCharacterPositionCost( GeneticCharacter c ){
		return this.mapTiles[ getCharacterPositionY(c) ][ getCharacterPositionX(c) ].VertexCost;
	}
	public void setPlayer( GeneticPlayer player ){
		this.player = player;
	}
	public void addTarget( GeneticTarget target ){
		this.targets.Add (target);
	}

	/*
	 * PATH FINDING
	 */
	/*public List<PathVertexInfo> findPathFloydWarshall( Character enemy ){
		if( this.players.Count > 0 ){
			PathVertexInfo targetPathInfo = this.graph.aStar(
				this.graphIndexFromTile( this.getCharacterPositionX( enemy ), this.getCharacterPositionY( enemy ) ),
				this.graphIndexFromTile( this.getCharacterPositionX( this.players[0] ), this.getCharacterPositionY( this.players[0] ) ) );
			if( targetPathInfo != null ){
				return targetPathInfo.pathTo();
			}
		}
		return null;
	}*/

	/*
	 * CHARACTERS
	 */
	public void characterMove( GeneticCharacter c, float newX, float newY ){
		int new_x = Mathf.RoundToInt(newX);
		int new_y = Mathf.RoundToInt(newY);

		// Check if the new Position is a valid Tile
		if( this.isUsefulPosition( new_x, new_y ) ){
			int old_x = Mathf.RoundToInt(c.getPosX());
			int old_y = Mathf.RoundToInt(c.getPosY());
			int oldIndex = this.graphIndexFromTile ( old_x, old_y );
			int newIndex = this.graphIndexFromTile ( new_x, new_y );

			// Check if there is a edge between the new Position and the old one
			if( this.graph.isAdjacent( oldIndex, newIndex ) != null ){
				c.setPos( new Vector2(new_x, new_y) );
			}
		}
	}

	public string toString()
	{
		string res = "Map : {\nheight = "+this.height.ToString ()+"\nwidth = "+this.width.ToString ()+"\n";
		for( int i=0; i<this.height; i++ )
		{
			for( int j=0; j<this.width; j++ )
			{
				res += "["+this.mapTiles[i][j].VertexIndex+": ("+this.mapTiles[i][j].x+","+this.mapTiles[i][j].y+") "+this.mapTiles[i][j].VertexCost+"]\n";
			}
		}
		res += this.graph.toString();
		res += "}\n";

		return res;
	}
	public static MapGenetic read( string content )
	{
		/*
		 * File format :
		 * Height Width 0|1(neighborhood4)
		 * Height *
		 * 	[Width * MapTileType (index)]
		*/

		// Reading Information
		try{
			string[] linesWithComments = content.Split('\n');
			List<string[]> lines = new List<string[]>();
			// filtering comments and empty lines
			for ( int i = 0; i < linesWithComments.Length; i++ ){
				string[] lineSplitSpace = linesWithComments[i].Trim().Split(' ');
				if( lineSplitSpace.Length > 0 && lineSplitSpace[0].Length > 0 && lineSplitSpace[0][0] != '#' ){
					lines.Add( lineSplitSpace );
				}
			}

			int lineIt = 0;

			// initial info
			string[] infoline = lines[lineIt];
			lineIt++;
			int height = int.Parse(infoline [0]);
			int width = int.Parse(infoline [1]);
			bool neighborhood4 = int.Parse(infoline [2]) == 1;
			//bool bidirectional = int.Parse(infoline [3]) == 1;

			MapGenetic m = new MapGenetic(height, width, neighborhood4, true);

			// Vertices Costs
			for (int i = 0; i < 4; i++) {
				// line for the cost
				string[] costLine = lines[lineIt];
				lineIt++;

				m.setMapTileTypeCost( MapGenetic.typeIndexToType( int.Parse(costLine[0]) ), float.Parse( costLine[1] ) );
			}

			// Vertices
			for (int y = 0; y < height; y++) {
				string[] mapLine = lines[lineIt];
				lineIt++;
				for( int x = 0; x < width; x++ ){
				m.addTile( x, height-1-y, MapGenetic.typeIndexToType( int.Parse(mapLine[x]) ) );
				}
			}

			// Characters
			// Players
			int nPlayers = 1;
			//int nPlayers = int.Parse( lines[lineIt][0] );
			//lineIt++;
			for( int i=0; i<nPlayers; i++ ){
				GeneticPlayer p = new GeneticPlayer( new Vector2( int.Parse( lines[lineIt][0] ), int.Parse(lines[lineIt][1]) ), m );
				if( m.checkCharacterPosition( p ) ){
					m.setPlayer( p );
				}
				else{
					Debug.LogError( "Map : the position of the player "+ (i+1) +" is incorrect." );
					throw new UnityEngine.UnityException( "Map : the position of the player "+ (i+1) +" is incorrect." );
				}
				lineIt++;
			}
			// Targets
			int nTargets = int.Parse( lines[lineIt][0] );
			lineIt++;
		for( int i=0; i<nTargets; i++ ){
				GeneticTarget t = new GeneticTarget( new Vector2( int.Parse( lines[lineIt][0] ), int.Parse(lines[lineIt][1]) ), m );
				if( m.checkCharacterPosition( t ) ){
					m.addTarget(t);
				}
				else{
					Debug.LogError( "Map : the position of the enemy "+(i+1)+" is incorrect." );
					throw new UnityEngine.UnityException( "Map : the position of the enemy "+(i+1)+" is incorrect." );
				}
				lineIt++;
			}

			return m;
		}catch( System.Exception ex ){
			Debug.LogError ( "Error while opening the Map file : wrong format" );
			Debug.LogError( ex.Message );
			return null;
		}
	}
	public static MapGenetic readFromFile( string fileName )
	{
		string path = fileName;

		// UNCOMMENT FOR FIXED MAP FILES : WEB BUILD
		//return Map.read( Resources.Load<TextAsset>( fileName ).text );

		// Starting Reader 
		System.IO.StreamReader reader; 
		try{ 
			reader = new System.IO.StreamReader( path ); 
		} 
		catch( System.Exception ex ){ 
			Debug.LogError ( "Error while opening the Map file" ); 
			Debug.LogError( ex.Message ); 
			return null; 
		} 

		// Reading Information 
		return MapGenetic.read( reader.ReadToEnd() ); 
	}
}
