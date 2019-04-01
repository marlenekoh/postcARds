using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TBEasyWebCam;
using LitJson;

public class QRDecode : MonoBehaviour
{
	public QRCodeDecodeController e_qrController;

	public Text UiText;

	public GameObject resetBtn;
    public GameObject stopBtn;
    public GameObject scanBtn;

	public GameObject scanLineObj;
	#if (UNITY_ANDROID||UNITY_IOS) && !UNITY_EDITOR
	bool isTorchOn = false;
	#endif
	public Sprite torchOnSprite;
	public Sprite torchOffSprite;
	public Image torchImage;

    public GameObject entryFoundDialog;
    public GameObject entryNotFoundDialog;
    public GameObject entryInvalidDialog;

    private JsonData jsonvale;
    private string card_id;

    private void Start()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.onQRScanFinished += new QRCodeDecodeController.QRScanFinished(this.qrScanFinished);
		}
    }

	private void Update()
	{
	}

	private void qrScanFinished(string id_str)
	{
        int id_int;

        if (id_str != null)
        {
            if (int.TryParse(id_str, out id_int) && id_int >= 0)
            {
                card_id = id_str;
                this.UiText.text = id_str;
                StartCoroutine(GetRequest("https://valiant-postcard.herokuapp.com/retrieve?id=" + id_str));
            } else
            {
                entryInvalidDialog.SetActive(true);
            }
            
        }
		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(true);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(false);
		}

	}

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("ERROR: " + webRequest.error);
            }
            else
            {
                Processjson(webRequest.downloadHandler.text);
            }
        }
    }

    private void Processjson(string jsonString)
    {
        Debug.Log(jsonString);
        jsonvale = JsonMapper.ToObject(jsonString);

        bool hasRecord = (jsonvale["has_record"].ToString().ToLower() == "true");
        DisplayDialog(hasRecord);
    }

    private void DisplayDialog(bool hasRecord)
    {
        if (hasRecord)
        {
            entryFoundDialog.SetActive(true);
        } else
        {
            entryNotFoundDialog.SetActive(true);
        }
    }

    public void Reset()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.Reset();
		}
		if (this.UiText != null)
		{
			this.UiText.text = string.Empty;
		}
		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(false);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(true);
		}
        if (entryFoundDialog != null)
        {
            this.entryFoundDialog.SetActive(false);
        }
        if (entryNotFoundDialog != null)
        {
            this.entryNotFoundDialog.SetActive(false);
        }
        if(entryInvalidDialog != null)
        {
            this.entryInvalidDialog.SetActive(false);
        }
	}

	public void Play()
	{
		Reset ();
		if (this.e_qrController != null)
		{
			this.e_qrController.StartWork();
		}
        if (this.scanBtn != null)
        {
            this.scanBtn.SetActive(false);
        }
        if (this.stopBtn != null)
        {
            this.stopBtn.SetActive(true);
        }
	}

	public void Stop()
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}

		if (this.resetBtn != null)
		{
			this.resetBtn.SetActive(false);
		}
		if (this.scanLineObj != null)
		{
			this.scanLineObj.SetActive(false);
		}
        if (this.scanBtn != null)
        {
            this.scanBtn.SetActive(true);
        }
        if (this.stopBtn != null)
        {
            this.stopBtn.SetActive(false);
        }
    }

	public void GotoNextScene(string scenename)
	{
		if (this.e_qrController != null)
		{
			this.e_qrController.StopWork();
		}

        if (jsonvale != null)
        {
            Session.CreateSession(jsonvale, card_id);
        }

        //Application.LoadLevel(scenename);
        SceneManager.LoadScene(scenename);
	}

	/// <summary>
	/// Toggles the torch by click the ui button
	/// note: support the feature by using the EasyWebCam Component 
	/// </summary>
	public void toggleTorch()
	{
		#if (UNITY_ANDROID||UNITY_IOS) && !UNITY_EDITOR
		if (EasyWebCam.isActive) {
			if (isTorchOn) {
				torchImage.sprite = torchOffSprite;
				EasyWebCam.setTorchMode (TBEasyWebCam.Setting.TorchMode.Off);
			} else {
				torchImage.sprite = torchOnSprite;
				EasyWebCam.setTorchMode (TBEasyWebCam.Setting.TorchMode.On);
			}
			isTorchOn = !isTorchOn;
		}
		#endif
	}
}
