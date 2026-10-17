namespace AgaBaga.Rumblebash.CustomComponenets.rvComponents
{
    public class CardAdapter : RecyclerView.Adapter
    {
        List<Card> data;
        public CardAdapter(List<Card> data)
        {
            this.data = data;
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.card_layout, parent, false);
            return new CardViewHolder(itemView);
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is CardViewHolder cardViewHolder)
            {
                Card item = data[position];
                cardViewHolder.tvName.Text = item.cName;
                cardViewHolder.tvHP.Text = Convert.ToString(item.cHP);
                cardViewHolder.tvAT.Text = Convert.ToString(item.cAT);
                cardViewHolder.tvSP.Text = Convert.ToString(item.cSP);
                if (item.cAbility != null) cardViewHolder.tvDesc.Text = item.cAbility.desc; else cardViewHolder.tvDesc.Text = "";
                cardViewHolder.ivPort.SetImageBitmap(item.cPortrait);
            }
        }
        public override int ItemCount => data.Count;

    }

    public class CardViewHolder : RecyclerView.ViewHolder
    {
        public static Card cInStorage;
        public static View vHeldCard;
        public static int iAdapterPosition;
        public TextView tvName { get; }
        public TextView tvHP { get; }
        public TextView tvAT { get; }
        public TextView tvSP { get; }
        public TextView tvDesc { get; }
        public ImageView ivPort { get; }
        public int iPosition { get; set; }
        public CardViewHolder(View itemView) : base(itemView)
        {
            tvName = itemView.FindViewById<TextView>(Resource.Id.tvCardName);
            tvHP = itemView.FindViewById<TextView>(Resource.Id.tvCardHP);
            tvAT = itemView.FindViewById<TextView>(Resource.Id.tvCardAT);
            tvSP = itemView.FindViewById<TextView>(Resource.Id.tvCardSP);
            tvDesc = itemView.FindViewById<TextView>(Resource.Id.tvCardAbility);
            ivPort = itemView.FindViewById<ImageView>(Resource.Id.ivCardPortrait);
            tvName.Typeface = nameFont;
            tvAT.Typeface = statsFont;
            tvSP.Typeface = statsFont;
            tvHP.Typeface = statsFont;
            tvDesc.Typeface = abilityFont;
            itemView.LongClick += CardSelected;
        }

        //Functions handling the movement and placing of cards
        private void CardSelected(object sender, View.LongClickEventArgs e)
        {
            if (isInCombat || isInSettings) return;
            PlaySFX(Resource.Raw.CardPickSFX);

            vHeldCard = (View)sender;
            if (SlotView.crosshair != null) rlParentLayout.RemoveView(SlotView.crosshair);
            if (SlotView.infoTab != null) rlParentLayout.RemoveView(SlotView.infoTab);
            iAdapterPosition = rvDeck.GetChildAdapterPosition(rvDeck.FindChildViewUnder((sender as View).GetX(), (sender as View).GetY()));
            cInStorage = cardPile[iAdapterPosition];

            var item = new ClipData.Item((string)vHeldCard.Tag);
            string[] mimeType = new string[1] { ClipDescription.MimetypeTextPlain };
            var data = new ClipData("Aga", mimeType, item);
            var cardShadow = new View.DragShadowBuilder(vHeldCard);
            vHeldCard.Drag += DragCard;
            vHeldCard.StartDragAndDrop(data, cardShadow, vHeldCard, 0);

        }
        private void DragCard(object sender, View.DragEventArgs e)
        {
            switch (e.Event.Action)
            {
                case DragAction.Started:
                    rvDeck.Visibility = ViewStates.Invisible;
                    break;
                case DragAction.Entered:
                    (sender as View).Invalidate();
                    break;
                case DragAction.Exited:
                    (sender as View).Invalidate();
                    break;
                case DragAction.Drop:
                    rvDeck.Visibility = ViewStates.Visible;
                    break;
                case DragAction.Location:
                    break;
                case DragAction.Ended:
                    rvDeck.Visibility = ViewStates.Visible;
                    break;
            }
        }
    }
}
