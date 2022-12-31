using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WPFUI.Constants;

public static class AcceptableTypesFormatConstants
{
    public const NumberStyles numberStyleForLong = NumberStyles.Integer | NumberStyles.AllowThousands;
    public const NumberStyles numberStyleForDecimals = NumberStyles.Float | NumberStyles.AllowThousands;

    public static readonly IReadOnlyList<string> listOfGroupSeparators = new List<string>()
    {
        ",",
        " ",
        "_"
    };

    internal readonly static IReadOnlyList<CultureCurrencyInfo> get_currencyData = InitializeCurrencyDictionary();

    private static IReadOnlyList<CultureCurrencyInfo> InitializeCurrencyDictionary()
    {
        CultureInfo[] array = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

        List<CultureCurrencyInfo> output = array.Select(cultInfo => cultInfo.LCID).Distinct()
            .Select(lcid => new RegionInfo(lcid))
            .DistinctBy(regionInfo => regionInfo.ISOCurrencySymbol)
            .Select(regionInfo => new CultureCurrencyInfo(regionInfo.ISOCurrencySymbol, regionInfo.CurrencySymbol, regionInfo.CurrencyEnglishName))
            .OrderBy(customInfo => customInfo.get_CurrencySymbol)
            .ToList();

        return output;
    }
}

public record class CultureCurrencyInfo(string get_ISOCurrencySymbol, string get_CurrencySymbol, string get_CurrencyEnglishName);