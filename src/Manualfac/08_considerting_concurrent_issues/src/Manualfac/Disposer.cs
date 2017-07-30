using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class Disposer : Disposable
    {
        Stack<IDisposable> trackedItems = new Stack<IDisposable>();
        readonly object syncObj = new object();

        public void AddItemsToDispose(object item)
        {
            var disposable = item as IDisposable;
            if (disposable == null) return;
            lock (syncObj)
            {
                trackedItems.Push(disposable);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (syncObj)
                {
                    while (trackedItems.Count > 0)
                    {
                        trackedItems.Pop().Dispose();
                    }

                    trackedItems = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}