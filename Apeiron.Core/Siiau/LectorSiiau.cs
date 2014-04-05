using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Apeiron.Core.Models;
using CsQuery;
using NodaTime;
using SmartFormat;

namespace Apeiron.Siiau
{
    public class LectorSiiau
    {
        private HttpClient client;
        private readonly string urlRegistros = "http://t09.siiau.udg.mx/wco/sspseca.consulta_oferta?ciclop={CicloEscolar}&cup={CentroUniversitario}&crsep={ClaveMateria}&p_start={PaginacionInicio}&mostrarp={NumRegistros}";
        private readonly string urlFormaConsulta = "http://t09.siiau.udg.mx/wco/sspseca.forma_consulta";
        private Dictionary<string, string> simpleCache;

        public LectorSiiau()
        {
            this.client = new HttpClient();
            simpleCache = new Dictionary<string, string>();
        }

        public async Task<IEnumerable<string>> GetCiclosEscolares()
        {
            var payload = await GetHtml(urlFormaConsulta, true);
            var queryableHtml = new CQ(payload);

            var options = queryableHtml["select[name='ciclop']"].Children();
            return options.Select(e => e.Value);
        }

        public async Task<Dictionary<string, string>> GetCentrosUniversitarios()
        {
            var payload = await GetHtml(urlFormaConsulta, true);
            var queryableHtml = new CQ(payload);

            var options = queryableHtml["select[name='cup']"].Children();
            return options.Select(e => new { Key = e.Value, Text = e.InnerText }).ToDictionary(o => o.Key, o => o.Text.Replace("~", "Ñ"));
        }

        private async Task<string> GetHtml(string formattedUrl, bool usaCache = false)
        {
            if (usaCache && simpleCache.ContainsKey(formattedUrl))
            {
                return simpleCache[formattedUrl];
            }

            var httpResponse = await this.client.GetAsync(formattedUrl);

            var payload = await httpResponse.Content.ReadAsStringAsync();

            if (usaCache)
            {
                simpleCache.Add(formattedUrl, payload);
            }

            return payload;
        }

        public async Task<List<Materia>> GetMateriasPorCentro(List<string> claves, string cicloEscolar, string centroUniversitario = null)
        {
            List<Materia> materias = new List<Materia>();
            foreach (var clave in claves)
            {
                var materia = await GetMateriaPorCentro(clave, cicloEscolar, centroUniversitario);
                materias.Add(materia);
            }
            return materias;
        }

        public async Task<Materia> GetMateriaPorCentro(string clave, string cicloEscolar, string codigoCentro = null)
        {
            var formattedUrl = Smart.Format(urlRegistros, new { CicloEscolar = cicloEscolar, ClaveMateria = clave, CentroUniversitario = codigoCentro, PaginacionInicio = 0 });

            var payload = await this.GetHtml(formattedUrl);
            var queryableHtml = new CQ(payload);
            var rows = queryableHtml["body > table:first > tbody > tr"].Skip(2).Select(tr => tr.OuterHTML);

            if (rows.Count() == 0)
            {
                return null;
            }

            return ConvierteTRsEnMateria(rows, cicloEscolar, codigoCentro);
        }

        private Materia ConvierteTRsEnMateria(IEnumerable<string> trs, string cicloEscolar, string centroUniversitario = null)
        {
            int i = 0;

            //buscar por todos los centros agrega una columna a los resultados
            if (string.IsNullOrEmpty(centroUniversitario))
            {
                i++;
            }

            Materia materia = new Materia();
            List<Grupo> grupos = new List<Grupo>();
            foreach (var tr in trs)
            {
                var cqRow = CQ.CreateFragment(tr);
                var lugarHoraInfo = cqRow["td:nth-child(" + (i + 8) + ") tr"];
                //este NRC no tiene horario.
                if (lugarHoraInfo.Length == 0)
                {
                    continue;
                }

                var grupo = new Grupo();

                grupo.Materia = materia;

                if (string.IsNullOrEmpty(centroUniversitario))
                {
                    grupo.CentroUniversitario = cqRow["td"].Eq(0).Text();
                }

                grupo.NRC = cqRow["td"].Eq(i).Text();
                materia.Clave = cqRow["td"].Eq(i + 1).Text(); ;
                materia.Nombre = cqRow["td"].Eq(i + 2).Text().Replace("~", "Ñ");
                grupo.Seccion = cqRow["td"].Eq(i + 3).Text();
                materia.Creditos = int.Parse(cqRow["td"].Eq(i + 4).Text());
                materia.Calendario = cicloEscolar;
                grupo.Cupo = int.Parse(cqRow["td"].Eq(i + 5).Text());
                grupo.CupoDisponible = int.Parse(cqRow["td"].Eq(i + 6).Text());

                for (int numInfo = 0; numInfo < lugarHoraInfo.Length; numInfo++)
                {
                    var info = CQ.CreateFragment(cqRow["td:eq(" + (i + 7) + ") tr"].Eq(numInfo));
                    var horas = info["td:eq(1)"].Text().Split('-');

                    LugarHora lg = new LugarHora();
                    lg.HoraInicio = new NodaTime.LocalTime(
                        hour: int.Parse(horas.First().Substring(0, 2)),
                        minute: int.Parse(horas.First().Substring(2, 2)));
                    lg.HoraFin = new NodaTime.LocalTime(
                        hour: int.Parse(horas.Last().Substring(0, 2)),
                        minute: int.Parse(horas.Last().Substring(2, 2)));
                    lg.Dias = ParseDias(info["td:eq(2)"].Text());
                    lg.Edificio = info["td:eq(3)"].Text();
                    lg.Aula = info["td:eq(4)"].Text();

                    grupo.LugaresHoras.Add(lg);
                    grupo.Periodo = info["td:eq(5)"].Text();
                }
                var ses_maestro = CQ.CreateFragment(cqRow["td:nth-child(9) tr"]);
                grupo.Maestro = ses_maestro["td:nth-child(2)"].Text().Replace("~", "Ñ");

                grupos.Add(grupo);
            }
            materia.Grupos = grupos;
            return materia;
        }

        private HashSet<IsoDayOfWeek> ParseDias(string diasLetras)
        {
            var dias = new HashSet<IsoDayOfWeek>();

            foreach (var dia in diasLetras)
            {
                switch (dia)
                {
                    case 'L': dias.Add(IsoDayOfWeek.Monday); break;
                    case 'M': dias.Add(IsoDayOfWeek.Tuesday); break;
                    case 'I': dias.Add(IsoDayOfWeek.Wednesday); break;
                    case 'J': dias.Add(IsoDayOfWeek.Thursday); break;
                    case 'V': dias.Add(IsoDayOfWeek.Friday); break;
                    case 'S': dias.Add(IsoDayOfWeek.Saturday); break;
                    default:
                        break;
                }
            }
            return dias;
        }

    }
}