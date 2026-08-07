using System.Text.Json.Serialization;

namespace UzMarket.ServiceLayer.Services.Integration.Dtos
{
    public class TaxPersonInfoDto
    {
        [JsonPropertyName("ns10Code")]
        public int? Ns10Code { get; set; }

        [JsonPropertyName("ns11Code")]
        public int? Ns11Code { get; set; }

        [JsonPropertyName("shortName")]
        public string ShortName { get; set; } = null!;

        [JsonPropertyName("tin")]
        public string? Tin { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("regDate")]
        public string? RegDate { get; set; }

        [JsonPropertyName("na1Code")]
        public string? Na1Code { get; set; }

        [JsonPropertyName("na1Name")]
        public string? Na1Name { get; set; }

        [JsonPropertyName("statusCode")]
        public string? StatusCode { get; set; }

        [JsonPropertyName("statusName")]
        public string? StatusName { get; set; }

        [JsonPropertyName("mfo")]
        public string? Mfo { get; set; }

        [JsonPropertyName("account")]
        public string? Account { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        [JsonPropertyName("oked")]
        public string? Oked { get; set; }

        [JsonPropertyName("directorTin")]
        public string? DirectorTin { get; set; }

        [JsonPropertyName("directorPinfl")]
        public string? DirectorPinfl { get; set; }

        [JsonPropertyName("director")]
        public string? Director { get; set; }

        [JsonPropertyName("accountant")]
        public string? Accountant { get; set; }

        [JsonPropertyName("isBudget")]
        public int IsBudget { get; set; }

        [JsonPropertyName("taxpayerType")]
        public int TaxpayerType { get; set; }

        [JsonPropertyName("isItd")]
        public bool IsItd { get; set; }

        [JsonPropertyName("personalNum")]
        public string PersonalNum { get; set; } = null!;

        [JsonPropertyName("selfEmployment")]
        public bool SelfEmployment { get; set; }

        [JsonPropertyName("privateNotary")]
        public bool PrivateNotary { get; set; }

        [JsonPropertyName("peasantFarm")]
        public bool PeasantFarm { get; set; }
    }
}
