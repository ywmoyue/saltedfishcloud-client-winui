using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace SfcApplication.Services
{
    public class ToastService
    {
        private Panel m_rootElement;
        private const int TOAST_DURATION_TIME = 5000;

        public void Init(Panel rootElement)
        {
            m_rootElement = rootElement;
        }

        public void Show(string title,int duration= TOAST_DURATION_TIME)
        {
            var toast = new TeachingTip
            {
                PreferredPlacement = TeachingTipPlacementMode.Auto,
                Title = title,
                IsLightDismissEnabled = false
            };

            toast.CloseButtonClick += (sender, e) =>
            {
                m_rootElement.Children.Remove(toast);
            };
            m_rootElement.Children.Add(toast);
            toast.IsOpen = true;
            var timer = new Timer(duration);
            timer.Elapsed += (sender, e) =>
            {
                m_rootElement.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
                {
                    m_rootElement.Children.Remove(toast);
                });
                timer.Dispose();
            };
            timer.Start();
        }
    }
}
