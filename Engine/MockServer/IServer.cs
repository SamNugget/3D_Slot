using Slot;

namespace Server
{
    public interface IServer
    {
        public void requestResult(ISlotClient reciever, float bet, Reels.BurgerState[] burgerStates);
    }
}
