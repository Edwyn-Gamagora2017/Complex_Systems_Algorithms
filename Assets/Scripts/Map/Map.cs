﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a map is a grid graph composed of tiles with different values
public class Map {
	
	// Type of a map tile : it is associated to a value for the tile
	public enum MapTileType{
		Wall, Grass, Water, Rock, Mud, NotDefined
	};
	// Indicates the Weight of each TileType
	public static float mapTileTypeWeight( MapTileType type ){
		switch( type ){
		case MapTileType.Wall: 			return 0;
		case MapTileType.Grass: 		return 1;
		case MapTileType.Water: 		return 2;
		case MapTileType.Rock: 			return 0;
		case MapTileType.Mud: 			return 3;
		case MapTileType.NotDefined:	return 0;
		default:						return 0;
		}
	}

	// Class to be used to store a vertex in the graph
	public class TileInfo : VertexInfo{
		// Map
		public float weight;
		public MapTileType type;
		public int x;
		public int y;
		// Graph
		public float distance;

		public TileInfo( int x, int y, MapTileType type, int index ) : base(index){
			this.x = x;
			this.y = y;
			this.weight = Map.mapTileTypeWeight( type );
			this.type = type;
			this.distance = -1;
		}

		// Graph
		public override float distanceTo (VertexInfo vertex)
		{
			return Mathf.Sqrt( Mathf.Pow( ((TileInfo)vertex).x-this.x, 2 ) + Mathf.Pow( ((TileInfo)vertex).y-this.y, 2 ) );
		}
		public override string toString ()
		{
			return "{ x : "+this.x.ToString()+"; y : "+this.y.ToString()+"; weight : "+this.weight.ToString()+"}";
		}
	};

	int height;							// Map height
	int width;							// Map width
	private bool neighborhood4;			// the map doesnt consider the diagonal for the neighborhood
	private TileInfo[][] mapTiles;		// the grid of tile
	private	Graph graph;				// the representation of the map as a graph

	List<Character> players;			// Players
	List<Character> enemies;			// Enemies

	public Map( int height, int width, bool neighborhood4, bool bidirectional ){
		this.height = height;
		this.width = width;
		this.neighborhood4 = neighborhood4;
		this.players = new List<Character>();
		this.enemies = new List<Character>();

		// Creating empty matrix
		this.mapTiles = new TileInfo[height][];
		for(int y = 0; y < height; y++){
			this.mapTiles[y] = new TileInfo[width];
			for(int x=0; x<width; x++){
				this.mapTiles[y][x] = new TileInfo(x,y,MapTileType.NotDefined,this.graphIndexFromTile(x,y));
			}
		}

		// Each tile is a vertex in the graph
		this.graph = new Graph(width*height,bidirectional,false);
	}

	/* PROPERTIES */
	public List<Character> Players {
		get {
			return players;
		}
	}
	public List<Character> Enemies {
		get {
			return enemies;
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

	// insert the tile in the map matrix and into the graph
	public void addTile( int x, int y, MapTileType type ){
		// Check map bounds
		if( this.isValidTilePosition(x,y) ){
			TileInfo info = new TileInfo( x, y, type, this.graphIndexFromTile(x,y));
			this.mapTiles[y][x] = info;
			this.graph.setVertex( info.VertexIndex, new TileInfo( x, y, type, info.VertexIndex ) );

			// create graph edges
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
				if( this.isUsefulPosition( x-1,y-1 ) ){ // UP LEFT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y-1),1 );
				}
				if( this.isUsefulPosition( x-1,y+1 ) ){ // DOWN LEFT 
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y+1),1 );
				}
				if( this.isUsefulPosition( x+1,y-1 ) ){ // UP RIGHT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y-1),1 );
				}
				if( this.isUsefulPosition( x+1,y+1 ) ){ // DOWN RIGHT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y+1),1 );
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

	public Vector2 getCharacterPosition( Character c ){
		return new Vector2( Mathf.RoundToInt( c.getPosX() ), Mathf.RoundToInt( c.getPosY() ));
	}
	private bool checkCharacterPosition( Character c ){
		return this.isUsefulPosition( (int)getCharacterPosition( c ).x, (int)getCharacterPosition( c ).y );
	}
	public void addPlayer( Character player ){
		this.players.Add( player );
	}
	public void addEnemy( Character enemy ){
		this.enemies.Add( enemy );
	}

	public string toString()
	{
		string res = "Map : {\nheight = "+this.height.ToString ()+"\nwidth = "+this.width.ToString ()+"\n";
		for( int i=0; i<this.height; i++ )
		{
			for( int j=0; j<this.width; j++ )
			{
				res += this.mapTiles[i][j].ToString()+":"+Map.mapTileTypeWeight(this.mapTiles[i][j].type).ToString()+" ";
			}
			res += "\n";
		}
		res += "}\n";

		return res;
	}
	public static Map read( string content )
	{
		/*
		 * File format :
		 * Height Width 0|1(neighborhood4) 0|1(bidirectional)
		 * Height *
		 * 	[Width * MapTileType (index)]
		*/

		// Reading Information
		//try{
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
			bool bidirectional = int.Parse(infoline [3]) == 1;

			Map m = new Map(height, width, neighborhood4, bidirectional);
			
			// Vertices
			for (int y = 0; y < height; y++) {
				string[] mapLine = lines[lineIt];
				lineIt++;
				for( int x = 0; x < width; x++ ){
					m.addTile( x, height-1-y, Map.typeIndexToType( int.Parse(mapLine[x]) ) );
				}
			}

			// Characters
			// Players
			int nPlayers = int.Parse( lines[lineIt][0] );
			lineIt++;
			for( int i=0; i<nPlayers; i++ ){
				Character c = new Character( new Vector2( int.Parse( lines[lineIt][0] ), int.Parse(lines[lineIt][1]) ), m );
				if( m.checkCharacterPosition( c ) ){
					m.addPlayer(c);
				}
				else{
					Debug.LogError( "Map : the position of the player "+ i +" is incorrect." );
				}
				lineIt++;
			}
			// Enemies
			int nEnemies = int.Parse( lines[lineIt][0] );
			lineIt++;
			for( int i=0; i<nEnemies; i++ ){
				Character c = new Character( new Vector2( int.Parse( lines[lineIt][0] ), int.Parse(lines[lineIt][1]) ), m );
				if( m.checkCharacterPosition( c ) ){
					m.addEnemy(c);
				}
				else{
					Debug.LogError( "Map : the position of the enemy "+i+" is incorrect." );
				}
				lineIt++;
			}

			return m;
		/*}catch( System.Exception ex ){
			Debug.LogError ( "Error while opening the Map file : wrong format" );
			Debug.LogError( ex.Message );
			return null;
		}*/
	}
	public static Map readFromFile( string fileName )
	{
		string path = fileName;

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
		return Map.read( reader.ReadToEnd() );
	}
}
