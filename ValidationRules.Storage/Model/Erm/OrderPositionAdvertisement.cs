﻿namespace NuClear.ValidationRules.Storage.Model.Erm
{
    public sealed class OrderPositionAdvertisement
    {
        public long Id { get; set; }
        public long OrderPositionId { get; set; }
        public long PositionId { get; set; }
        public long? AdvertisementId { get; set; }
        public long? FirmAddressId { get; set; }
        public long? CategoryId { get; set; }
        public long? ThemeId { get; set; }
    }
}
