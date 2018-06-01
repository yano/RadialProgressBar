using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AdvancedInspector;
using UnityEngine.Events;

public class RadialProgressBarTimer: MonoBehaviour
{
	[Inspect(0)] [SerializeField] private RectTransform _canvas;
	[Inspect(1)] [SerializeField] private Image _loadingBar;
	[Inspect(2)] [SerializeField] private Text _text;
	[Inspect(3)] [SerializeField] private float _duration;
	[Inspect(4), Help(HelpType.None, "falseの場合自分でResetRPB,ShowRPB,StartRPBなどしてタイマーを進める必要がある")]
	[SerializeField] private bool _autoStart = true;

	[Inspect(10)] [SerializeField] private bool useCompleteEvent;
	public bool UseCompleteEvent()	{ return useCompleteEvent;	}
	[Inspect("UseCompleteEvent", 11)] [SerializeField]
	private UnityEvent _completeEvent; // = new UnityEvent();

		
	private float _speed;
	private float _currentAmount = 0.0f;	
	private bool _starting = false;
		
	private void Awake()
	{
		Debug.Assert(_loadingBar != null, "_loadingBar != null", transform);
		Debug.Assert(_text != null, "_text != null", transform);

		_speed = 100.0f / _duration;
		
		HideRPB();
		ResetRPB();

		if (_autoStart)
		{
			ShowRPB();
			StartRPB();
		}
	}

	void Update()
	{
		if (_starting)
		{
			if (_currentAmount < 100)
			{
				_currentAmount += _speed * Time.deltaTime;
				_text.text = Mathf.FloorToInt(_currentAmount).ToString() + "%";
			}
			else
			{
				_text.text = "100%";
				_starting = false;

				if (UseCompleteEvent())
				{
					_completeEvent.Invoke();
				}
			}
		}

		_loadingBar.fillAmount = _currentAmount / 100.0f;
	}

	public void SetDuration(float duration)
	{
		_duration = duration;
	}
	
	[Inspect(100)]
	public void ShowRPB()
	{
		_canvas.gameObject.SetActive(true);
	}

	[Inspect(101)]
	public void HideRPB()
	{
		_canvas.gameObject.SetActive(false);
	}
	
	[Inspect(102)]
	public void ResetRPB()
	{
		_currentAmount = 0;
		_text.text = "0%";		
	}
	
	[Inspect(103)]
	public void StartRPB()
	{
		_starting = true;
	}
}
