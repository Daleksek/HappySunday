using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Collections.Specialized;

namespace HappySunday
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class CookieAwareWebClient : WebClient
         {
             public CookieAwareWebClient()
             {
                 CookieContainer = new CookieContainer();
             }
             public CookieContainer CookieContainer { get; private set; }

             protected override WebRequest GetWebRequest(Uri address)
             {
                 var request = (HttpWebRequest)base.GetWebRequest(address);
                 /*
                 WebProxy proxyObject = new WebProxy("http://10.31.0.1:3128/", true);
                 request.Proxy = proxyObject;*/
                 request.CookieContainer = CookieContainer;
                 return request;
             }
         }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            this.DragMove();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (username.Text != "" && password.Password != "")
            {
                try
                {
                    using (var client = new CookieAwareWebClient())
                    {
                        var values = new NameValueCollection
                        {
                            { "login", username.Text },
                            { "pass", password.Password },
                        };

                        client.UploadValues("http://www.europeconomic.net/", values);

                        // If the previous call succeeded we now have a valid authentication cookie
                        // so we could download the protected page
                        string result = client.DownloadString("http://www.europeconomic.net/index.php?p=ticket");

                        richTextBox1.SelectAll(); richTextBox1.Selection.Text = result.ToString();

                    }
                }
                catch
                {
                    MessageBox.Show("Erreur d'accès au réseau");
                }
            }
            else
            {
                MessageBox.Show("Attention identifiants de connexion vides !");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        }
    }

