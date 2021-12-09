using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App2
{
    public class InterfaceForJS : Java.Lang.Object
    {
        private WebView webView;
        private Context context;

        public InterfaceForJS(WebView webView, Context context)
        {
            this.webView = webView;
            this.context = context;
        }

        [Export]
        [JavascriptInterface]
        public void test()
        {
            ((Activity)context).RunOnUiThread(() =>
                        webView.LoadUrl("javascript:test('world')"));

            Toast.MakeText(context, "hello", ToastLength.Short).Show();
        }
    }
}