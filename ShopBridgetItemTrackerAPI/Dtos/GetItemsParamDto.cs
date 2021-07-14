using System.ComponentModel.DataAnnotations;
using static ShopBridgeItemTrackerAPI.Helper;

namespace ShopBridgeItemTrackerAPI.Dtos
{
    public class GetItemsParamDto
    {
        public GetItemsParamDto()
        {
            PageNo = 1;
            PageSize = 10;
            SortField = "Name";
            SortExp = "asc";
        }
        [RegularExpression(VHelper.AlphaNumRegex, ErrorMessage = VHelper.InvalidMessage)]
        public string Keyword { get; set; }

        [RegularExpression(VHelper.NumberOnlyRegex, ErrorMessage = VHelper.InvalidMessage)]
        public int? PageNo { get; set; }

        [RegularExpression(VHelper.NumberOnlyRegex, ErrorMessage = VHelper.InvalidMessage)]
        public int? PageSize { get; set; }

        [RegularExpression(VHelper.AlphaOnlyRegex, ErrorMessage = VHelper.InvalidMessage)]
        public string SortField { get; set; }

        [RegularExpression(VHelper.AlphaOnlyRegex, ErrorMessage = VHelper.InvalidMessage)]
        public string SortExp { get; set; }
    }
}
