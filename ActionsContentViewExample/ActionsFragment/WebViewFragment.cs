using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace ActionsContentViewExample.ActionFragment
{
    public class WebViewFragment : Fragment
    {
        public static readonly string TAG = typeof(WebViewFragment).FullName;

        private WebView ViewContentWebView;
        private string Url_Renamed;

        private bool ResetHistory = true;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.webview, container, false);

            ProgressBar viewContentProgress = v.FindViewById<ProgressBar>(Resource.Id.progress);
            ViewContentWebView = v.FindViewById<WebView>(Resource.Id.webview);
            ViewContentWebView.Settings.JavaScriptEnabled = true;
            ViewContentWebView.SetWebViewClient(new MyWebViewClient(this));
            ViewContentWebView.SetWebChromeClient(new MyWebChromeClient(this, viewContentProgress));

            return v;
        }

        private class MyWebViewClient : WebViewClient
        {
            private readonly WebViewFragment OuterInstance;

            public MyWebViewClient(WebViewFragment outerInstance)
            {
                this.OuterInstance = outerInstance;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                return base.ShouldOverrideUrlLoading(view, url);
            }
        }

        private class MyWebChromeClient : WebChromeClient
        {
            private readonly WebViewFragment OuterInstance;

            private ProgressBar ViewContentProgress;

            public MyWebChromeClient(WebViewFragment outerInstance, ProgressBar viewContentProgress)
            {
                this.OuterInstance = outerInstance;
                this.ViewContentProgress = viewContentProgress;
            }

            public override void OnProgressChanged(WebView view, int newProgress)
            {
                ViewContentProgress.Progress = newProgress;
                ViewContentProgress.Visibility = newProgress == 100 ? ViewStates.Gone : ViewStates.Visible;

                if (newProgress == 100 && OuterInstance.ResetHistory)
                {
                    OuterInstance.ViewContentWebView.ClearHistory();
                    OuterInstance.ResetHistory = false;
                }
            }
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Reload();
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);

            if (hidden)
            {
                ViewContentWebView.StopLoading();
            }
            else
            {
                Reload();
            }
        }

        public string Url
        {
            set
            {
                this.Url_Renamed = value;

                if (ViewContentWebView != null)
                {
                    ViewContentWebView.StopLoading();
                }

                ResetHistory = true;
            }
        }

        public void Reload()
        {
            if (TextUtils.IsEmpty(Url_Renamed))
            {
                return;
            }

            ViewContentWebView.LoadUrl(Url_Renamed);
        }

        public bool OnBackPressed()
        {
            if (ViewContentWebView.CanGoBack())
            {
                ViewContentWebView.GoBack();

                return true;
            }
            return false;
        }
    }
}