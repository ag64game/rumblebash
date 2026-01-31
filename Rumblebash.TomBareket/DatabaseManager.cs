using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Support.V7.Widget;
using Android.Support.V7;
using Android.Content;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.Res;
using Android.Graphics;
using System.IO;
using Android.Graphics.Drawables;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using Kotlin.Ranges;
using Android.Support.V13.View;
using Android.Icu.Text;
using Android.Views.Animations;
using Android.Media;
using Android.Renderscripts;
using System.Threading;
using System.Timers;
using Firebase;
using Firebase.Database;
using System.Diagnostics.Contracts;
using Android.Drm;
using Java.Interop;
using AndroidX.Browser.Trusted.Sharing;

namespace Rumblebash.TomBareket
{
    public class Score
    {
        public int score { get; set; }
        public string name { get; set;  }
        public string key { get; set; }
        public Score(int toScore = 69, string toName = "Fred")
        {
            score = toScore;
            name = toName;
        }
        public Score(DataSnapshot toRetrieve)
        {
            key = toRetrieve.Key;
            name = toRetrieve.Child("name").Value.ToString();
            score = Convert.ToInt32(toRetrieve.Child("score").Value.ToString());
        }
    }


    public static class AppDataHelper
    {
        public static void GetDatabase()
        {
            FirebaseApp app = FirebaseApp.InitializeApp(MainActivity.main);
            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                    .SetApplicationId("rumblebashhighscores")
                    .SetApiKey("AIzaSyDGtCnMjlkFC7vlw0U2O1DmBN9iTbbsiZw")
                    .SetDatabaseUrl("https://rumblebashhighscores-default-rtdb.europe-west1.firebasedatabase.app")
                    .SetStorageBucket("rumblebashhighscores.appspot.com")
                    .Build();
                app = FirebaseApp.InitializeApp(MainActivity.main, option);
            }
            MainActivity.dbScoreStorage = FirebaseDatabase.GetInstance(app);
        }
    }


    public class ScoreboardListener : Java.Lang.Object, IValueEventListener
    {
        public void OnCancelled(DatabaseError error)
        {

        }
        public void OnDataChange(DataSnapshot snapshot)
        {
            if(snapshot.Value != null)
            {
                var child = snapshot.Children.ToEnumerable<DataSnapshot>();
                MainActivity.scoreboard.Clear();
                foreach (DataSnapshot scoreData in child) MainActivity.scoreboard.Add(new Score(scoreData));
                MainActivity.scoreboard = MainActivity.scoreboard.OrderBy(s => s.score).ToList();
                MainActivity.scoreboard.Reverse();
            }
        }
        public void RetrieveScoresFromDatabase()
        {
            DatabaseReference scoreRef = MainActivity.dbScoreStorage.GetReference("scoreList");
            scoreRef.AddValueEventListener(this);
        }
    }


    public class ScoreboardViewHolder : RecyclerView.ViewHolder
    {
        public TextView tvName { get; }
        public TextView tvScore { get; }
        public ScoreboardViewHolder(View itemView) : base(itemView)
        {
            tvName = itemView.FindViewById<TextView>(Resource.Id.tvScorerNameDisplay);
            tvScore = itemView.FindViewById<TextView>(Resource.Id.tvScoreDisplay);
            tvName.Typeface = MainActivity.nameFont;
            tvScore.Typeface = MainActivity.nameFont;
        }

    public class ScoreAdapter : RecyclerView.Adapter
    {
        List<Score> data;
        public ScoreAdapter(List<Score> data)
        {
            this.data = data;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.score_layout, parent, false);
            return new ScoreboardViewHolder(itemView);
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is ScoreboardViewHolder scoreViewHolder)
            {
                Score item = data[position];
                    scoreViewHolder.tvName.Text = item.name;
                    scoreViewHolder.tvScore.Text = Convert.ToString(item.score);
            }
        }
        public override int ItemCount => data.Count;

    }
}
}