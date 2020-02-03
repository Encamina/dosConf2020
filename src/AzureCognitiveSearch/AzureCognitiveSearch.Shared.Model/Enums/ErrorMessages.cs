namespace AzureCognitiveSearch.Shared.Model.Enums
{
    public static class ErrorMessages
    {
        public static string InvalidNumberOfNotifications => "The number of notifications doesnt match with the origin";
        public static string InvalidAmountInTransaction => "The amount entered is not equal to the amount deducted from pending notifications";
        public static string InvalidAmount => "The amount mustn´t be null";
        public static string InvalidGoalPriorityDuplicate => "Goal priority exist to this customer";
        public static string InvalidInsufficientAmount => "Operation Error. Insufficient amount";
        public static string InvalidExceededGoalAmount => "Has exceeded the maximum amount of the goal";
        public static string InvalidGoalDesactivated => "Goal desactivated, not valid oepration";
        public static string InvalidExceededOrigenAmount => "The transected amount can not exceed the total amount in the goal";
        public static string InvalidMaxRuleNumberSurpassed => "Rule number limit surpassed while trying to add this rule";
        public static string InvalidAccountOwner => "The associated account does not belong to this customer";
        public static string InvalidGoalNotFromCustomer => "The goal isn´t from customer";
        public static string InvalidGoalDetail => "The selected goal does not belong to that customer";
    }
}
