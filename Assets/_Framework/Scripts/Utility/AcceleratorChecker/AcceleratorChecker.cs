using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework
{
    public class AcceleratorChecker
    {
        public static int maxCheckCount = 3;
        public static int deviation = 20;

        private Action _callback;

        private DateTime dateTime;
        private int checkCount;
        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public AcceleratorChecker()
        {

        }

        public AcceleratorChecker(Action callback)
        {
            _callback = callback;
        }

        public Action callback
        {
            get { return _callback; }
            set { _callback = value; }
        }

        public void Start()
        {
            dateTime = DateTime.Now;
            sw.Reset();
            sw.Start();
            App.timerManager.RegisterLoop(0.03f, Check);
        }

        public void Stop()
        {
            sw.Stop();
            App.timerManager.Unregister(Check);
        }

        private void Check()
        {
            sw.Stop();
            long timeDelay = sw.ElapsedMilliseconds;
            DateTime now = DateTime.Now;
            long dateDelay = (now - dateTime).Milliseconds;

            if (timeDelay - dateDelay > deviation)
            {
                checkCount++;
                if (checkCount >= maxCheckCount)
                    if (_callback != null)
                        _callback.Invoke();
            }
            else
                checkCount = 0;

            dateTime = now;
            sw.Reset();
            sw.Start();
        }
    }
}
