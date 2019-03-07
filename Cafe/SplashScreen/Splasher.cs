﻿using System;
using System.Windows;

namespace Cafe.SplashScreen
{
    public static class Splasher
    {
        private static Window mSplash;

        public static Window Splash
        {
            get
            {
                return mSplash;
            }
            set
            {
                mSplash = value;
            }
        }
        public static void ShowSplash()
        {
            if (mSplash != null)
            {
                mSplash.Show();
            }
        }
        public static void CloseSplash()
        {
            if (mSplash != null)
            {
                mSplash.Close();

                if (mSplash is IDisposable)
                    (mSplash as IDisposable).Dispose();
            }
        }
    }
}
