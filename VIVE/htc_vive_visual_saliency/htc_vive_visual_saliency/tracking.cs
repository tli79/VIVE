using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using System.IO;
using UnityEngine.Rendering.VirtualTexturing;
using System.Text;
using System;
using UnityEditor.SceneManagement;

public class eye_tracking : MonoBehaviour
{
    string filePath = @"C:\Users\joeli\Desktop\Data.csv";
    string delimiter = ",";
    void Start()
    {
        var settings = new TobiiXR_Settings();
        TobiiXR.Start(settings);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }

    // Update is called once per frame
    private void Update()
    {
        // For social use cases, data in local space may be easier to work with
        var eyeTrackingDataLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);

        //If loop to extract valid data
        if (eyeTrackingDataLocal.GazeRay.IsValid)
        {
            // The origin of the gaze ray is a 3D point
            var rayOrigin = eyeTrackingDataLocal.GazeRay.Origin;
            //Seprate XYZ for rayDirection
            float[] origin_output = new float[]{
                rayOrigin.x, rayOrigin.y, rayOrigin.z,
            };

            // The direction of the gaze ray is a normalized direction vector
            var rayDirection = eyeTrackingDataLocal.GazeRay.Direction;
            //Seprate XYZ for rayDirection
            float[] direction_output = new float[]{
                rayDirection.x, rayDirection.y, rayDirection.z,
            };

            //Write to CSV

            using (StreamWriter sw = File.CreateText("gazevector.csv"))
                        {
                            var line = String.Format("{0},{1},{2},{3},{4},{5}", rayOrigin.x, rayOrigin.y, rayOrigin.z, rayDirection.x, rayDirection.y, rayDirection.z);
                            sw.WriteLine(line);
                        }

            int length = direction_output.Length;

            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(direction_output[index].ToString());

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, sb.ToString());
            }
            else
            {
                File.AppendAllText(filePath, sb.ToString());
            }

        }

    }
}