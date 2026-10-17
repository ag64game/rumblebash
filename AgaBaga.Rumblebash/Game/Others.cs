using System;
using System.Collections.Generic;
using System.Text;

namespace AgaBaga.Rumblebash.Game
{
    public static class  Others
    {
        //Has 3 functions all of which related to adding cards to the deck: It can give out a selected amount of random cards from either the entire or a selected card pool, or give a predetermined list of cards
        public static void LoadCards(int amount = 0, Card[] content = null, int[] range = null)
        {
            if (content != null) for (int i = 0; i < content.Count(); i++)
            {
                cardPile.Add(content[i]);
                if (content[i].cAbility != null) content[i].cAbility.OnDrawnFromDeck(content[i]);
            }

            System.Random rand = new System.Random();
            if (range != null) for (int i = 0; i < amount; i++)
            {
                Card newCard = new Card(rand.Next(0, range.Length - 1));
                cardPile.Add(newCard);
                if (newCard.cAbility != null) newCard.cAbility.OnDrawnFromDeck(newCard);
            }

            else for (int i = 0; i < amount; i++)
            {
                Card newCard = new Card(rand.Next(0, Card.CardCount));
                cardPile.Add(newCard);
                if (newCard.cAbility != null) newCard.cAbility.OnDrawnFromDeck(newCard);
            }
            rvDeck.GetAdapter().NotifyDataSetChanged();
        }

        //returns true if none of the cards on board have 0AT / can't attack due to their ability
        public static bool AllCardsNoAT()
        {
            for (int i = 0; i < 2; i++) for (int j = 0; j < 3; j++)
            {
                if (svCardSlot[i, j].slotCard != null && svCardSlot[i, j].slotCard.cAT > 0 && svCardSlot[i, j].slotCard.cAbility.canAttack) return false;
            }

            return true;
        }

        public static void SetKills(int newKills)
        {
            kills = newKills;
            tvCountKills.Text = "Kill: " + Convert.ToString(kills);
        }
    }
}
