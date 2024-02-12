namespace Map.Domain.Settings;

public class JWTSettings
{
    #region Properties

    public string Secret { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int DurationTime { get; set; }

    #endregion Properties
}