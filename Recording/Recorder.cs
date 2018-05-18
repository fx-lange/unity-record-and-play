﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RecordAndPlay
{
    public abstract class Recorder : MonoBehaviour
    {
        //folder to store recordings
        protected static string recordingsPath = "DataRecordings";

        //interface via inspector
        public bool doRecord = false;
        public bool doSave = false;

        //private members
        private Recording recording = null;
        private float startTimeSec;
        protected bool isRecording = false;
        
        protected abstract Recording CreateInstance();

        protected void Start()
        {
            doSave = false;
        }

        protected void Update()
        {
            if (!isRecording && doRecord)
            {
                StartRecording();
            }
            else if (isRecording && !doRecord)
            {
                StopRecording();
            }

            if (doSave)
            {
                StopRecording();
                SaveRecording();
                doSave = false;
            }
        }

        void StartRecording()
        {
            recording = CreateInstance();
            recording.recordingName = "UNIQUE NAME";

            startTimeSec = Time.realtimeSinceStartup;
            isRecording = true;
        }

        void StopRecording()
        {
            doRecord = false;
            isRecording = false;
        }

        void SaveRecording()
        {
            if (recording == null || recording.duration <= 0)
            {
                return;
            }

            string path = "Assets/" + recordingsPath;
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets", recordingsPath);
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + recording.recordingName + ".asset");

            AssetDatabase.CreateAsset(recording, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            recording = null;
        }
        
        protected void RecordData(DataFrame dataFrame)
        {
            if (!isRecording)
            {
                return;
            }

            dataFrame.time = Time.realtimeSinceStartup - startTimeSec;

            recording.duration = dataFrame.time; //always as long as the last data frame
            recording.Add(dataFrame);
        }
    }
}

