using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public GameObject prefab;

	protected List<Unit> units = new List<Unit>();

	protected int size = 10;

	private const float interval = 0.1f;
	private float timer = 0;

	private bool isAutomatic = false;
	
	protected virtual void Start() {
		constructGame();
		if (!Directory.Exists(Application.dataPath + "/InitialStates")) {
			Directory.CreateDirectory(Application.dataPath + "/InitialStates");
		}
		initializeGame("233");
	}

	protected virtual void Update() {
		if (isAutomatic) {
			timer += Time.deltaTime;
			if (timer > interval) {
				timer = 0;
				updateUnits();
			}
		}
		else if (Input.GetKeyDown("space")) {
			updateUnits();
		}

		if (Input.GetKeyDown("a")) {
			isAutomatic = !isAutomatic;
		}

		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				Unit hitUnit = hit.transform.gameObject.GetComponent<Unit>();
				hitUnit.switchState();
			}
		}
	}

	protected virtual void updateUnits() {
		foreach (Unit unit in units) {
			unit.updateState();
		}
		foreach (Unit unit in units) {
			unit.updateMemory();
		}
	}

	public void switchAutomatic() {
		isAutomatic = !isAutomatic;
	}

	protected virtual void constructGame() {
		Unit[,] newUnits = new Unit[size, size];
		for (int i=0;i<size;i++) {
			for (int j=0;j<size;j++) {
				GameObject newObject = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
				newUnits[i, j] = newObject.GetComponent<Unit>();
			}
		}
		for (int i=0;i<size;i++) {
			for (int j=0;j<size;j++) {
				if (i > 0) {
					newUnits[i, j].addConnection(newUnits[i - 1, j], true);
				}
				if (j > 0) {
					newUnits[i, j].addConnection(newUnits[i, j - 1], true);
				}
				if (i > 0 && j > 0) {
					newUnits[i, j].addConnection(newUnits[i - 1, j - 1], true);
				}
				if (i < size - 1 && j > 0) {
					newUnits[i, j].addConnection(newUnits[i + 1, j - 1], true);
				}
			}
		}
		for (int i=0;i<size;i++) {
			for (int j=0;j<size;j++) {
				units.Add(newUnits[i, j]);
			}
		}
	}

	protected virtual void initializeGame(string fileName) {
		if (File.Exists(Application.dataPath + "/InitialStates/" + fileName)) {
			string data = File.ReadAllText(Application.dataPath + "/InitialStates/" + fileName);
			data = data.Replace("\n", "");
			for (int i=0;i<units.Count;i++) {
				if (i == data.Length) {
					break;
				}
				units[i].setState(data[i] == '*' ? 1 : 0);
			}
		}
		else {
			Debug.Log("File not found");
		}
	}

}
