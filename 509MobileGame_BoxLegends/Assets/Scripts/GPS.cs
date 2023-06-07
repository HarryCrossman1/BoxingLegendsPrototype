using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GPS : MonoBehaviour
{

    private string countryCode;
   [SerializeField] private Text GpsNotWorking;

    //Requires multiple presses for some reason. still cannot fix :(
    public void GetLocationData()
    {
        StartCoroutine(GetCountryCode());
    }

    IEnumerator GetCountryCode()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Player hasn't turned on location");
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.LogError("Timed out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("cant get  location");
            yield break;
        }

        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;

        string url = "https://nominatim.openstreetmap.org/reverse?format=json&lat=" + latitude + "&lon=" + longitude;

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Cant get location data");
            yield break;
        }

        string json = www.downloadHandler.text;
        LocationData data = JsonUtility.FromJson<LocationData>(json);

        countryCode = data.address.country_code;

        Debug.Log("Current location: " + data.display_name);
        Debug.Log("Country code: " + countryCode);

        if (countryCode == "de")
        {
            Debug.Log("Germany");
            UiControllerV2.ImageIndex = 0;
        }
        else if (countryCode == "us")
        {
            Debug.Log("United States");
            UiControllerV2.ImageIndex = 2;
        }
        else if (countryCode == "gb")
        {
            Debug.Log("United Kingdom was chosen");
            UiControllerV2.ImageIndex = 1;
        }
        else
        {
            Debug.Log("OtherCountry/NaughtyGPS");
            GpsNotWorking.enabled = true;
            StartCoroutine(ErrorMessage(3));
        }

        Input.location.Stop();
    }

    private IEnumerator ErrorMessage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GpsNotWorking.enabled = false;
    }
}

[System.Serializable]
public class LocationData
{
    public string display_name;
    public AddressData address;
}

[System.Serializable]
public class AddressData
{
    public string country;
    public string country_code;
}