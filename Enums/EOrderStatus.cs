namespace OrdersApi.Enums
{
    public enum EOrderStatus
    {
        Created = 1,
        WaitingPayment = 2,
        Paid = 3,
        Shipping = 4,
        Delivered = 5,
        Canceled = 6
    }
}