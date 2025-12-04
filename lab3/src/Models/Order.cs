using DeliverySystem.Interfaces;
using DeliverySystem.States;

namespace DeliverySystem.Models
{
    public class Order
    {
        public int Id { get; private set; }
        public Customer Customer { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public DateTime OrderTime { get; private set; }
        public IOrderState State { get; private set; }
        public string DeliveryAddress { get; private set; }
        public bool IsFastDelivery { get; private set; }
        public string SpecialPreferences { get; private set; }

        private IOrderMediator Mediator { get; }

        public Order(int id, Customer customer, List<OrderItem> items, string deliveryAddress, IOrderMediator mediator,
                    bool isFastDelivery = false, string specialPreferences = "")
        {
            Id = id;
            Customer = customer;
            Items = items ?? new List<OrderItem>();
            DeliveryAddress = deliveryAddress;
            IsFastDelivery = isFastDelivery;
            SpecialPreferences = specialPreferences;
            OrderTime = DateTime.Now;
            State = new PendingState();

            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            Mediator.NotifyOrderCreated(this);
        }

        private void ChangeState(IOrderState state)
        {
            var oldState = State;
            State = state;
            State.ProcessOrder(this);

            Mediator.NotifyOrderStateChanged(this, oldState, state);
        }

        public void Approve()
        {
            if (State.GetStatus() != "Ожидает подтверждения")
                throw new InvalidOperationException("Можно подтвердить только ожидающий заказ");

            ChangeState(new PreparingState());
        }

        public void CompletePreparation()
        {
            if (State.GetStatus() != "Готовится")
                throw new InvalidOperationException("Заказ должен быть в состоянии приготовления");

            ChangeState(new ReadyForDeliveryState());
        }

        public void AssignCourier()
        {
            if (State.GetStatus() != "Готов к доставке")
                throw new InvalidOperationException("Заказ должен быть готов к доставке");

            ChangeState(new CourierAssignedState());
        }

        public void StartDelivery()
        {
            if (State.GetStatus() != "Курьер назначен")
                throw new InvalidOperationException("Сначала назначьте курьера");

            ChangeState(new InDeliveryState());
        }

        public void CompleteDelivery()
        {
            if (State.GetStatus() != "В доставке")
                throw new InvalidOperationException("Можно завершить только доставляемый заказ");

            ChangeState(new CompletedState());
        }

        public void Cancel()
        {
            if (State.GetStatus() == "Выполнен" || State.GetStatus() == "В доставке" || State.GetStatus() == "Курьер назначен")
                throw new InvalidOperationException($"Этот заказ уже нельзя отменить: он находится в статусе '{State.GetStatus()}')");

            ChangeState(new CancelledState());
        }

        public string GetStatus() => State.GetStatus();

        public decimal CalculateSubtotal()
        {
            return Items.Sum(item => item.GetTotalPrice());
        }
    }
}
