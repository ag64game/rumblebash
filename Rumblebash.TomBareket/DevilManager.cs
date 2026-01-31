using Android.App;
using Android.Content;
using Android.Hardware.Lights;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AndroidX.AppCompat.App;
using Android.Support.V7.Widget;
using Android.Support.V7;
using Java.Lang;
using Android.Media;
using System.Timers;
using System.Threading;

    
namespace Rumblebash.TomBareket
{
    [Service]
    public class DevilService : Service
    {

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            Random randPickLine = new Random();
            int choose = randPickLine.Next(1, 4);
            MediaPlayer mpDevilLine = new MediaPlayer();

            switch (choose)
            {
                case 1: 
                    MainActivity.PlayDemon(Resource.Raw.DevilSound1);
                    break;

                case 2:
                    MainActivity.PlayDemon(Resource.Raw.DevilSound2);
                    break;

                case 3:
                    MainActivity.PlayDemon(Resource.Raw.DevilSound3);
                    break;
            }

            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}