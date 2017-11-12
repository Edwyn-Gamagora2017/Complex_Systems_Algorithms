using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

	[SerializeField]
	Game_Manager manager;		// Game Controller that will generate and manipulate the map and players

	[SerializeField]
	EnemyView enemyViewPrefab;	// Prefab that defines the colors that represents the algorithms

	[SerializeField]
	GameObject MapLoadFailPanel;	// Panel to be shown if the map cant be loaded

	[SerializeField]
	UnityEngine.UI.Image aStarIcon;
	[SerializeField]
	UnityEngine.UI.Image dijkstraIcon;
	[SerializeField]
	UnityEngine.UI.Dropdown mapsDropdown; // Used to list maps

	//static string mapsFolderPath = "Maps/"; // Path to the folder that contains the map files // UNCOMMENT FOR FIXED MAP FILES : WEB BUILD
	static string mapsFolderPath = "Assets/Resources/Maps/"; // Path to the folder that contains the map files

	// Use this for initialization
	void Start () {
		listMaps();

		setMapPath();
	}
	
	// Update is called once per frame
	void Update () {
		aStarIcon.color = enemyViewPrefab.AStar;
		dijkstraIcon.color = enemyViewPrefab.Dijkstra;
	}

	public void listMaps(){
		if(mapsDropdown != null){
			mapsDropdown.ClearOptions();
			mapsDropdown.AddOptions( getMapsPaths() );
		}
	}

	// get the list of maps in the folder
	List<string> getMapsPaths(){
		List<string> result = new List<string>();

		// UNCOMMENT FOR FIXED MAP FILES : WEB BUILD
		/*Object[] files = Resources.LoadAll( HUD.mapsFolderPath );
		foreach( Object o in files ){
			result.Add( o.name );
		}*/
		System.IO.DirectoryInfo info = new System.IO.DirectoryInfo( HUD.mapsFolderPath ); 
		if( info.Exists ){ 
			foreach( System.IO.FileInfo f in info.GetFiles() ){ 
				if( !f.Name.EndsWith(".meta") ){ 
					result.Add( f.Name ); 
				} 
			} 
		} 

		return result;
	}

	public void setMapPath(){
		string path = getMapsPaths()[ this.mapsDropdown.value ];

		if( manager != null ){
			bool result = manager.setMapPath( HUD.mapsFolderPath + path );

			if( MapLoadFailPanel != null ){
				MapLoadFailPanel.SetActive( !result );
			}
		}
	}
}