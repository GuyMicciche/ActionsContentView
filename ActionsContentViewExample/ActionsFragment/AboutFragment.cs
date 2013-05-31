using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Net;

namespace ActionsContentViewExample.ActionFragment
{
    public class AboutFragment : Fragment
    {
        public static readonly string TAG = typeof(AboutFragment).FullName;

        private static string ABOUT_SCHEME = "settings";
        private static string ABOUT_AUTHORITY = "about";

        public static Uri ABOUT_URI = new Uri.Builder().Scheme(ABOUT_SCHEME).Authority(ABOUT_AUTHORITY).Build();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View v = inflater.Inflate(Resource.Layout.about, container, false);

            v.FindViewById(Resource.Id.play).SetOnClickListener(new PlayButtonClickListener(this, v));

            return v;
        }

        private class PlayButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private readonly AboutFragment OuterInstance;

            private View v;

            public PlayButtonClickListener(AboutFragment outerInstance, View v)
            {
                this.OuterInstance = outerInstance;
                this.v = v;
            }

            public void OnClick(View v)
            {
                Activity a = OuterInstance.Activity;
                if (a is ExamplesActivity)
                {
                    ExamplesActivity examplesActivity = (ExamplesActivity)a;
                    examplesActivity.UpdateContent(SandboxFragment.SETTINGS_URI);
                }
            }
        }
    }
}