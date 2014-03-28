using Apeiron.Core.Models;
using CsQuery;
using Newtonsoft.Json;
using NodaTime;
using SmartFormat;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Apeiron.Siiau
{
    public class LectorSiiau
    {

        private readonly string urlRegistros = "http://t09.siiau.udg.mx/wco/sspseca.consulta_oferta?ciclop={CicloEscolar}&cup={CentroUniversitario}&crsep={ClaveMateria}&p_start={PaginacionInicio}&mostrarp={NumRegistros}";
        private readonly string urlFormaConsulta = "http://t09.siiau.udg.mx/wco/sspseca.forma_consulta";
        private readonly List<char> CU = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public async Task<IEnumerable<string>> GetCiclosEscolares()
        {
            HttpClient client = new HttpClient();
            var httpResponse = await client.GetAsync(urlFormaConsulta);
            var payload = await httpResponse.Content.ReadAsStringAsync();

            var queryableHtml = new CQ(payload);

            var options = queryableHtml["select[name='ciclop']"].Children();
            return options.Select(e => e.Value);
        }

        public async Task<Materia> GetMateria(string clave, CentrosUniversitarios centroUniversitario, string cicloEscolar)
        {
            Materia materia = null;

            materia = new Materia();

            HttpClient client = new HttpClient();
            var formattedUrl = Smart.Format(urlRegistros, new { CicloEscolar = cicloEscolar, ClaveMateria = clave, CentroUniversitario = (char)centroUniversitario, PaginacionInicio = 0 });
            var httpResponse = await client.GetAsync(formattedUrl);

            var payload = await httpResponse.Content.ReadAsStringAsync();

            var queryableHtml = new CQ(payload);
            var rows = queryableHtml["body > table:first > tbody > tr"].Skip(2);

            if (rows.Count() == 0)
            {
                return null;
            }

            materia.Clave = clave;
            materia.Nombre = rows.First().Cq()["td:eq(2)"].Text().Replace("~", "Ñ");
            materia.Creditos = int.Parse(rows.First().Cq()["td:eq(4)"].Text());
            materia.Calendario = cicloEscolar;

            foreach (var row in rows)
            {
                var cqRow = CQ.CreateFragment(row.Cq());
                var lugarHoraInfo = cqRow["td:nth-child(8) tr"];
                var grupo = new Grupo();

                if (lugarHoraInfo.Length == 0)
                {
                    continue;
                }

                for (int i = 0; i < lugarHoraInfo.Length; i++)
                {

                    grupo.Materia = materia;
                    grupo.CentroUniversitario = centroUniversitario;
                    grupo.NRC = cqRow["td:eq(0)"].Text();
                    grupo.Seccion = cqRow["td:eq(3)"].Text();
                    grupo.Cupo = int.Parse(cqRow["td:eq(5)"].Text());
                    grupo.CupoDisponible = int.Parse(cqRow["td:eq(6)"].Text());

                    var info = CQ.CreateFragment(cqRow["td:eq(7) tr"].Eq(i));
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

                    var ses_maestro = CQ.CreateFragment(cqRow["td:nth-child(9) tr"]);
                    grupo.Maestro = ses_maestro["td:nth-child(2)"].Text().Replace("~", "Ñ");
                }
                materia.Grupos.Add(grupo);

            }

            return materia;
        }

        private List<IsoDayOfWeek> ParseDias(string diasLetras)
        {
            List<IsoDayOfWeek> dias = new List<IsoDayOfWeek>();

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

        /// <summary>
        /// Este metodo fue para experimentar y practicar, no se recomienda usarlo en realidad, aunque es buena manera de hacer una copia de todo el siiau.
        /// Regresa una lista de objetos en formato json, uno por cada centro universitario.
        /// </summary>
        public async Task<List<string>> LeerTodoSiiau()
        {
            ConcurrentBag<RegistroSiiau> groups = new ConcurrentBag<RegistroSiiau>();

            int numRegistros = 0;

            var actionBlock = new ActionBlock<char>(async cu =>
            {
                int inicioPaginacion = 0;
                string res;
                do
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html"));
                    var formattedUrl = Smart.Format(urlRegistros, new { CentroUniversitario = cu, NumRegistros = numRegistros, PaginacionInicio = inicioPaginacion++ * numRegistros });
                    var httpResponse = await client.GetAsync(formattedUrl);
                    res = await httpResponse.Content.ReadAsStringAsync();

                    var rows = new CQ(res)["body > table:first > tbody > tr"].Skip(2);

                    foreach (var row in rows)
                    {
                        var cqRow = CQ.CreateFragment(row.Cq());
                        var infos = cqRow["td:nth-child(8) tr"];

                        for (int i = 0; i < infos.Length; i++)
                        {
                            var g = new RegistroSiiau();
                            g.CentroUniversitario = (CentrosUniversitarios)cu;
                            g.NRC = cqRow["td:eq(0)"].Text();
                            g.Clave = cqRow["td:eq(1)"].Text();
                            g.Materia = cqRow["td:eq(2)"].Text().Replace("~", "Ñ");
                            g.SeccionId = cqRow["td:eq(3)"].Text();
                            g.Creditos = int.Parse(cqRow["td:eq(4)"].Text());
                            g.Cupo = int.Parse(cqRow["td:eq(5)"].Text());
                            g.CupoDisponible = int.Parse(cqRow["td:eq(6)"].Text());

                            var info = CQ.CreateFragment(cqRow["td:eq(7) tr"].Eq(i));
                            g.Ses = info["td:eq(0)"].Text();
                            g.Horario = info["td:eq(1)"].Text();
                            g.Dias = ParseDias(info["td:eq(2)"].Text());

                            g.Edificio = info["td:eq(3)"].Text();

                            g.Aula = info["td:eq(4)"].Text();

                            g.Periodo = info["td:eq(5)"].Text();

                            var ses2_profesor = CQ.CreateFragment(cqRow["td:nth-child(9) tr"]);
                            g.Ses2 = ses2_profesor["td:nth-child(1)"].Text();
                            g.Profesor = ses2_profesor["td:nth-child(2)"].Text().Replace("~", "Ñ");

                            groups.Add(g);
                        }

                    }


                } while (!(res.Contains("ERROR") || res.Contains("FIN DEL REPORTE")));
            }, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = ExecutionDataflowBlockOptions.Unbounded
            });

            foreach (var cu in CU)
            {
                await actionBlock.SendAsync(cu);
            }

            actionBlock.Complete();
            await actionBlock.Completion;

            var g2 = groups
                .GroupBy(g => g.CentroUniversitario, g => g)
                .Select(g =>
                    new
                    {
                        CentroUniversitario = g.Key,
                        Materias = g
                            .GroupBy(gm => new { Clave = gm.Clave, Nombre = gm.Materia, Creditos = gm.Creditos }, gm => gm)
                            .Select(gm =>
                                new
                                {
                                    Clave = gm.Key.Clave,
                                    Nombre = gm.Key.Nombre,
                                    Creditos = gm.First().Creditos,
                                    Secciones = gm
                                        .GroupBy(gs => gs.SeccionId, gs => gs)
                                        .Select(gs =>
                                            new
                                            {
                                                Seccion = gs.Key,
                                                Cupo = gs.First().Cupo,
                                                CupoDisponible = gs.First().CupoDisponible,
                                                Profesor = gs.First().Profesor,
                                                Horarios = gs.Select(gh =>
                                                    new
                                                    {
                                                        Ses = gh.Ses,
                                                        Hora = gh.Horario,
                                                        Dias = gh.Dias,
                                                        Aula = gh.Aula,
                                                        Periodo = gh.Periodo
                                                    })
                                            }).ToDictionary(gsd => gsd.Seccion)
                                }).ToDictionary(gmd => gmd.Clave)
                    });

            List<string> resultados = new List<string>();
            foreach (var g in g2)
            {
                var s = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(g));
                resultados.Add(s);
            }
            return resultados;
        }
    }
}