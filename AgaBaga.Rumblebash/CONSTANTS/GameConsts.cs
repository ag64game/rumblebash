namespace Constants
{
    public static class GameConsts
    {
        //Game board related variables
        public static SlotView[,] svCardSlot = svCardSlot = new SlotView[2, 3];
        public static List<Projectile> lProjectileBank = new List<Projectile>();
        public static TextView[,] tvMonsterStat = new TextView[6, 2];
        public static ImageView ivCombatButton;
        public static MediaPlayer mpCombatHandler;
        public static TextView tvCountDown, tvCountKills;
        public static RelativeLayout rlParentLayout;
        public static Intent iDevil;
        public static System.Timers.Timer mTimer;
        public static int timeElapsed = 20;
        public static int stallCounter = 0;
        public static bool canPress = true;

        //Statistics about the board, as well as settings and internal variables of the board
        public static ImageView ivForeground;
        public static LinearLayout llBackground;
        public static BackgroundScrollView svBgContainer;
        public static Animation aBackgroundMovement, aForegroundMovement;
        public static Typeface nameFont, abilityFont, statsFont, starkillFont;
        public static MediaPlayer mpFightTheme;

        //Combat related statistics
        public const int MAX_TURNS = 20;
        public static bool isInCombat = false;
        public static int kills = 0;
        public static int curKills = 0;
        public static ImageView ivSettingsButton, ivCloseButton;
    }
}