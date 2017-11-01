using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a map is a grid graph composed of tiles with different values
public class Map {
	
	// Type of a map tile : it is associated to a value for the tile
	public enum MapTileType{
		Wall, Grass, Water, Rock, Mud, NotDefined
	};
	// Stores the value for each tile type
	private Dictionary<MapTileType,float> MapTileValue;

	int height;							// Map height
	int width;							// Map width
	private MapTileType[][] mapTiles;	// the grid of tile types that represents the tiles
	private	Graph graph;				// the representation of the map as a graph
	private bool neighborhood4;			// the map doesnt consider the diagonal for the neighborhood

	public Map( int height, int width, bool neighborhood4, bool bidirectional ){
		this.generateMapTileValue();

		this.height = height;
		this.width = width;

		// Creating empty matrix
		this.mapTiles = new MapTileType[height][];
		for(int i = 0; i < height; i++){
			this.mapTiles[i] = new MapTileType[width];
			for(int j = 0; j < width; j++){
				this.mapTiles[i][j] = MapTileType.NotDefined;
			}
		}
		this.neighborhood4 = neighborhood4;

		// Each tile is a vertex in the graph
		this.graph = new Graph(width*height,bidirectional,false);
	}

	// Fills the value for each tile type
	private void generateMapTileValue()
	{
		this.MapTileValue = new Dictionary<MapTileType,float>();
		this.MapTileValue.Add(MapTileType.Wall,0);
		this.MapTileValue.Add(MapTileType.Grass,1);
		this.MapTileValue.Add(MapTileType.Water,2);
		this.MapTileValue.Add(MapTileType.Rock,0);
		this.MapTileValue.Add(MapTileType.Mud,3);
	}

	public void addTile( int x, int y, MapTileType type ){
		// Check map bounds
		if( isValidTilePosition(x,y) ){
			this.mapTiles[y][x] = type;
			this.graph.setVertex( this.graphIndexFromTile(x,y), this.MapTileValue[ type ] );

			// create graph edges
				// neighborhood 4 - applied to everyone
			if( this.isDefinedTile( x,y-1 ) ){ // UP
				this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x,y-1),1 );
			}
			if( this.isDefinedTile( x,y+1 ) ){ // DOWN
				this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x,y+1),1 );
			}
			if( this.isDefinedTile( x-1,y ) ){ // LEFT
				this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y),1 );
			}
			if( this.isDefinedTile( x+1,y ) ){ // RIGHT
				this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y),1 );
			}
				// neighborhood 8
			if( !this.neighborhood4 ){
				if( this.isDefinedTile( x-1,y-1 ) ){ // UP LEFT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y-1),1 );
				}
				if( this.isDefinedTile( x-1,y+1 ) ){ // DOWN LEFT 
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x-1,y+1),1 );
				}
				if( this.isDefinedTile( x+1,y-1 ) ){ // UP RIGHT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y-1),1 );
				}
				if( this.isDefinedTile( x+1,y+1 ) ){ // DOWN RIGHT
					this.graph.setAdjacency( this.graphIndexFromTile(x,y),this.graphIndexFromTile(x+1,y+1),1 );
				}
			}
		}
	}

	public bool isValidTilePosition( int x, int y )
	{
		return( y >= 0 && y < this.height && x >= 0 && x < this.width );
	}
	public bool isDefinedTile( int x, int y ){
		return isValidTilePosition( x,y ) && this.mapTiles[y][x] != MapTileType.NotDefined;
	}
	public bool isUsefulPosition( int x, int y ){
		return isDefinedTile( x,y ) && this.mapTiles[y][x] != MapTileType.Wall;
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

	/*GETTERS*/
	public Graph getGraph {
		get {
			return graph;
		}
	}

	public string toString()
	{
		string res = "Map : {\nheight = "+this.height.ToString ()+"\nwidth = "+this.width.ToString ()+"\n";
		for( int i=0; i<this.height; i++ )
		{
			for( int j=0; j<this.width; j++ )
			{
				res += this.mapTiles[i][j].ToString()+":"+this.MapTileValue[this.mapTiles[i][j]].ToString()+" ";
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
		try{
			string[] lines = content.Split('\n');

			// initial info
			string[] line = lines[0].Split(' ');
			int height = int.Parse(line [0]);
			int width = int.Parse(line [1]);
			bool neighborhood4 = int.Parse(line [2]) == 1;
			bool bidirectional = int.Parse(line [3]) == 1;

			Map m = new Map(height, width, neighborhood4, bidirectional);

			// Vertices
			for (int i = 0; i < height; i++) {
				string[] mapLine = lines[1+i].Split(' ');
				for( int j = 0; j < width; j++ ){
					m.addTile( j, i, Map.typeIndexToType( int.Parse(mapLine[j]) ) );
				}
			}

			return m;
		}catch( System.Exception ex ){
			Debug.LogError ( "Error while opening the Map file : wrong format" );
			return null;
		}
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
			return null;
		}

		// Reading Information
		return Map.read( reader.ReadToEnd() );
	}
}
