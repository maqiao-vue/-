using System;
using System.Collections;
using System.Collections.Generic;
using Emgu.CV;
using Emgu.CV.Structure;
using UnityEngine;

public class text : MonoBehaviour
{
    private Capture _capture;
    private Mat _frame;

    // Start is called before the first frame update
    void Start()
    {
        _capture = new Capture("rtsp://admin:12345@192.168.0.150:8554/0");
        _frame = new Mat();

    }

    // Update is called once per frame
    void Update()
    {
        if (_capture != null && _capture.Ptr != IntPtr.Zero)
        {
            _capture.Grab();
            _capture.Retrieve(_frame, 0);
            int ff = 1;
        }
    }
    void OnDestroy()
    {
        if (_capture != null)
        {
            _capture.Dispose();
        }

        if (_frame != null)
        {
            _frame.Dispose();
        }
    }
}