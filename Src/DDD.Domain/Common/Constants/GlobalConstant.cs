namespace DDD.Domain.Common.Constants
{
    public static class GlobalConstant
    {
        public enum SearchEnum
        {
            Category = 0,
            Book = 1, //(Titre,ISBN)
            Editor = 2,
            Author = 3
        }
        public enum JoinRequestState
        {
            Accepted = 1,
            Rejected = 2,
            Pending = 3
        }

        public enum JoinRequestType
        {  /* Contact =0,*/
            Author = 1,
            Editor = 2,
            Competition = 3,
            MothersDay = 4,
            Laureat=5
        }

        public enum PaymentType
        {
            Bank = 0,
            PostOffice = 1
        }

        public enum PaymentReason
        {
            Purshase = 0,
            WalletRefill = 1
        }
    }
}
