namespace MagniseTaskNET.Core.Entities
{
    public class Mapping
    {
        public Guid Id { get; set; }

        public string MappingType { get; set; }

        public string Symbol { get; set; }

        public string Exchange { get; set; }

        public int DefaultOrderSize { get; set; }

        public Guid AssetId { get; set; }

        public Asset Asset { get; set; }
    }
}
