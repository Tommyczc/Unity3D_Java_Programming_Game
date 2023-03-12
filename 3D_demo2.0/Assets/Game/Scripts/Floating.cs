using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
	public float radian = 0f; // ����
	public float perRadian = 0.02f; // ÿ�α仯�Ļ���
	public float radius = 0.8f; // �뾶
	public float oxygen_percent=0.5f;
	Vector3 oldPos; // ��ʼʱ�������
					// Use this for initialization
	void Start()
	{
		oldPos = transform.position; // �������λ�ñ��浽oldPos
	}

	// Update is called once per frame
	void Update()
	{
		radian += perRadian; // ����ÿ�μ�0.03
		float dy = Mathf.Sin(radian) * radius; // dy����������y��ı�����Ҳ����ʹ��sin���ҵ�һ���ʺϵ�ֵ�Ϳ���
		transform.position = oldPos + new Vector3(0, dy, 0);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag=="Player") {
			other.gameObject.GetComponent<Health_System>().oxygen_remain += other.gameObject.GetComponent<Health_System>().oxygen_max * oxygen_percent;
			Destroy(this.gameObject);
		}
	}
}
