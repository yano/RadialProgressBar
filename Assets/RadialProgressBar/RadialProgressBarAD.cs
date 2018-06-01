using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AdvancedInspector;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class RadialProgressBarAD: MonoBehaviour
{
	[Inspect(0)] [SerializeField] private RectTransform _canvas;
	[Inspect(1)] [SerializeField] private Image _loadingBar;
	[Inspect(2)] [SerializeField] private Text _text;
	[Inspect(3)] [SerializeField] private int _keyDownCount = 10; //float _duration;
	[Inspect(4), Help(HelpType.None, "falseの場合自分でResetRPB,ShowRPB,StartRPBなどしてタイマーを進める必要がある")]
	[SerializeField] private bool _autoStart = true;

	[Inspect(10)] [SerializeField] private bool _useCompleteEvent;
	public bool UseCompleteEvent()	{ return _useCompleteEvent;	}
	[Inspect("UseCompleteEvent", 11)] [SerializeField]
	private UnityEvent _completeEvent;

	[Inspect(20)] [SerializeField] private Image _imageA;
	[Inspect(21)] [SerializeField] private Image _imageD;
		
	//private float _speed;
	private float _currentAmount = 0; //0.0f;	
	private bool _starting = false;

	private bool _targetIsA = true;

	private AudioSource _audioSource;

	[Inspect(30), Help(HelpType.None, "アサインするとClick時に音が鳴る")] [SerializeField]
	private AudioClip _clickSound;
	
	private void Awake()
	{
		Debug.Assert(_loadingBar != null, "_loadingBar != null", transform);
		Debug.Assert(_text != null, "_text != null", transform);

		Debug.Assert(_imageA != null, "_imageA != null", transform);
		Debug.Assert(_imageD != null, "_imageD != null", transform);

		_audioSource = GetComponent<AudioSource>();
		//_speed = 100.0f / _duration;
		
		HideRPB();
		ResetRPB();

		if (_autoStart)
		{
			ShowRPB();
			StartRPB();
		}
		
		_imageD.color = Color.white;
		_imageA.color = Color.red;
	}

	void Update()
	{
		if (_starting)
		{			
			if (_currentAmount < _keyDownCount)
			{
				if (_targetIsA)
				{
					if (Input.GetKeyDown(KeyCode.A))
					{
						_currentAmount += 1.0f;
						_text.text = Mathf.FloorToInt(100.0f * (_currentAmount / _keyDownCount)).ToString() + "%";
						_imageD.color = Color.red;
						_imageA.color = Color.white;
						_targetIsA = false;
						
						if (_audioSource != null && _clickSound != null)
							_audioSource.PlayOneShot(_clickSound);
					}
				}
				else // targetIsD
				{
					if (Input.GetKeyDown(KeyCode.D))
					{
						_currentAmount += 1.0f;
						_text.text = Mathf.FloorToInt(100.0f * (_currentAmount / _keyDownCount)).ToString() + "%";
						_imageD.color = Color.white;
						_imageA.color = Color.red;
						_targetIsA = true;

						if (_audioSource != null && _clickSound != null)
							_audioSource.PlayOneShot(_clickSound);
					}
				}
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

		_loadingBar.fillAmount = _currentAmount / _keyDownCount;
	}

	public void SetKeyDownCount(int count)
	{
		_keyDownCount = count;
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
