namespace NewsArticle.Servicios
{// Services/GeoLocationService.cs
    public class GeoLocationService
    {
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        public void SetLocation(double lat, double lon)
        {
            Latitud = lat;
            Longitud = lon;
        }

        public (double? Lat, double? Lon) GetLocation()
        {
            return (Latitud, Longitud);
        }

        public void ClearLocation()
        {
            Latitud = null;
            Longitud = null;
        }
    }

}
