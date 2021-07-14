namespace ShopBridgeItemTrackerAPI
{
    public class Helper
    {
        public struct VHelper
        {
            public const string AlphaNumRegex = @"^[a-zA-Z0-9-&( )_.,]*$";
            public const string AlphaOnlyRegex = @"^[a-zA-Z]*$";
            public const string NumberOnlyRegex = @"^[1-9]\d*$";
            public const string StringRegex = @"^[^<|>]+$";
            public const string AmtRegex = @"^\d\d?[,.]\d\d?$";

            public const string RequiredMessage = "{0} is required";
            public const string InvalidMessage = "{0} is invalid";
            public const string LengthMessage = "Maximum length of {0} should be {1}";
            public const string RangeMessage = "Value of {0} should be between {1} to {2}";
            public const string PhoneRegexMsg = "{0} must have a correct USA phone";
        }
    }
}
