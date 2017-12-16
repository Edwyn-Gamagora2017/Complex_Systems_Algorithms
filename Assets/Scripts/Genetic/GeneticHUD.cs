using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticHUD : MonoBehaviour {

	/*
	 * Singleton
	*/
	static GeneticHUD instance;

	// To be executed before the component starts
	void Awake () {
		if( instance != null ){
			this.showSolution = instance.showSolution;
			Destroy( instance.gameObject );
		}
		instance = this;

		GameObject.DontDestroyOnLoad( this.gameObject );
	}
	/*
	 *	END SINGLETON
	 */

	bool showSolution = true;

	public bool ShowSolution {
		get {
			return showSolution;
		}
	}

	[SerializeField]
	UnityEngine.UI.Button generationButton;
	[SerializeField]
	UnityEngine.UI.Button solutionButton;

	[SerializeField]
	MapGeneticController gController;
	[SerializeField]
	UnityEngine.UI.Button backButton;
	[SerializeField]
	UnityEngine.UI.Button nextButton;
	[SerializeField]
	UnityEngine.UI.Button replayButton;

	[SerializeField]
	UnityEngine.UI.Text fitnessText;
	[SerializeField]
	UnityEngine.UI.Text generationText;

	void backScene(){
		UnityEngine.SceneManagement.SceneManager.LoadScene( "GeneticSceneMenu" );
	}
	void generationScene(){
		showSolution = false;
		UnityEngine.SceneManagement.SceneManager.LoadScene( "GeneticScene" );
	}
	void solutionScene(){
		showSolution = true;
		UnityEngine.SceneManagement.SceneManager.LoadScene( "GeneticScene" );
	}
	void next(){
		gController.nextGeneration();
	}
	void replay(){
		gController.replayGeneration();
	}

	// Use this for initialization
	void Start () {
		if( generationButton != null ){
			generationButton.onClick.AddListener( generationScene );
		}
		if( solutionButton != null ){
			solutionButton.onClick.AddListener( solutionScene );
		}
		if( backButton != null ){
			backButton.onClick.AddListener( backScene );
		}
		if( nextButton != null ){
			nextButton.onClick.AddListener( next );
			if( !showSolution ){
				nextButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Next Generation";
			}
			else{
				nextButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "New Solution";
			}
		}
		if( replayButton != null ){
			replayButton.onClick.AddListener( replay );
			if( !showSolution ){
				replayButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Replay Generation";
			}
			else{
				replayButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "Replay Solution";
			}
		}

		if( gController != null ){
			gController.selectShowSolutionMode( this.showSolution );
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( fitnessText != null ){
			if( gController != null ){
				float fitness = gController.getSolutionFitness();
				if( fitness < 0 ){
					fitnessText.text = "";
				}
				else{
					fitnessText.text = "Fitness Solution:\n"+Mathf.RoundToInt( fitness );
				}
			}
			else{
				fitnessText.text = "";
			}
		}
		if( generationText != null ){
			if( gController != null && !showSolution ){
				generationText.text = "Generation:\n"+(gController.getCurrentGeneration()+1)+"/"+gController.getMaxGeneration();
			}
			else{
				generationText.text = "";
			}
		}
		if( nextButton != null ){
			if( !showSolution && gController.getCurrentGeneration() == gController.getMaxGeneration()-1 ){
				nextButton.interactable = false;
			}
		}
	}
}
