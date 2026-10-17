namespace Constants
{
    public static class OtherConsts
    {
        public static Activity main;

        //Variables for the start menu
        public static ImageView ivTitle;
        public static Button btnStart, btnOptions, btnHigh;
        public static Animation aJumpTitle;
        public static RelativeLayout rlMainMenu;

        //Constant variables for bug fixing
        public static string worksTxt = "-------------------MASSIVE DAY FOR THE UNEMPLOYED------------------", errTxt = "-------------------ERROR------------------";

        //Recycler View variables, both the deck as well as the scoreboard
        public static List<Card> cardPile;
        public static RecyclerView rvDeck, rvScoreboard;
        public static CardAdapter cAdapter;

        //To find abilities easier in case of migration, for Card's constructor
        public const string ABILITY_DIR = "AgaBaga.Rumblebash.Classes.Abilities.";
    }

}