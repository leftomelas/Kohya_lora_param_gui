﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kohya_lora_trainer
{
    public static class BatchProcess
    {
        public static Stack<string> BatchStack = new Stack<string>();
        public static bool IsRunning = false;
        public static bool IsCancel = false;
        public static bool LogResultText = false;
        public static int SkippedCount = 0;
        public static int CompletedCount = 0;
        public static int FailCount = 0;
        public static string LogText = string.Empty;
        public static bool ShuffleCaptionsBeforeTraining = false;
        public static int KeepTokensCount = 0;

        public static void LoadBatchPresetText(string text)
        {
            BatchStack.Clear();
           string newtext = text.Replace("\r\n", "\n");

            List<string> lines = new List<string>(newtext.Split(char.Parse("\n")));
            for (int i = 0; i < lines.Count; i++)
            {
                if (i >= lines.Count) break;
                if (string.IsNullOrWhiteSpace(lines[i]) || string.IsNullOrEmpty(lines[i]))
                {
                    lines.RemoveAt(i);
                    i--;
                }

            }

            for (int i = lines.Count - 1; i >= 0; i--)
            {
                BatchStack.Push(lines[i]);
            }
        }

        public static void ShuffleCaptions()
        {
            if (!ShuffleCaptionsBeforeTraining || TrainParams.Current == null || !IsRunning || TrainParams.Current.ShuffleCaptions || !Directory.Exists(TrainParams.Current.TrainImagePath))
                return;
            try
            {
                string[] subDirs = Directory.GetDirectories(TrainParams.Current.TrainImagePath);
                foreach (string dir in subDirs)
                {
                    MyUtils.ShuffleCaptions(dir, KeepTokensCount, false);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Shuffle Errored: " + ex.Message);
            }
        }
    }
}
