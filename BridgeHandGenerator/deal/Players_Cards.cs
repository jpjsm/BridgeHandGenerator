using BridgeHandGenerator.hand;

namespace BridgeHandGenerator
{
    public readonly record struct Players_Cards(Hand North, Hand East, Hand South, Hand West);
}