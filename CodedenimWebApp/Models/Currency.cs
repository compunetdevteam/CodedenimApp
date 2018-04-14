using System.ComponentModel;

namespace CodedenimWebApp.Models
{
    public enum Currency
    {
        [Description("U.S. Dollar")]
        USD = 1,
        [Description("Euro")]
        EUR
    }
}