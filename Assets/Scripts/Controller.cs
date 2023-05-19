using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
	public GameObject prefab;
	public Material positive;
	public Material negative;

	private const int width = 48;
	private const int height = 10;
	private const int radius = 10;
	private const int thickness = 2;

	private readonly int[] dx = {0, 1, 0, -1, 1, 1, -1, -1};
	private readonly int[] dy = {1, 0, -1, 0, 1, -1, 1, -1};

	private List<List<bool>> game;
	private List<List<GameObject>> units;

	private const float interval = 0.1f;
	private float timer = 0;
	
	void Start() {
		game = create();
		units = new List<List<GameObject>>();
		for (int i=0;i<width;i++) {
			units.Add(new List<GameObject>());
			for (int j=0;j<height;j++) {
				GameObject unit = Instantiate(prefab, new Vector3(
					(radius + thickness * Mathf.Cos(2 * Mathf.PI  * j / height)) * Mathf.Cos(2 * Mathf.PI * i / width),
					thickness * Mathf.Sin(2 * Mathf.PI * j / height),
					(radius + thickness * Mathf.Cos(2 * Mathf.PI * j / height)) * Mathf.Sin(2 * Mathf.PI * i / width)
				), Quaternion.identity
				);
				units[i].Add(unit);
			}
		}
		set(24, 0, true);

		set(24, 1, true);
		set(22, 1, true);

		set(12, 2, true);
		set(13, 2, true);
		set(20, 2, true);
		set(21, 2, true);
		set(34, 2, true);
		set(35, 2, true);

		set(11, 3, true);
		set(15, 3, true);
		set(20, 3, true);
		set(21, 3, true);
		set(34, 3, true);
		set(35, 3, true);

		set(0, 4, true);
		set(1, 4, true);
		set(10, 4, true);
		set(16, 4, true);
		set(20, 4, true);
		set(21, 4, true);

		set(0, 5, true);
		set(1, 5, true);
		set(10, 5, true);
		set(14, 5, true);
		set(16, 5, true);
		set(17, 5, true);
		set(22, 5, true);
		set(24, 5, true);

		set(10, 6, true);
		set(16, 6, true);
		set(24, 6, true);

		set(11, 7, true);
		set(15, 7, true);

		set(12, 8, true);
		set(13, 8, true);
	}

	void Update() {
		timer += Time.deltaTime;
		if (timer > interval) {
			timer = 0;
			List<List<bool>> memory = clone(game);
			for (int i=0;i<width;i++) {
				for (int j=0;j<height;j++) {
					int count = 0;
					for (int d=0;d<8;d++) {
						count += memory[(i + dx[d] + width) % width][(j + dy[d] + height) % height] ? 1 : 0;
					}
					if ((memory[i][j] && (count == 2 || count == 3)) || (!memory[i][j] && count == 3)) {
						set(i, j, true);
					}
					else {
						set(i, j, false);
					}
				}
			}
		}
	}

	private void set(int i, int j, bool state) {
		game[i][j] = state;
		units[i][j].GetComponent<MeshRenderer>().material = state ? positive : negative;
	}

	private List<List<bool>> create() {
		List<List<bool>> array = new List<List<bool>>();
		for (int i=0;i<width;i++) {
			array.Add(new List<bool>());
			for (int j=0;j<height;j++) {
				array[i].Add(false);
			}
		}
		return array;
	}

	private List<List<bool>> clone(List<List<bool>> origin) {
		List<List<bool>> newArray = create();
		for (int i=0;i<width;i++) {
			for (int j=0;j<height;j++) {
				newArray[i][j] = origin[i][j];
			}
		}
		return newArray;
	}
}
