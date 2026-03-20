namespace ImpresionesCB.API.Models
{
    public class ConfiguracionEtiqueta
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Subtitulo { get; set; } = string.Empty;
        public string PreEtiqueta { get; set; } = string.Empty;
    }
}