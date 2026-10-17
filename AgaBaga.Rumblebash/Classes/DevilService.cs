namespace AgaBaga.Rumblebash.Classes
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

            System.Random randPickLine = new System.Random();
            int choose = randPickLine.Next(1, 4);
            MediaPlayer mpDevilLine = new MediaPlayer();

            switch (choose)
            {
                case 1:
                    PlayDemon(Resource.Raw.DevilSound1);
                    break;

                case 2:
                    PlayDemon(Resource.Raw.DevilSound2);
                    break;

                case 3:
                    PlayDemon(Resource.Raw.DevilSound3);
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
