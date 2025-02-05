﻿using System.Net.Http;

namespace AiPrompts.LMStudio
{
    public class LMStudioSingleton
    {
        private static readonly object SyncLock = new object();

        private static LMStudioSingleton LMStudioSingletonInstance { get; set; }

        public readonly HttpClient HttpClient;

        private LMStudioSingleton()
        {
            HttpClient = new HttpClient();
        }

        public static LMStudioSingleton Instance
        {
            get
            {
                if (LMStudioSingletonInstance == null)
                {
                    lock (SyncLock)
                    {
                        if (LMStudioSingletonInstance == null)
                        {
                            LMStudioSingletonInstance = new LMStudioSingleton();
                        }
                    }
                }
                return LMStudioSingletonInstance;
            }
        }
    }
}
