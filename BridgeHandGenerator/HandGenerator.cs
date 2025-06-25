using BridgeHandGenerator.deal;
using BridgeHandGenerator.cards;
using BridgeHandGenerator.hand;

namespace BridgeHandGenerator
{
    public class HandGenerator
    {
        private Players_Cards cards;

        public Hand North { get { return cards.North; } }
        public Hand East { get { return cards.East; } }
        public Hand South { get { return cards.South; } }
        public Hand West { get { return cards.West; } }

        public HandGenerator() 
        {
            Deck deck = new Deck();
            cards = deck.Deal();
        }

        public HandGenerator(List<Hand_constraints> hand_constraints)
        {
            Deck deck = new Deck();
            cards = deck.Deal(hand_constraints);
        }
    }
}
