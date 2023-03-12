using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_System : MonoBehaviour
{
    private float health = 100f;
    public float oxygen_max = 600;
    public float time_gap = 1f;// the seconds that update the oxygen and the health
    public GameObject HP_canvas;
    public GameObject HP_bar;
    public GameObject oxygen_container;
    public GameObject oxygen_bar;

    private float lastTime;   //timer
    private float curTime;
    private float current_oxy_length;

    public GameObject gameInteraction;
	private float oxygen;
	public float HP
	{
		set
		{
			if (value >= 100f)
			{
				health = 100f;
			}
			else if (value <= 0f)
			{
				health = 0f;
				gameLost();
			}
			else { health = value; }

			HP_bar.GetComponent<ProgressBar>().ChangeValue(health);
		}
		get { return health; }
	}
	
	public float oxygen_remain
	{
		get { return oxygen; }
		set
		{
			if (value >= oxygen_max)
			{
				oxygen = oxygen_max;
			}
			else if (value <= 0f)
			{
				oxygen = 0f;
				HP -= 1;
			}
			else
			{
				oxygen = value;
			}
			updateOxygen(oxygen);
		}
	}
	// Start is called before the first frame update
	void Start()
    {
		HP = 100f;
		oxygen_remain = oxygen_max;
		HP_canvas.SetActive(false);
		current_oxy_length = 1;
		lastTime = Time.time;
	}

    // Update is called once per frame
    void Update()
    {
		curTime = Time.time;
		if (oxygen_remain > 0)
		{
			if (curTime - lastTime >= time_gap)
			{
				if (HP < 100)
				{
					if (!HP_canvas.activeSelf) {
						HP_canvas.SetActive(true);
					}
					HP += 1;
				}
				else if (HP==100 && HP_canvas.activeSelf) {
					HP_canvas.SetActive(false);
				}
				oxygen_remain--;
				lastTime = curTime;
			}
		}

		else if (HP > 0 && oxygen_remain <= 0)
		{
			if (!HP_canvas.activeSelf) { HP_canvas.SetActive(true); }
			if (curTime - lastTime >= time_gap)
			{
				HP--;
				lastTime = curTime;
			}

		}
	}

	private void updateOxygen(float oxygen)
	{
		float rate = (float)oxygen / oxygen_max;
		oxygen_bar.transform.localScale = new Vector3(oxygen_bar.transform.localScale.x, rate * current_oxy_length, oxygen_bar.transform.localScale.z);

	}

	private void gameLost()
	{
		Debug.Log("game over!!");
		gameInteraction.gameObject.GetComponent<level1_allInteraction>().game_lost();
	}
}
