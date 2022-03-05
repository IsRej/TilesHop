using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Platform : MonoBehaviour
{
	SphereMovement _spheremovement;
	public ParticleSystem _collidedObj;
	public GameObject _collectives;
	public bool _canSpawnCollectives;
	public Material _oldMat;
	public Material _newMat;
/*
	public Transform jumpEffect;

	public GameObject diamond;*/

	void Awake()
	{
		this._spheremovement = FindObjectOfType<SphereMovement>();
		//jumpEffect.transform.localScale = Vector3.zero;
		//jumpEffect.gameObject.SetActive(false);
		//diamond.SetActive(false);
	}

	void Start()
	{
		int Probability = Random.Range(1,10);
        if (Probability == 1 || Probability == 3)
        {
            if (_canSpawnCollectives)
            {
				_collectives.SetActive(true);
			}
		}
		StartCoroutine(CoroutDestroyIfNotVisible());
	}

	public void OnPlayerBounce()
	{
		_collidedObj.Play();
		transform.DOMoveY(0f,0.15f).OnComplete(() => transform.DOMoveY(0.3f,0.15f));
		GetComponent<MeshRenderer>().material = _newMat;
		//jumpEffect.transform.localScale = Vector3.zero;
		//jumpEffect.gameObject.SetActive(true);

		//jumpEffect.DOScale(new Vector3(3f, 0.3f, 3f), 0.2f).SetLoops(2, LoopType.Yoyo);
	}

	IEnumerator CoroutDestroyIfNotVisible()
	{
		if (_spheremovement._point > 0)
		{
			transform.position = new Vector3(transform.position.x, 10f, transform.position.z);

#if AADOTWEEN
				transform.DOMoveY(0,0.2f);
#endif
		}

		if (_spheremovement._point > 5 && Random.Range(0f, 100f) < 30f)
		{
			//diamond.SetActive(true);
		}

		yield return new WaitForSeconds(1f);

		while (true)
		{
			if (_spheremovement._camGame.transform.position.z - transform.position.z > 5)
			{
				DODestroy();
				break;
			}

			yield return new WaitForSeconds(0.3f);
		}
	}

	void DODestroy()
	{
		int Probability = Random.Range(1, 10);
		if (Probability == 1 || Probability == 3)
		{
			if (_canSpawnCollectives)
			{
				_collectives.SetActive(true);
			}
		}
		GetComponent<MeshRenderer>().material = _oldMat;
		StopAllCoroutines();
		_spheremovement.SpawnPlatform(this.transform);
		StartCoroutine(CoroutDestroyIfNotVisible());
	}
}