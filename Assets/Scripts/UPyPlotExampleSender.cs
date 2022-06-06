using UnityEngine;

public class UPyPlotExampleSender : MonoBehaviour {


	[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
	private float time;

	[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
	private float yVar;

	//[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
	//private float xVar; 

	//[UPyPlot.UPyPlotController.UPyProbe] // Add probe so this value will be plotted.
	//[Range(-10,10)]                      // Add a manual control slider so its easy to change in real time.
	//[SerializeField] public float yVar;  // A public variable that user can adjust manually and see in plot.

	private float lastRndX = 0;
	private float lastRndZ = 0;
    private object tmElapsed;

    private void Start()
    {
        
	}

    void Update () {
		// Some example code that makes the values change in the plot.
		//xVar = Mathf.Lerp(lastRndX, Random.Range (0.0f, 10.0f), Time.deltaTime * 0.5f );
		//lastRndX = xVar;

		//zVar = Mathf.Lerp(lastRndZ, Random.Range(-10.0f, 10.0f), Time.deltaTime * 0.5f );
		//lastRndZ = zVar;
		//xVar = this.transform.position.x;
		yVar = TurbineSetting.WindOutputValue;
		//time = ((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f) + ((float)DateTime.Now.Second * 0.01f));
		//time = ((float)DateTime.Now.Month + ((float)DateTime.Now.Day) + ((float)DateTime.Now.Hour * 0.01f) + ((float)DateTime.Now.Hour * 0.01f));

			

		//zVar = this.transform.position.z;


	}

}
