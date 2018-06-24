using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Subscriber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var notifications = Windows.UI.Notifications;

            // Get the toast notification manager for the current app.
            var notificationManager = notifications.ToastNotificationManager;

            // The getTemplateContent method returns a Windows.Data.Xml.Dom.XmlDocument object
            // that contains the toast notification XML content.
            var template = notifications.toastTemplateType.toastImageAndText01;
            var toastXml = notificationManager.getTemplateContent(notifications.ToastTemplateType[template]);

            // You can use the methods from the XML document to specify the required elements for the toast.
            var images = toastXml.getElementsByTagName("image");
            images[0].setAttribute("src", "images/toastImageAndText.png");

            var textNodes = toastXml.getElementsByTagName("text");
            textNodes.forEach(function(value, index) {
                var textNumber = index + 1;
                var text = "";
                for (var j = 0; j < 10; j++)
                {
                    text += "Text input " + /*@static_cast(String)*/textNumber + " ";
                }
                value.appendChild(toastXml.createTextNode(text));
            });

            // Create a toast notification from the XML, then create a ToastNotifier object
            // to send the toast.
            var toast = new notifications.ToastNotification(toastXml);

            notificationManager.createToastNotifier().show(toast);
        }
    }
}
