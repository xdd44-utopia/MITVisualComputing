using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutController : Controller
{

	private const int width = 49;
	private const int height = 41;
	private const int radius = 16;
	private const int thickness = 10;
	
	protected override void Start() {
		base.Start();
	}

	protected override void Update() {
		base.Update();
	}

	protected override void constructGame() {
		Unit[,] newUnits = new Unit[width, height];
		for (int i=0;i<width;i++) {
			for (int j=0;j<height;j++) {
				GameObject newObject = Instantiate(prefab, new Vector3(
					(radius + thickness * Mathf.Cos(2 * Mathf.PI  * j / height)) * Mathf.Cos(2 * Mathf.PI * i / width),
					thickness * Mathf.Sin(2 * Mathf.PI * j / height),
					(radius + thickness * Mathf.Cos(2 * Mathf.PI * j / height)) * Mathf.Sin(2 * Mathf.PI * i / width)
					), Quaternion.identity
				);
				newUnits[i, j] = newObject.GetComponent<Unit>();
			}
		}
		for (int i=0;i<width;i++) {
			for (int j=0;j<height;j++) {
				newUnits[i, j].addConnection(newUnits[(i + 1) % width, j], true);
				newUnits[i, j].addConnection(newUnits[i, (j + 1) % height], true);
				newUnits[i, j].addConnection(newUnits[(i + 1) % width, (j + 1) % height], true);
				newUnits[i, j].addConnection(newUnits[(i + 1) % width, (j + height - 1) % height], true);
			}
		}
		for (int i=0;i<width;i++) {
			for (int j=0;j<height;j++) {
				units.Add(newUnits[i, j]);
			}
		}
	}

	// protected override void initializeGame(string fileName) {
	// 	return;
	// }

}
