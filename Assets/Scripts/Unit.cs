using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public GameObject linePrefab;

	private List<Unit> adj = new List<Unit>();
	private List<LineRenderer> lines = new List<LineRenderer>();

	public Material positive;
	public Material negative;
	public Material lockDeadMat;
	public Material lockLiveMat;

	private enum State {
		dead,
		live,
		lockdead,
		locklive
	}

	private State current = State.dead;
	private State memory = State.dead;

	void Start() {
		
	}

	void Update() {
	}

	public void addConnection(Unit other, bool first) {
		adj.Add(other);
		GameObject newLine = Instantiate(linePrefab, this.transform);
		lines.Add(newLine.GetComponent<LineRenderer>());
		lines[lines.Count - 1].startWidth = 0.1f;
		lines[lines.Count - 1].endWidth = 0.1f;
		lines[lines.Count - 1].SetPosition(0, transform.position);
		lines[lines.Count - 1].SetPosition(1, transform.position);
		if (first) {
			other.addConnection(this, false);
		}
	}

	public void setState(int s) {
		switch (s) {
			case 0: current = State.dead; break;
			case 1: current = State.live; break;
			case 2: current = State.lockdead; break;
			case 3: current = State.locklive; break;
		}
		memory = current;
		updateVisual();
	}

	public void switchState() {
		switch (current) {
			case State.dead: {
				current = State.live;
				break;
			}
			case State.live: {
				current = State.lockdead;
				break;
			}
			case State.lockdead: {
				current = State.locklive;
				break;
			}
			case State.locklive: {
				current = State.dead;
				break;
			}
		}
		memory = current;
		updateVisual();
	}

	public void updateState() {
		if (current == State.locklive || current == State.lockdead) {
			return;
		}
		int count = 0;
		foreach (Unit unit in adj) {
			count += (unit.memory == State.live || unit.memory == State.locklive)  ? 1 : 0;
		}
		if (((memory == State.live || memory == State.locklive) && (count == 2 || count == 3)) || ((memory == State.dead || memory == State.lockdead)  && count == 3)) {
			current = State.live;
		}
		else {
			current = State.dead;
		}
	}

	public void updateMemory() {
		if (memory != current) {
			memory = current;
			updateVisual();
		}
	}

	private void updateVisual() {
		switch (current) {
			case State.dead: {
				GetComponent<MeshRenderer>().material = negative;
				disableConnectionLines();
				break;
			}
			case State.live: {
				GetComponent<MeshRenderer>().material = positive;
				enableConnectionLines();
				break;
			}
			case State.lockdead: {
				GetComponent<MeshRenderer>().material = lockDeadMat;
				disableConnectionLines();
				break;
			}
			case State.locklive: {
				GetComponent<MeshRenderer>().material = lockLiveMat;
				enableConnectionLines();
				break;
			}
		}
	}

	private void enableConnectionLines() {
		for (int i=0;i<adj.Count;i++) {
			lines[i].SetPosition(1, adj[i].transform.position);
		}
	}

	private void disableConnectionLines() {
		for (int i=0;i<adj.Count;i++) {
			lines[i].SetPosition(1, transform.position);
		}
	}

}
