using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
	public float radian = 0f; // 弧度
	public float perRadian = 0.02f; // 每次变化的弧度
	public float radius = 0.8f; // 半径
	public float oxygen_percent=0.5f;
	Vector3 oldPos; // 开始时候的坐标
					// Use this for initialization
	void Start()
	{
		oldPos = transform.position; // 将最初的位置保存到oldPos
	}

	// Update is called once per frame
	void Update()
	{
		radian += perRadian; // 弧度每次加0.03
		float dy = Mathf.Sin(radian) * radius; // dy定义的是针对y轴的变量，也可以使用sin，找到一个适合的值就可以
		transform.position = oldPos + new Vector3(0, dy, 0);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag=="Player") {
			other.gameObject.GetComponent<Health_System>().oxygen_remain += other.gameObject.GetComponent<Health_System>().oxygen_max * oxygen_percent;
			Destroy(this.gameObject);
		}
	}
}
