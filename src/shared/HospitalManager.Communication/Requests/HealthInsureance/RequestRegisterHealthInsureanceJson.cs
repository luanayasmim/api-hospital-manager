namespace HospitalManager.Communication.Requests.HealthInsureance;
public class RequestRegisterHealthInsureanceJson
{
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
}
