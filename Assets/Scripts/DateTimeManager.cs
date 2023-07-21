using System;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class DateTimeManager : MonoBehaviour
{
    public void GetServerDateTime()
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "GetServerDateTime", // Replace with your PlayFab Cloud Script function name
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptError);
    }

    private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
    {
        if (result.FunctionResult != null)
        {
            var jsonResult = (PlayFab.Json.JsonObject)result.FunctionResult;

            if (jsonResult.TryGetValue("ServerDateTime", out var serverDateTimeValue))
            {
                string serverDateTimeString = serverDateTimeValue.ToString();
                DateTime serverDateTime = DateTime.Parse(serverDateTimeString);

                // Now you have the server's current date and time in the 'serverDateTime' variable.
                // You can use this value in your game as needed.
                Debug.Log("Server's current date and time: " + serverDateTime);
            }
            else
            {
                Debug.LogError("ServerDateTime key not found in the Cloud Script response.");
            }
        }
        else
        {
            Debug.LogError("Cloud Script response is null or empty.");
        }
    }

    private void OnCloudScriptError(PlayFabError error)
    {
        Debug.LogError("Error executing Cloud Script: " + error.ErrorMessage);
    }
}