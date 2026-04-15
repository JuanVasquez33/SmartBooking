namespace SmartBooking.Application.Services
{
    public record CategoriaServicio(string Icono, string Nombre, string[] Sugerencias);

    public static class CatalogoServicios
    {
        private static readonly Dictionary<string, CategoriaServicio> _catalogo =
            new(StringComparer.OrdinalIgnoreCase)
        {
            ["medicina"] = new("⚕️", "Medicina", new[]
            {
                "Consulta médica general", "Consulta domiciliaria", "Control de tensión",
                "Certificado médico", "Valoración pediátrica", "Teleconsulta"
            }),
            ["medicina general"] = new("⚕️", "Medicina General", new[]
            {
                "Consulta médica general", "Consulta domiciliaria", "Control de tensión",
                "Certificado médico", "Teleconsulta"
            }),
            ["odontología"] = new("🦷", "Odontología", new[]
            {
                "Limpieza dental", "Consulta odontológica", "Blanqueamiento dental",
                "Extracción dental", "Ortodoncia", "Resina dental"
            }),
            ["odontologia"] = new("🦷", "Odontología", new[]
            {
                "Limpieza dental", "Consulta odontológica", "Blanqueamiento dental",
                "Extracción dental", "Ortodoncia", "Resina dental"
            }),
            ["barbería"] = new("✂️", "Barbería", new[]
            {
                "Corte clásico", "Corte + barba", "Afeitado tradicional",
                "Corte a domicilio", "Arreglo de barba", "Coloración"
            }),
            ["barberia"] = new("✂️", "Barbería", new[]
            {
                "Corte clásico", "Corte + barba", "Afeitado tradicional",
                "Corte a domicilio", "Arreglo de barba", "Coloración"
            }),
            ["estética"] = new("💆", "Estética", new[]
            {
                "Limpieza facial", "Manicure", "Pedicure", "Depilación",
                "Tratamiento capilar", "Maquillaje profesional"
            }),
            ["estetica"] = new("💆", "Estética", new[]
            {
                "Limpieza facial", "Manicure", "Pedicure", "Depilación",
                "Tratamiento capilar", "Maquillaje profesional"
            }),
            ["uñas"] = new("💅", "Uñas", new[]
            {
                "Manicure clásico", "Pedicure clásico", "Uñas acrílicas",
                "Uñas en gel", "Nail art", "Retoque de uñas"
            }),
            ["unas"] = new("💅", "Uñas", new[]
            {
                "Manicure clásico", "Pedicure clásico", "Uñas acrílicas",
                "Uñas en gel", "Nail art", "Retoque de uñas"
            }),
            ["psicología"] = new("🧠", "Psicología", new[]
            {
                "Consulta psicológica", "Terapia de pareja", "Terapia familiar",
                "Evaluación psicológica", "Teleconsulta psicológica"
            }),
            ["psicologia"] = new("🧠", "Psicología", new[]
            {
                "Consulta psicológica", "Terapia de pareja", "Terapia familiar",
                "Evaluación psicológica", "Teleconsulta psicológica"
            }),
            ["nutrición"] = new("🥗", "Nutrición", new[]
            {
                "Consulta nutricional", "Plan alimentario", "Seguimiento mensual",
                "Consulta deportiva", "Teleconsulta"
            }),
            ["nutricion"] = new("🥗", "Nutrición", new[]
            {
                "Consulta nutricional", "Plan alimentario", "Seguimiento mensual",
                "Consulta deportiva", "Teleconsulta"
            }),
            ["fitness"] = new("🏋️", "Fitness", new[]
            {
                "Entrenamiento personal", "Clase grupal", "Plan de entrenamiento",
                "Evaluación física", "Sesión a domicilio"
            }),
            ["entrenamiento"] = new("🏋️", "Entrenamiento", new[]
            {
                "Entrenamiento personal", "Clase grupal", "Plan de entrenamiento",
                "Evaluación física", "Sesión a domicilio"
            }),
            ["consultoría"] = new("💼", "Consultoría", new[]
            {
                "Consultoría de software", "Asesoría técnica", "Revisión de código",
                "Auditoría de sistemas", "Mentoría técnica"
            }),
            ["consultoria"] = new("💼", "Consultoría", new[]
            {
                "Consultoría de software", "Asesoría técnica", "Revisión de código",
                "Auditoría de sistemas", "Mentoría técnica"
            }),
            ["desarrollo de software"] = new("💻", "Desarrollo de Software", new[]
            {
                "Desarrollo a la medida", "Consultoría de software", "Revisión de código",
                "Auditoría técnica", "Mentoría de programación", "Soporte técnico"
            }),
            ["abogado"] = new("⚖️", "Derecho", new[]
            {
                "Consulta jurídica", "Asesoría laboral", "Asesoría civil",
                "Revisión de contratos", "Consulta penal", "Trámites legales"
            }),
            ["derecho"] = new("⚖️", "Derecho", new[]
            {
                "Consulta jurídica", "Asesoría laboral", "Asesoría civil",
                "Revisión de contratos", "Consulta penal", "Trámites legales"
            }),
            ["fotografía"] = new("📷", "Fotografía", new[]
            {
                "Sesión de fotos", "Fotografía de eventos", "Fotografía corporativa",
                "Edición de fotos", "Fotografía a domicilio"
            }),
            ["fotografia"] = new("📷", "Fotografía", new[]
            {
                "Sesión de fotos", "Fotografía de eventos", "Fotografía corporativa",
                "Edición de fotos", "Fotografía a domicilio"
            }),
            ["tutoría"] = new("📚", "Tutoría", new[]
            {
                "Clase particular", "Refuerzo escolar", "Preparación de exámenes",
                "Tutoría universitaria", "Clase virtual"
            }),
            ["tutoria"] = new("📚", "Tutoría", new[]
            {
                "Clase particular", "Refuerzo escolar", "Preparación de exámenes",
                "Tutoría universitaria", "Clase virtual"
            }),
        };

        public static CategoriaServicio? ObtenerPorEspecialidad(string? especialidad)
        {
            if (string.IsNullOrWhiteSpace(especialidad)) return null;

            // Búsqueda exacta
            if (_catalogo.TryGetValue(especialidad.Trim(), out var categoria))
                return categoria;

            // Búsqueda parcial
            var key = _catalogo.Keys.FirstOrDefault(k =>
                especialidad.Contains(k, StringComparison.OrdinalIgnoreCase) ||
                k.Contains(especialidad.Trim(), StringComparison.OrdinalIgnoreCase));

            return key != null ? _catalogo[key] : null;
        }

        public static CategoriaServicio Default => new("🛠️", "Servicios", new[]
        {
            "Consulta inicial", "Asesoría personalizada", "Servicio a domicilio",
            "Servicio en local", "Seguimiento"
        });
    }
}
