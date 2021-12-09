using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Webkit;
using System;
using System.Threading;
using Java.Interop;

namespace App2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            //获取WebView对象
            var webView = FindViewById<WebView>(Resource.Id.webView1);

            webView.LoadUrl("file:///android_asset/index.html");
            //申明WebView的配置
            WebSettings settings = webView.Settings;
            //设置允许执行JS
            settings.JavaScriptEnabled = true;
            //设置可以通过JS打开窗口
            settings.JavaScriptCanOpenWindowsAutomatically = true;
            //这里是自己创建的WebView客户端类
            var webc = new MyCommWebClient();

            //加载javascript接口方法，以便调用前台方法
            //webView.AddJavascriptInterface(new InterfaceForJS(webView, this), "ORAPS");
            webView.AddJavascriptInterface(new AndroidMethod(this), "AndroidMethod");

            //设置自己的WebView客户端
            webView.SetWebViewClient(webc);
  

        

           
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }

      


    }

    class MyCommWebClient : WebViewClient
    {
        //重写页面加载的方法
        public override bool ShouldOverrideUrlLoading(WebView view, String url)
        {
            //使用本控件加载
            view.LoadUrl(url);
            //并返回true
            return true;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            ValueCall vc = new ValueCall();
            //添加弹出返回值事件
            vc.TestEvent += ShowMessage;
            view.EvaluateJavascript("showmessage('安卓按钮点击')", null);
            base.OnPageFinished(view, url);


        }
        public void ShowMessage(string message)
        {
            
           
        }

    }

    //调用页面中的JS代码返回值
    public class ValueCall : Java.Lang.Object, IValueCallback
    {//定义delegate
        public delegate void TestEventHandler(string message);
        //用event 关键字声明事件对象
        public event TestEventHandler TestEvent;
        public void Dispose()
        {

        }

        //重写方法,获取返回值
        public void OnReceiveValue(Java.Lang.Object value)
        {
            string a = value.ToString();
            TestEvent(a);
        }



    }



    public class AndroidMethod : Java.Lang.Object//注意一定要继承java基类
    {
        Activity _activity = null;
        public AndroidMethod(Activity activity)
        {
            _activity = activity;
        }
        /// <summary>
        /// 弹出有消息的提示框（有参无反）
        /// </summary>
        /// <param name="Message"></param>
        [Java.Interop.Export("ShowToast")]//这个是js调用的c#类中方法名
        [JavascriptInterface]//表示这个Method是可以被js调用的
        public void ShowToast(string Message)
        {
            Toast.MakeText(_activity.ApplicationContext, Message, ToastLength.Short).Show();
        }
        /// <summary>
        /// 有参有反
        /// </summary>
        /// <param name="Message"></param>
        [Java.Interop.Export("ReturnMessage")]
        [JavascriptInterface]
        public String ReturnMessage(string Message)
        {
            return Message + "123";
        }
    }

}